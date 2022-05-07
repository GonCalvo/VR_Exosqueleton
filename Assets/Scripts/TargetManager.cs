using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.Extras;

public class TargetManager : MonoBehaviour, Manager
{

    public SteamVR_LaserPointer laserPointer;
    public GameObject target;
    public int total_targets = 10;
    public float max_delta_x = 2;
    public float max_delta_y = 10;
    public float time_between_targets = 5;

    private float last_target_rez_time = 0;
    private const float z_dist = 10f;

    private ScoreManager score_manager;

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        score_manager = new ScoreManager(total_targets, Finish);
        MoveTarget();
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.transform == target.transform)
        {
            //Destroy(e.target.gameObject);
            score_manager.Score(true);
            MoveTarget();
        }
    }

    void Update()
    {
        if (Time.time - last_target_rez_time > time_between_targets)
        {
            score_manager.Score(false);
            MoveTarget();
        }
    }

    private void MoveTarget()
    {
        float original_x = 0;
        float original_y = 1;
        target.transform.position = new Vector3(original_x + Random.Range(-max_delta_x, max_delta_x), original_y + Random.Range(-max_delta_y, max_delta_y), z_dist);
        last_target_rez_time = Time.time;
    }

    private void Finish()
    {

    }
}

