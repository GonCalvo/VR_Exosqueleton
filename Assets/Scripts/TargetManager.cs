using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Valve.VR.Extras;

public class TargetManager : MonoBehaviour
{

    public SteamVR_LaserPointer laserPointer;
    public GameObject SCORE_BOARD;
    public GameObject target;
    public GameObject explosion;
    public int total_targets = 10;
    public float max_delta_x = 2;
    public float max_delta_y = 10;
    public float time_between_targets = 5;

    private float last_target_rez_time = 0;
    private const float z_dist = 18.5f;

    private bool finished = false;
    private Transform cam_transform;

    private ScoreManager score_manager;

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;
        score_manager = new ScoreManager(total_targets, Finish);
        MoveTarget();
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentItem = transform.GetChild(i);
            if (currentItem.name.Equals("Camera"))
            {
                cam_transform = currentItem.transform;
            }
        }

        var particles = target.transform.GetChild(0).GetComponent<ParticleSystem>().emission;
        particles.enabled = false;
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.transform == target.transform)
        {
            //Destroy(e.target.gameObject);
            if ( ! finished )
            {
                score_manager.Score(true);
                GameObject _explosion = Instantiate( explosion, target.transform);
                Destroy(_explosion, 1f);
                MoveTarget();
            }
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
        finished = true;
        GameObject scoreboard = Instantiate(SCORE_BOARD);
        ScoreBoard scoreboard_script = scoreboard.GetComponent<ScoreBoard>();
        scoreboard_script.camera_transform = cam_transform;
        scoreboard_script.score = score_manager.score;
        scoreboard_script.targets = total_targets;
        scoreboard_script.max_spree = score_manager.max_streak;
    }
}

