#include "stdafx.h"

LevelScores::LevelScores(int curr, int num){
	currLevel = curr;
	numLevels = num;
}

void LevelScores::setScore(int level, int score, char * name){
	if( score <  highScores[level]){
		 highScores[level] = score;
		 names[level] = name;
	}
}

void LevelScores::readScores(){
  int lineNum = 0;
  string line;
  string buf;
  vector<string> words;
  ifstream myfile ("highscores.txt");
  if (myfile.is_open())
  {
    while ( myfile.good() )
    {
      getline (myfile,line);
	  stringstream ss(line); // Insert the string into a stream
		while (ss >> buf)
		   words.push_back(buf);
		if (!words.empty()) {
			 levels.push_back(atoi(words[0].c_str()));
			 highScores.push_back(atoi(words[1].c_str()));
			 names.push_back(words[2]);
		}
		words.clear();
    }
    myfile.close();
  }
}

void LevelScores::displayScores(){
	cout<<"<<<<<<<<<<<<<<<<<<<HIGH SCORES>>>>>>>>>>>>>>>>>>>>"<<endl;
  for(int i = 0; i<  numLevels; i++){
	  cout << "Level " <<  levels[i]+1 << ": ";
	  cout <<  highScores[i] << " " << LevelScores::names[i] << endl;
  }
}

void LevelScores::writeScores(){
	ofstream outFile("highscores.txt");
	for(int i = 0; i < numLevels; i++){
		outFile << levels[i] << " " << highScores[i] << " " << names[i] << "\n";
	}
}