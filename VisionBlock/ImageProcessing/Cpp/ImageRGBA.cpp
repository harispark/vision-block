#include "ImageRGBA.h"

#pragma once

#ifndef _IMAGE_RGBA_
#define _IMAGE_RGBA_

ImageRGBA::ImageRGBA(int hauteur, int largeur, int valAlpha, int valR, int valV, int valB)
{
	this->m_iHauteur = hauteur;
	this->m_iLargeur = largeur;
	this->m_sNom = "inconnu";

	this->m_pucData = new unsigned char[hauteur * largeur * 4];
	this->m_ppucPixel = new unsigned char* [hauteur * largeur];
	for (int i = 0; i < hauteur * largeur; i++)
		this->m_ppucPixel[i] = &this->m_pucData[4 * i];

	//assignation des 4 valeurs de chaque pixel
	if (valAlpha != -1)
		for (int i = 0; i < this->lireNbPixels(); i++)
			this->m_ppucPixel[i][0] = valAlpha;
	if (valR != -1)
		for (int i = 0; i < this->lireNbPixels(); i++)
			this->m_ppucPixel[i][1] = valR;
	if (valV != -1)
		for (int i = 0; i < this->lireNbPixels(); i++)
			this->m_ppucPixel[i][2] = valV;
	if (valB != -1)
		for (int i = 0; i < this->lireNbPixels(); i++)
			this->m_ppucPixel[i][3] = valB;
}

#endif 