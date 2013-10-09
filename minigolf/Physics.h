#include <vector>
using namespace std;

class Physics {
	public:
		static bool checkCollisions(Ball * ball, vector<Tile*> tiles, Cup * hole);
};