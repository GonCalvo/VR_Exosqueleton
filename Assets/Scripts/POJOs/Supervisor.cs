

public class Supervisor 
{
    public int id;
    public string supervisorName;
    public string password;

    public Supervisor(int id, string supervisorName, string password)
    {
        this.id = id;
        this.supervisorName = supervisorName;
        this.password = password;
    }

    public Supervisor(string supervisorName, string password)
    {
        this.supervisorName = supervisorName;
        this.password = password;
    }

    public Supervisor(int id, string supervisorName)
    {
        this.id = id;
        this.supervisorName = supervisorName;
    }
}
