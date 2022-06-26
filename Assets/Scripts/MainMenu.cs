using Mono.Data.Sqlite;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string game_scene_flying;
    public string game_scene_SimonSays;

    private bool valid_username;
    private bool valid_password;
    private string username;
    private string password;


    private string player_name = "";

    private string game_scene = "";


    // Start is called before the first frame update
    void Start()
    {
        DataHandler.SQLmanager = new SQLManager("fisioterapia.db");

        try
        {
            DataHandler.SQLmanager.createTablePlayers();
            DataHandler.SQLmanager.createTableSupervisors();
            DataHandler.SQLmanager.createTableSessions();
            DataHandler.SQLmanager.createTableFlyingGames();
            DataHandler.SQLmanager.insertSupervisor(new Supervisor("Paco", "123$456"));
        }
        catch( SqliteException e)
        {
            Debug.LogWarning("Default supervisor already in.");
        }

        GetMenuByName("Login").gameObject.SetActive(true);
        GetMenuByName("Patient").gameObject.SetActive(false);
        GetMenuByName("NewPatient").gameObject.SetActive(false);
        game_scene = DropDownToString(0);
    }

    private Transform getElementByName(string name)
    {
        //returns submenu's items such as buttons or text inputs
        //Debug.Log("Searching for: " + name);
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentItem = transform.GetChild(i);
            //Debug.Log(currentItem.name);
            for (int j = 0; j < currentItem.childCount; j++)
            {
                Transform currentElement = currentItem.GetChild(j);
                //Debug.Log("Element inside element: " + currentElement.name);
                if (currentElement.name.Equals(name))
                {
                    return currentElement;
                }
            }
            
        }

        return null;
    }

    private Transform GetMenuByName(string name)
    {
        //Returns menu canvas.
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentItem = transform.GetChild(i);
            if (currentItem.name.Equals(name)) return currentItem;

        }

        return null;
    }

    //=====================================================================
    // MENU DE SELECCIÓN DE PACIENTE

    public void StartGame()
    {
        Player p = DataHandler.SQLmanager.getPlayerByName(player_name);
        if (p == null)
        {
            getElementByName("PatientNameError").gameObject.SetActive(true);
            return;
        }

        DataHandler.Instance.Player = p;
        DataHandler.Instance.Session = new Session(p, System.DateTime.Now, DataHandler.Instance.Supervisor);
        getElementByName("PatientNameError").gameObject.SetActive(false);

        // First, before each session, we have to calibrate.
        SceneManager.LoadScene("Calibracion");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }

    public void NewPatient()
    {
        GetMenuByName("Patient").gameObject.SetActive(false);
        GetMenuByName("NewPatient").gameObject.SetActive(true);
    }

    private bool is_valid_patient(string patient)
    {
        return patient.Length > 0;
    }

    public void GetGame(int option)
    {
        game_scene = DropDownToString(option);
    }

    private string DropDownToString(int pos)
    {
        switch (pos)
        {
            case 0:
                return game_scene_flying;
            case 1:
                return game_scene_SimonSays;
            default:
                print("No option selected");
                return "";
        }
    }

    public void getPatient(string s)
    {
        Debug.Log("Patient: "+s);

        getElementByName("ButtonStart").GetComponent<Button>().interactable = s.Length > 0;

        player_name = s;
        
    }

    //=====================================================================
    // MENU DE INICIO DE SESIÓN DE FISIO

    public void login()
    {
        if (!(valid_username && valid_password)) return;
        Supervisor supervisor = DataHandler.SQLmanager.getSupervisorByNameAndPwd(username, password);
        if ( supervisor != null )
        {
            DataHandler.Instance.Supervisor = supervisor;
            Debug.Log("Login Successful");
            GetMenuByName("Login").gameObject.SetActive(false);
            GetMenuByName("Patient").gameObject.SetActive(true);
        }
        else
        {
            getElementByName("LoginError").gameObject.SetActive(true);
        }
    }

    public void checkUsername(string s)
    {
        valid_username = s.Length > 0;
        if (valid_username) username = s;
        getElementByName("ButtonLogin").GetComponent<Button>().interactable = valid_username && valid_password;
    }

    public void checkPassword(string s)
    {
        valid_password = s.Length > 0;
        if (valid_password) password = s;
        getElementByName("ButtonLogin").GetComponent<Button>().interactable = valid_username && valid_password;
    }

    //===========================================================================
    // MENU DE REGISTRO DE PACIENTE

    private string newPatientName = "";
    private int newPatientAge = -1;


    public void registerPatient()
    {
        try
        {
            DataHandler.SQLmanager.insertPlayer(new Player(newPatientName, newPatientAge));
            registerPatientBack();
        } 
        catch(SqliteException e)
        {
            getElementByName("NewPatientRegisterError").gameObject.SetActive(true);
        }
        
    }

    public void registerPatientBack()
    {
        getElementByName("PatientnameInput").GetComponent<TMP_InputField>().text = "";
        getElementByName("NewPatientRegisterError").gameObject.SetActive(false);
        getElementByName("AgeInput").GetComponent<TMP_InputField>().text = "";

        GetMenuByName("NewPatient").gameObject.SetActive(false);
        GetMenuByName("Patient").gameObject.SetActive(true);
    }

    public void setNewPatientName(string s)
    {
        newPatientName = s;
        getElementByName("ButtonRegisterPatient").GetComponent<Button>().interactable = newPatientName.Length > 0 && newPatientAge >= 0;

    }

    public void setNewPatientAge(string s)
    {
        if (s!= "") newPatientAge = int.Parse(s);
        getElementByName("ButtonRegisterPatient").GetComponent<Button>().interactable = newPatientName.Length > 0 && newPatientAge >= 0;

    }
    //===========================================================================

    private void OnDisable()
    {
        Debug.Log("Saving the name");
        PlayerPrefs.SetString("game", game_scene);

    }

}
