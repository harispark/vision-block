#include <iostream>
#include <fstream>
#include <sstream>
#include <string>
#include <windows.h>
#include <cmath>
#include <vector>
#include <ctime>
#include <stack>

#include "WpfTools.h"

int nbBytesPerPixel = 3;
CImageCouleur ConvertImgFromWpf(byte* data, int stride, int nbLig, int nbCol)
{
	CImageCouleur imgPt = CImageCouleur(nbLig, nbCol);
	byte* pixPtr = (byte*)data;

	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			imgPt(y, x)[0] = pixPtr[nbBytesPerPixel * x + 2];
			imgPt(y, x)[1] = pixPtr[nbBytesPerPixel * x + 1];
			imgPt(y, x)[2] = pixPtr[nbBytesPerPixel * x];
		}
		pixPtr += stride; // largeur une seule ligne gestion multiple 32 bits
	}
	return imgPt;
}

CImageNdg ConvertImgFromWpfNdg(byte* data, int stride, int nbLig, int nbCol)
{
	CImageNdg imgPt = CImageNdg(nbLig, nbCol);
	byte* pixPtr = (byte*)data;

	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			imgPt(y, x) = pixPtr[nbBytesPerPixel * x];
		}
		pixPtr += stride; // largeur une seule ligne gestion multiple 32 bits
	}
	return imgPt;
}

void ConvertWpfFromNdg(CImageNdg img, byte* data, int stride, int nbLig, int nbCol)
{
	byte* pixPtr = (byte*)data;
	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			pixPtr[nbBytesPerPixel * x + 2] = img(y, x);
			pixPtr[nbBytesPerPixel * x + 1] = img(y, x);
			pixPtr[nbBytesPerPixel * x] = img(y, x);
		}
		pixPtr += stride;
	}
}

void ConvertWpfFromColor(CImageCouleur img, byte* data, int stride, int nbLig, int nbCol)
{
	byte* pixPtr = (byte*)data;
	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			pixPtr[nbBytesPerPixel * x + 2] = img(y, x)[0];
			pixPtr[nbBytesPerPixel * x + 1] = img(y, x)[1];
			pixPtr[nbBytesPerPixel * x] = img(y, x)[2];
		}
		pixPtr += stride;
	}
}


void ConvertWpfFromClass(CImageClasse img, byte* data, int stride, int nbLig, int nbCol)
{
	byte* pixPtr = (byte*)data;
	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			pixPtr[nbBytesPerPixel * x + 2] = img(y, x);
			pixPtr[nbBytesPerPixel * x + 1] = img(y, x);
			pixPtr[nbBytesPerPixel * x] = img(y, x);
		}
		pixPtr += stride;
	}
}

void ConvertWpfFromClass8bitsFormat(CImageClasse img, byte* data, int stride, int nbLig, int nbCol)
{
	int nombreOctetsParPixels = 1;
	//ATTENTION, je n'utilise pas la variable globale car sinon tout les traitements auront 8bit par pixel > niveau de gris
	byte* pixPtr = (byte*)data;
	for (int y = 0; y < nbLig; y++)
	{
		for (int x = 0; x < nbCol; x++)
		{
			pixPtr[nombreOctetsParPixels * x] = img(y, x);
		}
		pixPtr += stride;
	}
}

void ConvertWpfFromARGB32bitsFormat(ImageRGBA imgOut, byte* dataReturn, int strideReturn, int nbLigReturn, int nbColReturn)
{
	int nombreOctetsParPixels = 4;
	//ATTENTION, je n'utilise pas la variable globale car sinon tout les traitements auront 8bit par pixel > niveau de gris
	byte* pixPtr = (byte*)dataReturn;
	for (int y = 0; y < nbLigReturn; y++)
	{
		for (int x = 0; x < nbColReturn; x++)
		{
			pixPtr[nombreOctetsParPixels * x + 2] = imgOut(y, x)[0];	//R
			pixPtr[nombreOctetsParPixels * x + 1] = imgOut(y, x)[1];	//G
			pixPtr[nombreOctetsParPixels * x] = imgOut(y, x)[2];		//B
			pixPtr[nombreOctetsParPixels * x + 3] = imgOut(y, x)[3];	//A
		}
		pixPtr += strideReturn;
		cout << "stride : " << strideReturn << "alpha : " <<std::endl;
	}
}

