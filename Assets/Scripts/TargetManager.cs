using System;
using UnityEngine;
using Valve.VR.Extras;

public class TargetManager : MonoBehaviour
{

    public SteamVR_LaserPointer laserPointer;
    public GameObject SCORE_BOARD;
    public GameObject target;
    public GameObject explosion;
    public GameObject corridor;
    public GameObject timer;
    public int total_targets = 10;
    public float max_delta_x = 4;
    public float max_delta_y = 2.5f;
    public float time_between_targets = 5;
    private float target_size = 1;

    private float last_target_rez_time = 0;
    private const float z_dist = 18.5f;

    private Vector3 target_init_pos;

    private bool finished = false;
    private Transform cam_transform;

    private ScoreManager score_manager;

    void Awake()
    {
        laserPointer.PointerIn += PointerInside;

        score_manager = new ScoreManager(total_targets, Finish);
        MoveTarget();

        float auxf = 0f;
        int auxi = 0;
        Debug.Log("Player_name = " + PlayerPrefs.GetString("name"));

        auxi = PlayerPrefs.GetInt("target_number");
        //if (auxi != 0) total_targets = auxi;
        Debug.Log("target_number = " + total_targets);

        auxf =  PlayerPrefs.GetFloat("flight_speed");
        //if (auxf != 0) speed = auxf;
        //Debug.Log("speed = " + speed);


        auxf = PlayerPrefs.GetFloat("target_x_max_distance");
        //if (auxf != 0) max_delta_x = auxf;
        Debug.Log("max_delta_x = " + max_delta_x);

        auxf = PlayerPrefs.GetFloat("target_y_max_distance");
        //if (auxf != 0) max_delta_y = auxf;
        Debug.Log("max_delta_y = " + max_delta_y);

        auxf = PlayerPrefs.GetFloat("target_z_distance");
        //if (auxf != 0) distance_between_rings = auxf;
        //Debug.Log("distance_between_rings = " + distance_between_rings);

        auxi = PlayerPrefs.GetInt("target_size");
        //if (auxi != 0) target_size = auxi;
        Debug.Log("ring_size = " + target_size);
        target.transform.localScale = new Vector3(target_size, target.transform.localScale.y, target_size);
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
        target_init_pos = target.transform.position;
    }

    public void PointerInside(object sender, PointerEventArgs e)
    {
        if (e.target.transform.name == target.transform.name)
        {
            //Destroy(e.target.gameObject);
            if ( ! finished )
            {
                score_manager.Score(true);
                //GameObject _explosion = Instantiate( explosion, target.transform);
                //Destroy(_explosion, 1f);
                MoveTarget();
            }
        }
    }

    void Update()
    {
        if (!finished)
        {
            float time_since_target_moved = Time.time - last_target_rez_time;
            if (time_since_target_moved >= time_between_targets)
            {
                score_manager.Score(false);
                MoveTarget();
            }
            else
            {
                timer.GetComponent<TextMesh>().text = ((int)Math.Ceiling(time_between_targets - time_since_target_moved)).ToString();
            }
        }
    }

    private void MoveTarget()
    {
        //float original_x = target_init_pos.x;
        //float original_y = target_init_pos.y;
        float original_x = 0;
        float original_y = 2.75f;
        print("Original_x: " + original_x);
        print("Original_y: " + original_x);
        target.transform.position = new Vector3(original_x + UnityEngine.Random.Range(-max_delta_x, max_delta_x), original_y + UnityEngine.Random.Range(-max_delta_y, max_delta_y), z_dist);
        print("Transform.position: " + target.transform.position);
        last_target_rez_time = Time.time;
        timer.GetComponent<TextMesh>().text = time_between_targets.ToString();
    }

    private void Finish()
    {
        finished = true;
        GameObject scoreboard = Instantiate(SCORE_BOARD);
        ScoreBoard scoreboard_script = scoreboard.GetComponent<ScoreBoard>();
        scoreboard_script.camera_transform = cam_transform;
        scoreboard_script.DISTANCE = 3;
        scoreboard.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        scoreboard_script.score = score_manager.score;
        scoreboard_script.targets = total_targets;
        scoreboard_script.max_spree = score_manager.max_streak;
        Destroy(target);
        timer.SetActive(false);
    }
}

