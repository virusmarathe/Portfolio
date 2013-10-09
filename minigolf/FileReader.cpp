#include "stdafx.h"


vector<Tile *> FileReader::parseLevelFile(string s, vector<Tile *> tiles, Ball * ball, Ball *ball2, Tee * tee, Cup * hole) {
	
	string line;
	string buf;
	vector<string> words;
	int counter = 0;
	ifstream myfile (s);
	if (myfile.is_open())
	{
		while ( myfile.good() )
		{
			getline (myfile,line);
			stringstream ss(line); // Insert the string into a stream
		    while (ss >> buf)
				words.push_back(buf);
			if (!words.empty()) {
				if (words[0].compare("tile") == 0) {
					double id = Util::string_to_double(words[1]);
					double numPoints = Util::string_to_double(words[2]);
					Tile *t = new Tile((int)id,(int)numPoints);
					for (int i =3; i< words.size() - (int)(numPoints); i+=3) {
						double xpos = Util::string_to_double(words[i]);
						double ypos = Util::string_to_double(words[i+1]);
						double zpos = Util::string_to_double(words[i+2]);
						t->addPoint(xpos,ypos,zpos);
					}
					for (int i = words.size() - (int)(numPoints);i < words.size(); i++) {
						double edge = Util::string_to_double(words[i]);
						t->addConnection((int)edge);
					}

					tiles.push_back(t);
				}
				else if (words[0].compare("tee") == 0) {
					double id = Util::string_to_double(words[1]);
					double xpos = Util::string_to_double(words[2]);
					double ypos = Util::string_to_double(words[3]);
					double zpos = Util::string_to_double(words[4]);
					tee->setPos(xpos,ypos,zpos);
					tee->setID(id);
					ball->setPos(xpos,ypos+ball->getRadius(),zpos);
					ball->setDirection(1,0,0);
					ball->setTileIndex(tee->getTile());
					ball2->setPos(xpos,ypos+ball2->getRadius(),zpos);
					ball2->setDirection(1,0,0);
					ball2->setTileIndex(tee->getTile());
				}
				else if (words[0].compare("cup") == 0) {
					double id = Util::string_to_double(words[1]);
					double xpos = Util::string_to_double(words[2]);
					double ypos = Util::string_to_double(words[3]);
					double zpos = Util::string_to_double(words[4]);
					hole->setPos(xpos,ypos,zpos);
					hole->setID(id);
				}
				words.clear();
			}
		}
		myfile.close();
	}
	return tiles;
}
