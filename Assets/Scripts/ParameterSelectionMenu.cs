using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ParameterSelectionMenu : MonoBehaviour
{
    TMP_InputField dist_between_targets_IF;
    TMP_InputField total_targets_IF;
    TMP_InputField total_targets_objective_IF;
    TMP_InputField max_streak_objective_IF;
    TMP_Dropdown total_targets_importance_IF;
    TMP_Dropdown max_streak_importance_IF;
    TMP_Dropdown total_targets_difficulty_IF;
    TMP_Dropdown max_streak_difficulty_IF;
    TMP_InputField target_size_IF;


    TMP_Dropdown game_selector;
    Button game_start;

    public void Start()
    {
        dist_between_targets_IF = getElementByName("DistanceBetweenTargetsInput").GetComponent<TMP_InputField>();
        total_targets_IF = getElementByName("TotalTargetsInput").GetComponent<TMP_InputField>();
        target_size_IF = getElementByName("TargetsSizeInput").GetComponent<TMP_InputField>();
        total_targets_objective_IF = getElementByName("TargetObjectiveInput").GetComponent<TMP_InputField>();
        max_streak_objective_IF = getElementByName("StreakObjectiveInput").GetComponent<TMP_InputField>();
        total_targets_importance_IF = getElementByName("TargetImportanceInput").GetComponent<TMP_Dropdown>();
        max_streak_importance_IF = getElementByName("StreakImportanceInput").GetComponent<TMP_Dropdown>();
        total_targets_difficulty_IF = getElementByName("TargetDificultyInput").GetComponent<TMP_Dropdown>();
        max_streak_difficulty_IF = getElementByName("StreakDificultyInput").GetComponent<TMP_Dropdown>();

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentElement = transform.GetChild(i);
            //Debug.Log("Element inside element: " + currentElement.name);
            if (currentElement.name.Equals("StartButton"))
            {
                game_start = currentElement.GetComponent<Button>();
            }
            else if ( currentElement.name.Equals("ExerciseDropdown"))
            {
                game_selector = currentElement.GetComponent<TMP_Dropdown>();
            }

        }

        //calculate rules
        RuleSystem rules = new RuleSystem(DataHandler.SQLmanager.getLast3SessionsFlyingGamesByPlayer(DataHandler.Instance.Player));
        dist_between_targets_IF.text = rules.GetDistBetweenTargets().ToString();
        total_targets_IF.text = rules.GetTargets().ToString();
        target_size_IF.text = rules.GetTargetSize().ToString();

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

    private Transform GetCanvasByName(string name)
    {
        //Returns menu canvas.
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentItem = transform.GetChild(i);
            if (currentItem.name.Equals(name)) return currentItem;

        }

        return null;
    }

    public void changeExercise(int i)
    {
        string exercise = getExerciseFromDropDown(i);

        if ( exercise != string.Empty)
        {
            GetCanvasByName(exercise).gameObject.SetActive(true);
        }
    }

    private string getExerciseFromDropDown(int i)
    {
        if (i == 0) return "AsteroidParameter";

        return string.Empty;
    }

    public void startGame()
    {

        string exercise = getExerciseFromDropDown(game_selector.value);
        if (exercise == "AsteroidParameter")
        {
            int dist_between_targets = int.Parse(dist_between_targets_IF.text);
            int total_targets = int.Parse(total_targets_IF.text);
            int targets_size = int.Parse(target_size_IF.text);
            int total_targets_objective = int.Parse(total_targets_objective_IF.text);
            int max_streak_objective = int.Parse(max_streak_objective_IF.text);
            int total_targets_importance = total_targets_importance_IF.value;
            int max_streak_importance = max_streak_importance_IF.value;
            int total_targets_difficulty = total_targets_difficulty_IF.value;
            int max_streak_difficulty = max_streak_difficulty_IF.value;

            FlyingGame game = new FlyingGame(DataHandler.Instance.Session.id, dist_between_targets, total_targets, targets_size,
                total_targets_objective, max_streak_objective, total_targets_importance, max_streak_importance, total_targets_difficulty, max_streak_difficulty);
            
            DataHandler.Instance.FlyingGame = game;

            SceneManager.LoadScene("Flying");
        }
    }

    public void endSession()
    {
        SceneManager.LoadScene("Menu");
    }

    public void checkValues(string name)
    {
        game_start.interactable = dist_between_targets_IF.text != "" &&
                                  total_targets_IF.text != "" &&
                                  target_size_IF.text != "" && 
                                  total_targets_objective_IF.text != "" &&
                                  max_streak_objective_IF.text != "";

    }
}
