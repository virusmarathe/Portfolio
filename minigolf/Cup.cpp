#include "stdafx.h"

Cup::Cup(int i, double x, double y, double z) {
	TILE_ID = i;
	pos.push_back(x);
	pos.push_back(y);
	pos.push_back(z);
}

Cup::Cup() {
	TILE_ID = -1;
	pos.push_back(0);
	pos.push_back(0);
	pos.push_back(0);
}

vector<double> Cup::getPos() {
	return this->pos;
}

void Cup::setID(int i) {
	TILE_ID = i;
}

int Cup::getTile() {
	return TILE_ID;
}

void Cup::setPos(double x, double y, double z) {
	pos[0] = x;
	pos[1] = y;
	pos[2] = z;
}

void Cup::setPosX(double x) {
	pos[0] = x;
}

void Cup::setPosY(double y) {
	pos[1] = y;
}

void Cup::setPosZ(double z) {
	pos[2] = z;
}