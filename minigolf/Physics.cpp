#include "stdafx.h"

int pointInPolygon(vector<double> point, Tile * t)
{
   vector<double> p = Util::calcNormal(t->pointsList[t->getSize()-1],point, t->pointsList[0]);
   if (p[1]>0) {
	   return t->getSize()-1;
   }
   for (int i = 0; i < t->getSize() - 1; i++)
   {
       vector<double> q = Util::calcNormal(t->pointsList[i],point, t->pointsList[i+1]);
       if (Util::dot(p, q) < 0) {
          return i;
	   }
   }
   return -1;
}
//returns true if ball in hole, false otherwise

bool Physics::checkCollisions(Ball * ball, vector<Tile*> tiles, Cup * hole) {
	
  // checks collisions
  int edge = pointInPolygon(ball->getPos(), tiles[ball->getTileIndex()-1]);
  if (edge != -1 && tiles[ball->getTileIndex()-1]->edgeConnections[edge] == 0) {
	  vector<double> edgeNormal;
	  vector<double> edgeHeight;
      edgeHeight.push_back(tiles[ball->getTileIndex()-1]->pointsList[edge][0]);
	  edgeHeight.push_back(tiles[ball->getTileIndex()-1]->pointsList[edge][1]+0.1);
	  edgeHeight.push_back(tiles[ball->getTileIndex()-1]->pointsList[edge][2]);
	  if (edge != tiles[ball->getTileIndex()-1]->getSize()-1) {
		  edgeNormal = Util::calcNormal(edgeHeight,tiles[ball->getTileIndex()-1]->pointsList[edge+1],tiles[ball->getTileIndex()-1]->pointsList[edge]);
	  }
	  else {
		  edgeNormal = Util::calcNormal(edgeHeight,tiles[ball->getTileIndex()-1]->pointsList[0], tiles[ball->getTileIndex()-1]->pointsList[edge]);
	  }
	  vector<double> ref = Util::getReflectionVector(ball->getDirection(), edgeNormal);
	  ball->setDirection(ref);
  }
  else if (edge != -1 && tiles[ball->getTileIndex()-1]->edgeConnections[edge] != 0) {
	  ball->setTileIndex((int)tiles[ball->getTileIndex()-1]->edgeConnections[edge]);
	  vector<double> up;
	  up.push_back(0);
  	  up.push_back(1);
	  up.push_back(0);
	  vector<double> zero;
	  zero.push_back(0);
  	  zero.push_back(0);
	  zero.push_back(0);

	  vector<double> xchange = Util::calcNormal(ball->getDirection(), zero,up);
	  vector<double> norm = Util::calcNormal(tiles[ball->getTileIndex()-1]->pointsList[0],tiles[ball->getTileIndex()-1]->pointsList[1],tiles[ball->getTileIndex()-1]->pointsList[2]);
	  vector<double> dir = Util::calcNormal(norm, zero, xchange);

	  ball->setDirection(dir);

	  xchange = Util::calcNormal(up, zero, norm);
	  vector<double> roll = Util::calcNormal(xchange, zero, norm);
	  roll = Util::vectorScalarMult(roll, 0.0098);
	  ball->setAcceleration(roll);
  }
  if (ball->getTileIndex() == hole->getTile()) {
	  vector<double> ballPos = ball->getPos();
	  vector<double> holePos = hole->getPos();
	  if (ballPos[0] > holePos[0] && ballPos[0] < holePos[0] + 0.1 && ballPos[2] > holePos[2] && ballPos[2] < holePos[2] + 0.1)
		  return true;
  }
  return false;
}