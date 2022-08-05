using Mono.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class SQLManager
{
    private IDbConnection dbConnection = null;
    private Encryptor encryptor = null;

    private string DBpath;

    public SQLManager(string db)
    {
        connect(db);
    } 

    public void connect(string db)
    {
        //System.Console.WriteLine( "Connecting to " + db);
        //string strConexion = string.Format("Data Source = {0}", db);
#if UNITY_EDITOR
        string strConexion = "";

        //Creo y abro conexion. Le pido que me ponga a la altura de la carpeta de Assets en la ruta de la aplicación en Android
        if (Application.platform == RuntimePlatform.WindowsEditor)
        {
            DBpath = Application.dataPath + "/StreamingAssets/" + db;
            strConexion = "URI=file:" + DBpath;
        }
        Debug.Log("STRConexion: "  + strConexion);
        dbConnection = new SqliteConnection(strConexion);
        dbConnection.Open();
#endif
        encryptor = new Encryptor();

    }

    public void disconnect()
    {
        if (dbConnection != null)
        {
            dbConnection.Dispose();
            dbConnection.Close();
        }
    }

    public void createTablePlayers()
    {
        try
        {
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string query = "CREATE TABLE players (" +
                "player_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                "name TEXT NOT NULL UNIQUE," +
                "age INTEGER)";
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException e)
        {
            System.Console.WriteLine("[WARNING]: Table \"players\" already exist, skipping its creation");
        }
    }

    public void createTableSupervisors()
    {
        try
        {
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string query = "CREATE TABLE supervisors (" +
                "supervisor_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                "name TEXT NOT NULL UNIQUE," +
                "passwd TEXT NOT NULL)";
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException e)
        {
            System.Console.WriteLine("[WARNING]: Table \"supervisors\" already exist, skipping its creation");
        }
    }

    public void createTableFlyingGames()
    {
        try
        {
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string query = "CREATE TABLE flying_games (" +
                "flying_game_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                "session_id REFERENCES sessions(session_id)," +
                "distance_between_targets REAL," +
                "total_targets INTEGER," +
                "targets_size REAL," +
                "targets_hit INTEGER," +
                "expected_targets_hit INTEGER," +
                "maximum_streak INTEGER," +
                "expected_max_streak INTEGER," +
                "total_targets_weight INTEGER," +
                "max_streak_weight INTEGER," +
                "targets_hit_puntuation INTEGER," +
                "max_streak_puntuation INTEGER,"+
                "GAS_score REAL)";
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();

        }
        catch (SqliteException e)
        {
            System.Console.WriteLine("[WARNING]: Table \"flying_games\" already exist, skipping its creation");
        }
    }

    public void createTableSessions()
    {

        try
        {
            IDbCommand dbCommand = dbConnection.CreateCommand();
            string query = "CREATE TABLE sessions (" +
                "session_id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT," +
                "date DATETIME NOT NULL," +
                "player REFERENCES players (player_id), " +
                "supervisor REFERENCES supervisors(supervisor_id)," +
                "range_left REAL," +
                "range_right REAL," +
                "range_top REAL," +
                "range_bottom REAL," +
                "range_fwd REAL," +
                "overall_GAS REAL)";
            dbCommand.CommandText = query;
            dbCommand.ExecuteNonQuery();
        }
        catch (SqliteException e)
        {
            System.Console.WriteLine("[WARNING]: Table \"sessions\" already exist, skipping its creation");
        }
    }

    public void insertSession(Session s)
    {
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = @"INSERT INTO sessions (" +
            "date, player, supervisor, range_left, range_right, range_top, range_bottom, range_fwd)" +
            " VALUES ($date, $player, $supervisor, $r_left, $r_right, $r_top, $r_bottom, $r_fwd);";

        System.Console.WriteLine(string.Format("[DEBUG]: Inserting session with " +
            "date = {0}, player = {1}, supervisor = {2}, range_left = {3}, range_right = {4}, range_top = {5}, range_bottom = {6}",
            s.date, s.player.id, s.supervisor.id, s.range_left, s.range_right, s.range_top, s.range_bottom));

        var parameterDate = command.CreateParameter();
        parameterDate.ParameterName = "$date";
        parameterDate.Value = s.date;
        command.Parameters.Add(parameterDate);

        var parameterPlayer = command.CreateParameter();
        parameterPlayer.ParameterName = "$player";
        parameterPlayer.Value = s.player.id;
        command.Parameters.Add(parameterPlayer);

        var parameterSupervisor = command.CreateParameter();
        parameterSupervisor.ParameterName = "$supervisor";
        parameterSupervisor.Value = s.supervisor.id;
        command.Parameters.Add(parameterSupervisor);

        var parameterRLeft = command.CreateParameter();
        parameterRLeft.ParameterName = "$r_left";
        parameterRLeft.Value = s.range_left;
        command.Parameters.Add(parameterRLeft);

        var parameterRRight = command.CreateParameter();
        parameterRRight.ParameterName = "$r_right";
        parameterRRight.Value = s.range_right;
        command.Parameters.Add(parameterRRight);

        var parameterRTop = command.CreateParameter();
        parameterRTop.ParameterName = "$r_top";
        parameterRTop.Value = s.range_top;
        command.Parameters.Add(parameterRTop);

        var parameterRBottom = command.CreateParameter();
        parameterRBottom.ParameterName = "$r_bottom";
        parameterRBottom.Value = s.range_bottom;
        command.Parameters.Add(parameterRBottom);

        var parameterRFwd = command.CreateParameter();
        parameterRFwd.ParameterName = "r_fwd";
        parameterRFwd.Value = s.range_fwd;
        command.Parameters.Add(parameterRFwd);

        command.ExecuteNonQuery();

        IDbCommand command2 = dbConnection.CreateCommand();
        command.CommandText = "SELECT session_id FROM sessions WHERE date = $session_date;";
        var paramSessionDate = command.CreateParameter();
        paramSessionDate.ParameterName = "$session_date";
        paramSessionDate.Value = s.date;
        command.Parameters.Add(paramSessionDate);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                s.id = reader.GetInt32(0); 
            }
        }

    }

    public void insertPlayer(Player p)
    {
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = @"INSERT INTO players(name, age) VALUES ($name, $age);";

        var parameterName = command.CreateParameter();
        parameterName.ParameterName = "$name";
        parameterName.Value = encryptor.encode(p.playerName);
        command.Parameters.Add(parameterName);

        var parameterAge = command.CreateParameter();
        parameterAge.ParameterName = "$age";
        parameterAge.Value = p.age;
        command.Parameters.Add(parameterAge);

        command.ExecuteNonQuery();

    }

    public void insertFlyingGame(FlyingGame f)
    {
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = @"INSERT INTO flying_games(" +
            "session_id, distance_between_targets, total_targets, targets_size, targets_hit, expected_targets_hit, maximum_streak, expected_max_streak, total_targets_importance, max_streak_importance, targets_hit_puntuation, max_streak_puntuation, GAS_Score)" +
            " VALUES ($session_id, $distance_between_targets, $total_targets, $targets_size, $targets_hit, $expected_hits, $max_streak, $expected_max_streak, $total_targets_importance, $max_streak_importance, $targets_hit_puntuation, $max_streak_puntuation, $GAS);";

        System.Console.WriteLine(string.Format("[DEBUG]: Inserting flying_game with session_id = {0}, distance_between_targets = {1}, total_targets = {2}, targets_hit = {3}, expected_targets_hit = {4}, " +
            "maximum_streak = {5}, expected_maximum_streak = {6}, GAS_score = {7}",
            f.session, f.distance_between_targets, f.distance_between_targets, f.total_targets, f.targets_hit, f.expected_targets_hit, f.maximum_streak, f.expected_max_streak, f.GAS_score));

        var parameterSessionId = command.CreateParameter();
        parameterSessionId.ParameterName = "$session_id";
        parameterSessionId.Value = f.session;
        command.Parameters.Add(parameterSessionId);

        var parameterDistTargets = command.CreateParameter();
        parameterDistTargets.ParameterName = "$distance_between_targets";
        parameterDistTargets.Value = f.distance_between_targets;
        command.Parameters.Add(parameterDistTargets);

        var parameterTotalTargets = command.CreateParameter();
        parameterTotalTargets.ParameterName = "$total_targets";
        parameterTotalTargets.Value = f.total_targets;
        command.Parameters.Add(parameterTotalTargets);

        var parameterTargetsSize = command.CreateParameter();
        parameterTargetsSize.ParameterName = "$targets_size";
        parameterTargetsSize.Value = f.targets_size;
        command.Parameters.Add(parameterTargetsSize);

        var parameterTargetsHit = command.CreateParameter();
        parameterTargetsHit.ParameterName = "$targets_hit";
        parameterTargetsHit.Value = f.targets_hit;
        command.Parameters.Add(parameterTargetsHit);

        var parameterExpectedTargetsHit = command.CreateParameter();
        parameterExpectedTargetsHit.ParameterName = "$expected_hits";
        parameterExpectedTargetsHit.Value = f.targets_hit;
        command.Parameters.Add(parameterExpectedTargetsHit);

        var parameterMaxStreak = command.CreateParameter();
        parameterMaxStreak.ParameterName = "$max_streak";
        parameterMaxStreak.Value = f.maximum_streak;
        command.Parameters.Add(parameterMaxStreak);

        var parameterExpectedMaxStreak = command.CreateParameter();
        parameterExpectedMaxStreak.ParameterName = "$expected_max_streak";
        parameterExpectedMaxStreak.Value = f.expected_max_streak;
        command.Parameters.Add(parameterExpectedMaxStreak);

        var parameterMaxStreakImportance = command.CreateParameter();
        parameterMaxStreakImportance.ParameterName = "$max_streak_importance";
        parameterMaxStreakImportance.Value = f.max_streak_weight;
        command.Parameters.Add(parameterMaxStreakImportance);

        var parameterTargetsHitImportance = command.CreateParameter();
        parameterTargetsHitImportance.ParameterName = "total_targets_importance";
        parameterTargetsHitImportance.Value = f.targets_hit_weight;
        command.Parameters.Add(parameterTargetsHitImportance);

        var paramTargetsHitPoints = command.CreateParameter();
        paramTargetsHitPoints.ParameterName = "$targets_hit_puntuation";
        paramTargetsHitPoints.Value = f.calification_targets_hit;
        command.Parameters.Add(paramTargetsHitPoints);

        var parameterMaxStreakPoint = command.CreateParameter();
        parameterMaxStreakPoint.ParameterName = "$max_streak_puntuation";
        parameterMaxStreakPoint.Value = f.calification_max_streak;
        command.Parameters.Add(parameterMaxStreakPoint);

        var parameterGAS = command.CreateParameter();
        parameterGAS.ParameterName = "$GAS";
        parameterGAS.Value = f.GAS_score;
        command.Parameters.Add(parameterGAS);

        System.Console.WriteLine("[DEBUG]: SQLite Command: " + command.CommandText);

        command.ExecuteNonQuery();
    }

    public void insertSupervisor(Supervisor s)
    {
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = @"INSERT INTO supervisors(name, passwd) VALUES ($name, $passwd);";

        System.Console.WriteLine(string.Format("[DEBUG]: Inserting Supervisor with name \"{0}\" into the db and password \"{1}\"", s.supervisorName, s.password));

        //TODO: Perhaps the hashing should be done here, right before inserting?
        var parameterName = command.CreateParameter();
        parameterName.ParameterName = "$name";
        parameterName.Value = s.supervisorName;
        command.Parameters.Add(parameterName);

        //TODO: Perhaps the hashing should be done here, right before inserting?
        var parameterPW = command.CreateParameter();
        parameterPW.ParameterName = "$passwd";
        parameterPW.Value = encryptor.hash(s.password);
        command.Parameters.Add(parameterPW);

        System.Console.WriteLine("[DEBUG]: SQLite Command: " + command.CommandText);

        command.ExecuteNonQuery();
    }

    public Session getSessionByPlayer(Player p)
    {
        return getSessionByPlayer(p.id);
    }

    public Session getSessionByPlayer(int id)
    {
        Session session = null;
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM sessions WHERE player = $player_id;";

        var parameterId = command.CreateParameter();
        parameterId.ParameterName = "$player_id";
        parameterId.Value = id;
        command.Parameters.Add(parameterId);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                int session_id = reader.GetInt32(0);
                DateTime date = reader.GetDateTime(1);
                int player_id = id;
                Player player = getPlayerById(player_id);
                Supervisor supervisor = getSupervisorById(reader.GetInt32(3));
                float range_left = reader.GetFloat(4);
                float range_right = reader.GetFloat(5);
                float range_top = reader.GetFloat(6);
                float range_bottom = reader.GetFloat(7);
                float overall_gas = reader.GetFloat(8);
                List<FlyingGame> flyingGames = getFlyingGamesBySession(session_id);

                session = new Session(session_id, player, date, supervisor, range_left, range_right, range_top, range_bottom, overall_gas, flyingGames);

            }
        }

        return session;
    }

    public List<FlyingGame> getFlyingGamesBySession(Session s)
    {
        return getFlyingGamesBySession(s.id);
    }

    public List<FlyingGame> getFlyingGamesBySession(int id)
    {
        List<FlyingGame> flyingGames = new List<FlyingGame>();
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM flying_games WHERE session_id = $id;";

        var parameterId = command.CreateParameter();
        parameterId.ParameterName = "$id";
        parameterId.Value = id;
        command.Parameters.Add(parameterId);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                int game_id = reader.GetInt32(0);
                int session_id = reader.GetInt32(1);
                float distance_between_targets = reader.GetFloat(2);
                int total_targets = reader.GetInt16(3);
                float targets_size = reader.GetFloat(4);
                int targets_hit = reader.GetInt16(5);
                int expected_targets_hit = reader.GetInt16(6);
                int max_streak = reader.GetInt16(7);
                int expected_max_streak = reader.GetInt16(8);
                int targets_hit_importance = reader.GetInt16(9);
                int max_streak_importance = reader.GetInt16(10);
                int calification_targets_hit = reader.GetInt16(11);
                int calification_max_streak = reader.GetInt16(12);
                float GAS = reader.GetFloat(13);
                FlyingGame game = new FlyingGame(game_id, session_id, distance_between_targets, total_targets, targets_size, targets_hit, expected_targets_hit,
                    max_streak, expected_max_streak, targets_hit_importance, max_streak_importance, calification_targets_hit, calification_max_streak,  GAS);

            }
        }

        return flyingGames;
    }

    public Supervisor getSupervisorById(int id)
    {
        Supervisor s = null;
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM supervisors WHERE id = $id;";

        var parameterId = command.CreateParameter();
        parameterId.ParameterName = "$id";
        parameterId.Value = id;
        command.Parameters.Add(parameterId);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                s = new Supervisor(reader.GetInt32(0), reader.GetString(1));

            }
        }

        return s;
    }

    public Supervisor getSupervisorByNameAndPwd(string name, string password)
    {
        /*
         * Returns null if the password doesn't match the username or there is no such supervisor
         * 
         */
        Supervisor s = null;

        if ( dbConnection == null ) Debug.Log("Connection not established.");
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM supervisors WHERE name = $name;";

        var parameterId = command.CreateParameter();
        parameterId.ParameterName = "$name";
        parameterId.Value = name;
        command.Parameters.Add(parameterId);

        using (var reader = command.ExecuteReader())
        {

            while (reader.Read())
            {
                if (!encryptor.authenticate(password, reader.GetString(2))) return null;

                s = new Supervisor(reader.GetInt32(0), reader.GetString(1));
            }
        }

        return s;
    }

    public Player getPlayerByName(string name)
    {
        Player player = null;
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM players WHERE name = $name;";

        var parameterId = command.CreateParameter();
        parameterId.ParameterName = "$name";
        parameterId.Value = encryptor.encode(name);
        command.Parameters.Add(parameterId);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                player = new Player(reader.GetInt32(0), reader.GetString(1), reader.GetInt16(2));

            }
        }

        return player;
    }

    public Player getPlayerById(int id)
    {
        Player player = null;
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM players WHERE id = $id;";

        var parameterId = command.CreateParameter();
        parameterId.ParameterName = "$id";
        parameterId.Value = id;
        command.Parameters.Add(parameterId);

        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            {
                player = new Player(reader.GetInt32(0), reader.GetString(1), reader.GetInt16(2));

            }
        }

        return player;
    }

    public List<FlyingGame> getLast3SessionsFlyingGamesByPlayer(Player p)
    {
        List<FlyingGame> games = new List<FlyingGame>();
        IDbCommand command = dbConnection.CreateCommand();
        command.CommandText = "SELECT * FROM flying_games WHERE session_id = (SELECT session_id FROM sessions WHERE player = $id ORDER BY date DESC LIMIT 3);";

        var parameterId = command.CreateParameter();
        parameterId.ParameterName = "$id";
        parameterId.Value = p.id;
        command.Parameters.Add(parameterId);

        FlyingGame game;
        using (var reader = command.ExecuteReader())
        {
            while (reader.Read())
            { 
                int game_id = reader.GetInt32(0);
                int session_id = reader.GetInt32(1);
                float dist_between_targets = reader.GetFloat(2);
                int total_targets = reader.GetInt16(3);
                float target_size = reader.GetFloat(4);

                int targets_hit = reader.GetInt16(5);
                int expected_targets_hit = reader.GetInt16(6);

                int max_streak = reader.GetInt16(7);
                int expected_max_streak = reader.GetInt16(8);

                int targets_hit_weight = reader.GetInt16(9);
                int max_streak_weight = reader.GetInt16(10);

                int total_targets_punctuation = reader.GetInt16(11);
                int max_streak_punctuation = reader.GetInt16(12);

                float gas_score = reader.GetFloat(13);

                game = new FlyingGame(game_id, session_id, dist_between_targets, total_targets, target_size, targets_hit, expected_targets_hit, max_streak, expected_max_streak, 
                    targets_hit_weight, max_streak_weight, total_targets_punctuation, max_streak_punctuation, gas_score);

                games.Add(game);
            }
        }
        return games;
    }
}

