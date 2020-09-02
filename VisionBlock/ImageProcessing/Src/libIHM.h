#pragma once

#include "ImageClasse.h"
#include "ImageNdg.h"
#include "ImageCouleur.h"
#include "ImageDouble.h"
#include <windows.h>

extern "C" _declspec(dllexport) void Addition(byte* data, int stride, int nbLig, int nbCol, byte* data2, int stride2, int nbLig2, int nbCol2);

extern "C" _declspec(dllexport) void ColorToNdg(byte* data, int stride, int nbLig, int nbCol, int plan);

extern "C" _declspec(dllexport) void Moyennage(byte* data, int stride, int nbLig, int nbCol);

extern "C" _declspec(dllexport) void Seuillage(byte* data, int stride, int nbLig, int nbCol, int valSeuilMin, int valSeuilMax);

extern "C" _declspec(dllexport) void GetPlan(byte* data, int stride, int nbLig, int nbCol, int nbPlan);

extern "C" _declspec(dllexport) void Inversion(byte* data, int stride, int nbLig, int nbCol);

extern "C" _declspec(dllexport) MOMENTS GetImageData(byte* data, int stride, int nbLig, int nbCol);

extern "C" _declspec(dllexport) MOMENTS* GetImageDataRGB(byte* data, int stride, int nbLig, int nbCol);

extern "C" _declspec(dllexport) void OperateurBinaire(byte* data, int stride, int nbLig, int nbCol, byte* data2, int stride2, int nbLig2, int nbCol2, char* operateurBinaire);

extern "C" _declspec(dllexport) void Filtrage(byte* data, int stride, int nbLig, int nbCol, int surfaceMin, int surfaceMax, bool miseAZero);

extern "C" _declspec(dllexport) void Morphologie(byte* data, int stride, int nbLig, int nbCol, int indexMorpho, int v4v8);

extern "C" _declspec(dllexport) void HistogrammeNdg(byte* data, int stride, int nbLig, int nbCol, int **hist);

extern "C" _declspec(dllexport) void HistogrammeRgb(byte* data, int stride, int nbLig, int nbCol, int **hist1, int **hist2, int **hist3);

extern "C" _declspec(dllexport) void Lut(byte* data, int stride, int nbLig, int nbCol, byte* lut);

extern "C" _declspec(dllexport) void MoyennePonderee(byte* data, int stride, int nbLig, int nbCol, byte* data2, int stride2, int nbLig2, int nbCol2, double moyennePondeeree);

extern "C" _declspec(dllexport)  void RgbFromPlans(byte* dataR, int strideR, int nbLigR, int nbColR, byte* dataG, int strideG, int nbLigG, int nbColG, byte* dataB, int strideB, int nbLigB, int nbColB);

extern "C" _declspec(dllexport) void Masque(byte* data, int stride, int nbLig, int nbCol, byte* dataBinaire, int strideBinaire, int nbLigBinaire, int nbColBinaire);

extern "C" _declspec(dllexport) void ContrasteRgb(byte* data, int stride, int nbLig, int nbCol, double contrasteValue);

extern "C" _declspec(dllexport) void ContrasteNdg(byte* data, int stride, int nbLig, int nbCol, double contrasteValue);

extern "C" _declspec(dllexport)void LuminositeRgb(byte* data, int stride, int nbLig, int nbCol, byte brightness);

extern "C" _declspec(dllexport)void LuminositeNdg(byte* data, int stride, int nbLig, int nbCol, byte brightness);

extern "C" _declspec(dllexport)int Etiquetage(byte* dataParam, int strideParam, int nbLigParam, int nbColParam, byte* dataReturn, int strideReturn, int nbLigReturn, int nbColReturn, byte* nbRegions);

extern "C" _declspec(dllexport) void AjoutCanalAlpha(byte* dataParam, int strideParam, int nbLigParam, int nbColParam, byte* dataReturn, int strideReturn, int nbLigReturn, int nbColReturn, byte r, byte g, byte b, byte precision);


