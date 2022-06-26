

using System.Collections.Generic;

public class Player
{
    public int id { get; set; }
    public string playerName { get; }
    public int age { get; }
    public List<Session> sessions { get; set; }

    public Player (int id, string playerName, int age, List<Session> sessions)
    {
        this.id = id;
        this.playerName = playerName;
        this.age = age;
        this.sessions = sessions;
    }
    public Player( string playerName, int age)
    {
        this.playerName = playerName;
        this.age = age;
        sessions = new List<Session>();
    }

    public Player(int id, string playerName, int age)
    {
        this.id = id;
        this.playerName = playerName;
        this.age = age;
        sessions = new List<Session>();
    }
}
