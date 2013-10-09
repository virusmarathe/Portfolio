#include "stdafx.h"

Tee::Tee(int i, double x, double y, double z) {
	TILE_ID = i;
	pos.push_back(x);
	pos.push_back(y);
	pos.push_back(z);
}

Tee::Tee() {
	TILE_ID = -1;
	pos.push_back(0);
	pos.push_back(0);
	pos.push_back(0);
}

vector<double> Tee::getPos() {
	return this->pos;
}

int Tee::getTile() {
	return TILE_ID;
}

void Tee::setPos(double x, double y, double z) {
	pos[0] = x;
	pos[1] = y;
	pos[2] = z;
}

void Tee::setID(int i) {
	TILE_ID = i;
}