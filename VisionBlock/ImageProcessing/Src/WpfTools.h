#pragma once
#include "ImageClasse.h"
#include "ImageNdg.h"
#include "ImageCouleur.h"
#include "ImageDouble.h"
#include "ImageRGBA.h"
#include <windows.h>

// fonction de conversion interne, pas besoin d'être exportées
CImageCouleur ConvertImgFromWpf(byte* data, int stride, int nbLig, int nbCol);
CImageNdg ConvertImgFromWpfNdg(byte* data, int stride, int nbLig, int nbCol);
void ConvertWpfFromNdg(CImageNdg img, byte* data, int stride, int nbLig, int nbCol);
void ConvertWpfFromColor(CImageCouleur img, byte* data, int stride, int nbLig, int nbCol);
void ConvertWpfFromClass(CImageClasse img, byte* data, int stride, int nbLig, int nbCol);
void ConvertWpfFromClass8bitsFormat(CImageClasse img, byte* data, int stride, int nbLig, int nbCol);
void ConvertWpfFromARGB32bitsFormat(ImageRGBA imgOut, byte* dataReturn, int strideReturn, int nbLigReturn, int nbColReturn);