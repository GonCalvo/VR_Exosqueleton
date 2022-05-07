using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager
{
    public int score;
    public int total_targets;
    public int targets_passed;
    public int cur_streak;
    public int max_streak;

    private GameFinished end;

    public ScoreManager( int total_targets, GameFinished end )
    {
        score = 0;
        cur_streak = 0;
        max_streak = 0;
        targets_passed = 0;
        this.total_targets = total_targets;
        this.end = end;
    }

    public void Score( bool obtained )
    {
        if ( obtained )
        {
            score++;
            cur_streak++;

            if (cur_streak > max_streak) max_streak = cur_streak;
        }
        else
        {
            cur_streak = 0;
        }
        targets_passed++;
        if ( targets_passed == total_targets)
        {
            targets_passed++; // prevents the previous condition from happening.
            end();
        }
    }

    public delegate void GameFinished(); //create a prototype of what the ending method has to be, it will be filled by the GameManagers
}
