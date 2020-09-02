#pragma once
#include <string>

using namespace std;

class ImageRGBA
{
	///////////////////////////////////////
private:
	///////////////////////////////////////

	int              m_iHauteur;
	int              m_iLargeur;
	std::string      m_sNom;
	unsigned char* m_pucData;
	unsigned char** m_ppucPixel;

	///////////////////////////////////////
public:
	///////////////////////////////////////

		// constructeurs
	_declspec(dllexport) ImageRGBA(int hauteur, int largeur, int valAlpha, int valR, int valV, int valB); // si -1 alors non pixels non initialisés

	_declspec(dllexport) int lireNbPixels() const {
		return m_iHauteur * m_iLargeur;
	}

	// pouvoir accéder à un pixel par image(i,j)[plan]
	_declspec(dllexport) unsigned char*& operator() (int i, int j) const {
		return m_ppucPixel[i * m_iLargeur + j];
	}

	// pouvoir accéder à un pixel par image(i)[plan]
	_declspec(dllexport) unsigned char*& operator() (int i) const {
		return m_ppucPixel[i];
	}
};

