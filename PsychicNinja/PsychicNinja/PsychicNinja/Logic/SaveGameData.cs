using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class SaveGameData
{
    private const long defaultHighScore = 5000000000;
    public long[] RecordTimeInTicks = { defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, 
                                          defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, 
                                          defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, 
                                          defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore,
                                          defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore, defaultHighScore};
 

    public int[] levelReached = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    public int worldReached = 0;
}
