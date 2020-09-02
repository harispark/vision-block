#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <windows.h>
#include <cmath>
#include <vector>
#include <ctime>
#include <stack>

#include "libIHM.h"
#include "WpfTools.h"

CImageNdg GrayscaleToBinary(CImageNdg img)
{
	for (int i = 0; i < img.lireNbPixels(); i++)
	{
		int newPixelValue = 0;
		if (img(i) > 0)
			newPixelValue = 1;
		img(i) = newPixelValue;
	}
	return img;
}

CImageNdg BinaryToGrayscale(CImageNdg img)
{
	for (int i = 0; i < img.lireNbPixels(); i++)
	{
		int newPixelValue = 0;
		if (img(i))
			newPixelValue = 255;
		img(i) = newPixelValue;
	}
	return img;
}

void Addition(byte* data, int stride, int nbLig, int nbCol, byte* data2, int stride2, int nbLig2, int nbCol2)
{
	CImageNdg img = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);
	CImageNdg img2 = ConvertImgFromWpfNdg(data2, stride2, nbLig2, nbCol2);

	for (int i = 0; i < img.lireNbPixels(); i++)
	{
		int sum = (int)img2(i) + (int)img(i);
		if (sum > 255)
		{
			sum = 255;
		}
		img(i) = sum;
	}
	ConvertWpfFromNdg(img, data, stride, nbLig, nbCol);
}

void ColorToNdg(byte* data, int stride, int nbLig, int nbCol, int plan)
{
	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);

	byte* pixPtr = (byte*)data;
	if (plan != 1 && plan != 2 && plan != 3)
		plan = 0;
	CImageNdg toNdg = img.plan(1);

	ConvertWpfFromNdg(toNdg, data, stride, nbLig, nbCol);
}

void Moyennage(byte* data, int stride, int nbLig, int nbCol)
{
	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);

	byte* pixPtr = (byte*)data;
	CImageNdg moyenne = img.plan(0).filtrage("moyennage",8,8);

	ConvertWpfFromNdg(moyenne, data, stride, nbLig, nbCol);
}

void Seuillage(byte* data, int stride, int nbLig, int nbCol, int valSeuilMin, int valSeuilMax)
{
	CImageNdg img = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);
	CImageNdg seuil = img.seuillage("automatique", valSeuilMin, valSeuilMax);

	for (int i = 0; i < seuil.lireNbPixels(); i++)
		seuil(i) = (unsigned char)(255 * (int)seuil(i));

	ConvertWpfFromNdg(seuil, data, stride, nbLig, nbCol);
}

void GetPlan(byte* data, int stride, int nbLig, int nbCol, int nbPlan)
{
	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);
	if (nbPlan > 3) {
		nbPlan = nbPlan-4;
		img = img.conversion("HSV");
	}
	CImageNdg cimg = img.plan(nbPlan);
	ConvertWpfFromNdg(cimg, data, stride, nbLig, nbCol);
}

void Inversion(byte* data, int stride, int nbLig, int nbCol)
{
	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);

	for (int i = 0; i < img.lireNbPixels(); i++) {
		img(i)[0] = (unsigned char)(255 - (int)img(i)[0]);
		img(i)[1] = (unsigned char)(255 - (int)img(i)[1]);
		img(i)[2] = (unsigned char)(255 - (int)img(i)[2]);
	}
		
	ConvertWpfFromColor(img, data, stride, nbLig, nbCol);
}

MOMENTS GetImageData(byte* data, int stride, int nbLig, int nbCol)
{
	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);
	return img.plan(3).signatures();
}

MOMENTS* GetImageDataRGB(byte* data, int stride, int nbLig, int nbCol)
{
	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);
	return img.signatures().data();
}

void Filtrage(byte* data, int stride, int nbLig, int nbCol, int surfaceMin, int surfaceMax, bool miseAZero)
{
	CImageNdg img = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);
	CImageClasse imgClasse = CImageClasse(img,"V8");
	imgClasse = imgClasse.filtrage("taille",surfaceMin,surfaceMax,miseAZero);
	ConvertWpfFromClass(imgClasse, data, stride, nbLig, nbCol);
}

void OperateurBinaire(byte* data, int stride, int nbLig, int nbCol, byte* data2, int stride2, int nbLig2, int nbCol2, char* operateurBinaire)
{
	CImageNdg img = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);
	CImageNdg img2 = ConvertImgFromWpfNdg(data2, stride2, nbLig2, nbCol2);
	
	img = GrayscaleToBinary(img);
	img2 = GrayscaleToBinary(img2);

	img.ecrireBinaire(true);
	img2.ecrireBinaire(true);

	std::string operateur(operateurBinaire);

	img = BinaryToGrayscale(img.operation(img2, operateur));

	ConvertWpfFromNdg(img, data, stride, nbLig, nbCol);
}

void Morphologie(byte* data, int stride, int nbLig, int nbCol, int indexMorpho, int v4v8)
{
	CImageNdg img = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);
	if (v4v8 == 0) {
		if (indexMorpho == 0) {
			img = img.morphologie("erosion","V4");
		}
		else {
			img = img.morphologie("dilatation","V4");
		}
	}
	else if (v4v8 == 1) {
		if (indexMorpho == 0) {
			img = img.morphologie("erosion", "V8");
		}
		else {
			img = img.morphologie("dilatation", "V8");
		}
	}
	
	ConvertWpfFromNdg(img, data, stride, nbLig, nbCol);
}

void HistogrammeNdg(byte* data, int stride, int nbLig, int nbCol, int **hist)
{
	CImageNdg img = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);
	*hist = static_cast<int*>(calloc(256,sizeof(int)));
	std::vector<int> vec = img.histogrammeInt(false, 1);
	memcpy(*hist, vec.data(), 256*sizeof(int));
}

void HistogrammeRgb(byte* data, int stride, int nbLig, int nbCol, int **hist1, int **hist2, int **hist3)
{
	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);
	CImageNdg R = img.plan(0);
	CImageNdg G = img.plan(1);
	CImageNdg B = img.plan(2);

	*hist1 = static_cast<int*>(calloc(256, sizeof(int)));
	*hist2 = static_cast<int*>(calloc(256, sizeof(int)));
	*hist3 = static_cast<int*>(calloc(256, sizeof(int)));

	std::vector<int> vec1 = R.histogrammeInt(false, 1);
	std::vector<int> vec2 = G.histogrammeInt(false, 1);
	std::vector<int> vec3 = B.histogrammeInt(false, 1);

	memcpy(*hist1, vec1.data(), 256 * sizeof(int));
	memcpy(*hist2, vec2.data(), 256 * sizeof(int));
	memcpy(*hist3, vec3.data(), 256 * sizeof(int));
}

void Lut(byte* data, int stride, int nbLig, int nbCol, byte* lut)
{
	CImageNdg img = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);
	
	for (int i = 0; i < img.lireNbPixels(); i++)
		img(i) = lut[img(i)];

	ConvertWpfFromNdg(img,data, stride, nbLig, nbCol);
}

void MoyennePonderee(byte* data, int stride, int nbLig, int nbCol, byte* data2, int stride2, int nbLig2, int nbCol2, double moyennePondeeree)
{
	CImageNdg img = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);
	CImageNdg img2 = ConvertImgFromWpfNdg(data2, stride2, nbLig2, nbCol2);

	for (int i = 0; i < img.lireNbPixels(); i++)
	{
		byte sum1 = (byte)(moyennePondeeree*(double)img(i));
		byte sum2 = (byte)((1 - moyennePondeeree)*(double)img2(i));
		img(i) = sum1 + sum2;
	}
	ConvertWpfFromNdg(img, data, stride, nbLig, nbCol);
}

void RgbFromPlans(byte* dataR, int strideR, int nbLigR, int nbColR, byte* dataG, int strideG, int nbLigG, int nbColG, byte* dataB, int strideB, int nbLigB, int nbColB)
{
	CImageNdg imgR = ConvertImgFromWpfNdg(dataR, strideR, nbLigR, nbColR);
	CImageNdg imgG = ConvertImgFromWpfNdg(dataG, strideG, nbLigG, nbColG);
	CImageNdg imgB = ConvertImgFromWpfNdg(dataB, strideB, nbLigB, nbColB);

	CImageCouleur imgRGB = CImageCouleur(imgG.lireHauteur(), imgG.lireLargeur(),-1,-1,-1);

	for (int i = 0; i < imgR.lireNbPixels(); i++)
	{
		imgRGB(i)[0] = imgR(i);
		imgRGB(i)[1] = imgG(i);
		imgRGB(i)[2] = imgB(i);
	}
	ConvertWpfFromColor(imgRGB,dataR,strideR,nbLigR,nbColR);
}

void Masque(byte* data, int stride, int nbLig, int nbCol, byte* dataBinaire, int strideBinaire, int nbLigBinaire, int nbColBinaire)
{
	CImageNdg imgNdg = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);
	CImageNdg imgBinaire = ConvertImgFromWpfNdg(dataBinaire, strideBinaire, nbLigBinaire, nbColBinaire);

	for (int i = 0; i < imgNdg.lireNbPixels(); i++)
		if (!imgBinaire(i))
			imgNdg(i) = 0;

	ConvertWpfFromNdg(imgNdg, data, stride, nbLig, nbCol);
}


//void Saturation(byte* data, int stride, int nbLig, int nbCol)
//{
//	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);
//	img;
//
//	/*for (int i = 0; i < imgNdg.lireNbPixels(); i++)
//		if (!imgBinaire(i))
//			imgNdg(i) = 0;*/
//
//	ConvertWpfFromColor(img, data, stride, nbLig, nbCol);
//}

void ContrasteRgb(byte* data, int stride, int nbLig, int nbCol, double contrasteValue)
{
	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);
	CImageNdg img0 = img.plan(0);
	CImageNdg img1 = img.plan(1);
	CImageNdg img2 = img.plan(2);

	CImageCouleur imgOut = CImageCouleur(nbLig, nbCol);
	img0 = img0.Contraste(contrasteValue, 1);
	img1 = img1.Contraste(contrasteValue, 1);
	img2 = img2.Contraste(contrasteValue, 1);

	for (int i = 0; i < imgOut.lireNbPixels(); i++)
	{
		imgOut(i)[0] = img0(i);
		imgOut(i)[1] = img1(i);
		imgOut(i)[2] = img2(i);

	} 
	
	ConvertWpfFromColor(imgOut, data, stride, nbLig, nbCol);
}

void ContrasteNdg(byte* data, int stride, int nbLig, int nbCol, double contrasteValue)
{
	CImageNdg img0 = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);

	img0 = img0.Contraste(contrasteValue, 1);

	ConvertWpfFromNdg(img0, data, stride, nbLig, nbCol);
}

void LuminositeRgb(byte* data, int stride, int nbLig, int nbCol, byte brightness)
{
	CImageCouleur img = ConvertImgFromWpf(data, stride, nbLig, nbCol);
	CImageNdg img0 = img.plan(0);
	CImageNdg img1 = img.plan(1);
	CImageNdg img2 = img.plan(2);

	CImageCouleur imgOut = CImageCouleur(nbLig, nbCol);
	img0 = img0.Contraste(1, brightness);
	img1 = img1.Contraste(1, brightness);
	img2 = img2.Contraste(1, brightness);

	for (int i = 0; i < imgOut.lireNbPixels(); i++)
	{
		imgOut(i)[0] = img0(i);
		imgOut(i)[1] = img1(i);
		imgOut(i)[2] = img2(i);

	}

	ConvertWpfFromColor(imgOut, data, stride, nbLig, nbCol);
}

void LuminositeNdg(byte* data, int stride, int nbLig, int nbCol, byte brightness)
{
	CImageNdg img0 = ConvertImgFromWpfNdg(data, stride, nbLig, nbCol);

	img0 = img0.Contraste(1, brightness);

	ConvertWpfFromNdg(img0, data, stride, nbLig, nbCol);
}

int Etiquetage(byte* dataParam, int strideParam, int nbLigParam, int nbColParam, byte* dataReturn, int strideReturn, int nbLigReturn, int nbColReturn, byte* nbRegions) {
	CImageCouleur img = ConvertImgFromWpf(dataParam, strideParam, nbLigParam, nbColParam);
	int seuilbas = 128;
	int seuilhaut = 255;
	CImageNdg imgNdg = img.plan(3).seuillage("automatique", seuilbas, seuilhaut);
	CImageClasse imgOut = CImageClasse(imgNdg, "V8");
	ConvertWpfFromClass8bitsFormat(imgOut, dataReturn, strideReturn, nbLigReturn, nbColReturn);
	return imgOut.lireNbRegions();
}

void AjoutCanalAlpha(byte* dataParam, int strideParam, int nbLigParam, int nbColParam, byte* dataReturn, int strideReturn, int nbLigReturn, int nbColReturn, byte r, byte g, byte b, byte tolerance) {
	CImageCouleur img = ConvertImgFromWpf(dataParam, strideParam, nbLigParam, nbColParam);

	ImageRGBA imgOut = ImageRGBA(nbLigParam, nbColParam, 0, 0, 0, 255);
	for (int i = 0; i < img.lireNbPixels(); i++) {
		byte rImage = img(i)[0];
		byte gImage = img(i)[1];
		byte bImage = img(i)[2];
		
		imgOut(i)[0] = rImage;
		imgOut(i)[1] = gImage;
		imgOut(i)[2] = bImage;
		
		if (rImage >= r - tolerance && rImage <= r + tolerance &&
			gImage >= g - tolerance && gImage <= g + tolerance &&
			bImage >= b - tolerance && bImage <= b + tolerance) 
		{
			imgOut(i)[3] = 0;
		}
		else
		{
			imgOut(i)[3] = 255;
		}
	}


	
	ConvertWpfFromARGB32bitsFormat(imgOut, dataReturn, strideReturn, nbLigReturn, nbColReturn);

	//CImageCouleur aaa = CImageCouleur(nbLigParam, nbColParam, 255, 128, 255);
	//ConvertWpfFromColor(aaa, dataReturn, strideReturn, nbLigReturn, nbColReturn);
}