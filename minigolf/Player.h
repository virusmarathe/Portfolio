class Player {
	public:
		Player();
		Player(char *);
		char* Player::getName();
		int   Player::getCurrentScore();
		int   Player::getTotalScore();
		void  Player::setName(char*);
		void  Player::setCurrentScore(int);
		void  Player::setTotalScore(int);
		void  Player::incrementCurrentScore();
		void  Player::switchTurn();
		bool  Player::getTurn();
	private:
		char * name;
		int current_score;
		int total_score;
		bool turn;
};