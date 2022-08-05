using System.Collections.Generic;
using UnityEngine;

public class Prediction
{
    private List<FlyingGame> games;
    private const float standard_dv = 10;
    private const float mean = 50;

    public Prediction(List<FlyingGame> games)
    {
        this.games = games; 
    }

    public int GetTargets()
    {
        if (games.Count == 0) return 10;

        float sum = 0;

        foreach(FlyingGame game in games)
        {
            sum +=game.total_targets * (1 + (game.GAS_score - mean)/(3*standard_dv));
        }

        return Mathf.RoundToInt(sum/games.Count);
    }

    public float GetTargetSize()
    {
        if (games.Count == 0) return 5;
        float sum = 0;

        foreach (FlyingGame game in games)
        {
            //we want to make it inversely proportional 
            sum += game.targets_size * (1 - (game.GAS_score - mean) / (3 * standard_dv));
        }

        return sum / games.Count;
    }

    public float GetDistBetweenTargets()
    {
        if (games.Count == 0) return 10;
        float sum = 0;

        foreach (FlyingGame game in games)
        {
            //we want to make it inversely proportional 
            sum += game.distance_between_targets * (1 - (game.GAS_score - mean) / (3 * standard_dv));
        }

        return sum / games.Count;
    }
}
