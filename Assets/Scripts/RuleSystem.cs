using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RuleSystem
{
    private List<FlyingGame> games;

    public RuleSystem(List<FlyingGame> games)
    {
        this.games = games; 
        
    }

    public int GetTargets()
    {
        return 10;
    }

    public float GetTargetSize()
    {
        return 5;
    }

    public float GetDistBetweenTargets()
    {
        return 10;
    }
}
