

using System;
using System.Collections.Generic;

public class Session
{
    public int id;
    public Player player { get; }
    public DateTime date { get; }
    public Supervisor supervisor { get; }
    public float range_left;
    public float range_right;
    public float range_top;
    public float range_bottom;
    public float range_fwd;
    public float overall_GAS;
    public List<FlyingGame> flying_games;

    public Session(int id, Player player, DateTime date, Supervisor supervisor, float range_left, float range_right, float range_top, 
        float range_bottom, float overall_GAS, List<FlyingGame> flying_games)
    {
        this.id = id;
        this.player = player;
        this.date = date;
        this.supervisor = supervisor;
        this.range_left = range_left;
        this.range_right = range_right;
        this.range_top = range_top;
        this.range_bottom = range_bottom;
        this.overall_GAS = overall_GAS;
        this.flying_games = flying_games;
    }

    public Session(int id, Player player, DateTime date, Supervisor supervisor, float range_left, float range_right, float range_top,
        float range_bottom)
    {
        this.id = id;
        this.player = player;
        this.date = date;
        this.supervisor = supervisor;
        this.range_left = range_left;
        this.range_right = range_right;
        this.range_top = range_top;
        this.range_bottom = range_bottom;
        this.flying_games = new List<FlyingGame>();
    }

    public Session(int id, Player player, DateTime date, Supervisor supervisor)
    {
        this.id = id;
        this.player = player;
        this.date = date;
        this.supervisor = supervisor;
        this.flying_games = new List<FlyingGame>();
    }

    public Session(Player player, DateTime date, Supervisor supervisor)
    {
        this.player = player;
        this.date = date;
        this.supervisor = supervisor;
        this.flying_games = new List<FlyingGame>();
        range_left = -1;
        range_right = -1;
        range_top = -1;
        range_bottom = -1;
        range_fwd = -1;
}

    override public string ToString()
    {
        return "Session at date " + date.ToString() + " done by " + player.playerName + " supervised by " + supervisor.supervisorName;
    }
}
