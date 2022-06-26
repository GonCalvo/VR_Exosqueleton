using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FlyingGamePuntuation : MonoBehaviour
{
    private TextMeshProUGUI TargetsHit;
    private TextMeshProUGUI TargetsExpected;
    private TextMeshProUGUI Streak;
    private TextMeshProUGUI StreakExpected;

    private TMP_Dropdown targets_DD;
    private TMP_Dropdown streak_DD;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if ( child.name.Equals("TargetsHit") )
            {
                TargetsHit = child.GetComponent<TextMeshProUGUI>();
            }
            else if (child.name.Equals("MaxStreak"))
            {
                Streak = child.GetComponent<TextMeshProUGUI>();
            }
            else if (child.name.Equals("TargetsExpected"))
            {
                TargetsExpected = child.GetComponent<TextMeshProUGUI>();
            }
            else if (child.name.Equals("StreakExpected"))
            {
                StreakExpected = child.GetComponent<TextMeshProUGUI>();
            }
            else if (child.name.Equals("TargetsHitDropdown"))
            {
                targets_DD = child.GetComponent<TMP_Dropdown>();
            }
            else if (child.name.Equals("StreakDropdown"))
            {
                streak_DD = child.GetComponent<TMP_Dropdown>();
            }
        }
    }

    // Start is called before the first frame update
    private int DropDownToScore(int pos)
    {
        return pos - 2;
    }

    public void setUp()
    {
        if (DataHandler.Instance.FlyingGame == null) Debug.Log("Flying game non-existent");
        TargetsHit.SetText(DataHandler.Instance.FlyingGame.targets_hit.ToString());
        TargetsExpected.text = DataHandler.Instance.FlyingGame.expected_targets_hit.ToString();

        Streak.text = DataHandler.Instance.FlyingGame.maximum_streak.ToString();
        StreakExpected.text = DataHandler.Instance.FlyingGame.expected_max_streak.ToString();

    }

    public void puntuate()
    {
        DataHandler.Instance.FlyingGame.calification_targets_hit = DropDownToScore(targets_DD.value);
        DataHandler.Instance.FlyingGame.calification_max_streak = DropDownToScore(streak_DD.value);
        DataHandler.Instance.FlyingGame.CalculateGASScore();

        DataHandler.SQLmanager.insertFlyingGame(DataHandler.Instance.FlyingGame);
        SceneManager.LoadScene("Calibracion");
    }
}
