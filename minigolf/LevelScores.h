#include <vector>
using namespace std;

class LevelScores{
	public:
		int currLevel;
		int numLevels;
		vector<string> names;
		vector<int> highScores;
		vector<int> levels;

		LevelScores(int curr, int num);
		void LevelScores::setScore(int level, int score, char * name);
		void LevelScores::readScores();
		void LevelScores::writeScores();
		void LevelScores::displayScores();
		//static void readScoers();
};