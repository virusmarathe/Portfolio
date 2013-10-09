// Tile Object

#include "stdafx.h"

Tile::Tile(int i, int n) {
	ID = i;
	numPoints = n;
}

void Tile::addPoint(double xpos, double ypos, double zpos) {
	vector<double> point;
	point.push_back(xpos);
	point.push_back(ypos);
	point.push_back(zpos);
	pointsList.push_back(point);
}

void Tile::addConnection(int num) {
	edgeConnections.push_back(num);
}

int Tile::getSize() {
	return numPoints;
}