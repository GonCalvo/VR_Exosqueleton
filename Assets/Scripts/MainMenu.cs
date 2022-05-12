using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public string game_scene_flying;
    public string game_scene_targetrange;


    private string player_name = "";
    private float flight_speed = 0.3f;
    private float manouver_speed = 0.3f;
    private float x_threshold = 0.3f;
    private float y_threshold = 0.3f;
    private float target_x_max_distance = 2;
    private float target_y_max_distance = 2;
    private float target_z_distance = 60;
    private int target_size = 5;
    private int target_number = 10;

    private string game_scene = "";


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        if ( game_scene == "" )
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform currentItem = transform.GetChild(i);
                if (currentItem.name.Equals("Dropdown"))
                {
                    game_scene = DropDownToString(currentItem.GetComponent<TMP_Dropdown>().value);
                }
            }
        }
        print("Loading gameScene: " + game_scene);
        SceneManager.LoadScene(game_scene);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quitting");
    }

    public void NewPatient()
    {

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
                return game_scene_targetrange;
            case 1:
                return game_scene_flying;
            default:
                print("No option selected");
                return "";
        }
    }

    public void getPatient(string s)
    {
        Debug.Log("Patient: "+s);


        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentItem = transform.GetChild(i);
            if (currentItem.name.Equals("ButtonStart"))
            {
                currentItem.GetComponent<Button>().interactable = is_valid_patient(s);
            }
        }

        player_name = s;
        
    }

    private void OnDisable()
    {
        Debug.Log("Saving the name");
        PlayerPrefs.SetString("name", player_name);
        PlayerPrefs.SetFloat("flight_speed", flight_speed);
        PlayerPrefs.SetFloat("manouver_speed", manouver_speed);
        PlayerPrefs.SetFloat("x_threshold", x_threshold);
        PlayerPrefs.SetFloat("y_threshold", y_threshold);
        PlayerPrefs.SetFloat("target_x_max_distance", target_x_max_distance);
        PlayerPrefs.SetFloat("target_y_max_distance", target_y_max_distance);
        PlayerPrefs.SetFloat("target_z_distance", target_z_distance);
        PlayerPrefs.SetInt("target_size", target_size);
        PlayerPrefs.SetInt("target_number", target_number);

    }

}
