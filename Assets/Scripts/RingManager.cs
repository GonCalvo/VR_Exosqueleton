
#define DEBUG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;


public class RingManager : MonoBehaviour
{
   
    public int FIRST_DISTANCE = 120;

    public GameObject RING;
    public GameObject SCORE_BOARD;
    public int simultaneus_rings = 3;
    public int total_targets = 10;
    public float max_delta_right = 1;
    public float max_delta_left = 1;
    public float max_delta_up = 1; 
    public float max_delta_down = 1;
    public float distance_between_rings = 10;
    public float ring_size = 0.1f;
    public GameObject normal_display_cam;
    public FlyingGamePuntuation canvas;

    private float speed = 0.1f;

    private Transform cam_transform;
    private Transform controller_transform;
    private Vector3 initial_cam_pos;
    private Quaternion initial_cam_rot;
    private List<GameObject> rings;

    private int rezzed_rings;
    private ScoreManager score_manager;

    private void OnEnable()
    {
        if (DataHandler.Instance != null)
        {
            total_targets = DataHandler.Instance.FlyingGame.total_targets;
            distance_between_rings = DataHandler.Instance.FlyingGame.distance_between_targets;
            ring_size = DataHandler.Instance.FlyingGame.targets_size/10;
            max_delta_down = DataHandler.Instance.Session.range_bottom;
            max_delta_up = DataHandler.Instance.Session.range_top;
            max_delta_right = DataHandler.Instance.Session.range_right;
            max_delta_left = DataHandler.Instance.Session.range_left;
        }

        Debug.Log("Stats:" +
            "total_targets = " + total_targets +
            "distance_between = " + distance_between_rings +
            "ring_size = " + ring_size +
            "down = " + max_delta_down +
            "up = " + max_delta_up +
            "right = " + max_delta_right +
            "left = " + max_delta_left);


    }
    void CreateRing(Vector3 position)
    {
        if (RING == null)
        {
            print("Ring not created");
            return;
        }
        GameObject ring = Instantiate(RING);
        ring.transform.position = position;
        rings.Add(ring);

        Ring ring_script = ring.GetComponent<Ring>();

        ring_script.ring_manager = this;
        ring_script.speed = speed;
        ring_script.player = controller_transform;
        ring_script.ring_size = ring_size;
    }


    bool point_inside_ellipsis( Vector2 center, float x_axis_size, float y_axis_size, Vector2 point)
    {
        double result = (Math.Pow((point.x - center.x), 2) /
             Math.Pow(x_axis_size, 2)) +
            (Math.Pow((point.y - center.y), 2) /
             Math.Pow(y_axis_size, 2));
        return result <= 1;
    }

    // Start is called before the first frame update
    void Start()
    {
        rings = new List<GameObject>();
        Debug.Log("Starting Rings");
        if (simultaneus_rings > total_targets) simultaneus_rings = total_targets;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentItem = transform.GetChild(i);
            if (currentItem.name.Equals("VRCamera"))
            {
                cam_transform = currentItem.transform;
                Debug.Log("CamTransform found");
            }

            if (currentItem.name.Equals("RightHand"))
            {
                controller_transform = currentItem.transform;
            }
        }


        initial_cam_pos = cam_transform.transform.position;
        initial_cam_rot = cam_transform.rotation;

        for (int i = 0; i < simultaneus_rings; i++)
        {
            Vector2 ring_pos = NewRingPos();
            CreateRing(initial_cam_pos + new Vector3(ring_pos.x, ring_pos.y, FIRST_DISTANCE + (i + 1) * distance_between_rings));
            Vector2 pos = new Vector2(2, 3);
            rezzed_rings++;
        }

        score_manager = new ScoreManager( total_targets, Finish );

    }


    private bool isNotInsideEllipsis(Vector2 point)
    {
        float x_axis;
        float y_axis;
        if ( point.x <= cam_transform.position.x)
        {
            x_axis = max_delta_left;
        }
        else
        {
            x_axis = max_delta_right;
        }

        if (point.y <= cam_transform.position.y)
        {
            y_axis = max_delta_down;
        }
        else
        {
            y_axis = max_delta_up;
        }

        return ! point_inside_ellipsis(cam_transform.position, x_axis, y_axis, point);
    }

    private Vector2 NewRingPos()
    {
        Vector2 ring_pos = new Vector2(Random.Range(-max_delta_left, max_delta_right), Random.Range(-max_delta_down, max_delta_up));
        while (isNotInsideEllipsis(ring_pos))
        {
            ring_pos.Set(Random.Range(-max_delta_left, max_delta_right), Random.Range(-max_delta_down, max_delta_up));
        }
        return ring_pos;
    }

    Vector3 NewRingPos( Vector3 main_pos )
    {
        Vector2 new_pos = NewRingPos();
        return main_pos + new Vector3(new_pos.x, new_pos.y, (simultaneus_rings) * distance_between_rings); //TODO: Make sure it is in front of the camera.
    }

    

    public void RingScore( bool hit, Ring ring )
    {
        score_manager.Score(hit);

        if(rezzed_rings < total_targets)
        {
            ring.transform.position = NewRingPos(cam_transform.position);
            rezzed_rings++;
        }

    }

    private void Finish()
    {
        GameObject scoreboard = Instantiate(SCORE_BOARD);
        ScoreBoard scoreboard_script = scoreboard.GetComponent<ScoreBoard>();
        scoreboard_script.camera_transform = cam_transform;
        scoreboard_script.score = score_manager.score;
        scoreboard_script.targets = total_targets;
        scoreboard_script.max_spree = score_manager.max_streak;


        DataHandler.Instance.FlyingGame.maximum_streak = score_manager.max_streak;
        DataHandler.Instance.FlyingGame.targets_hit = score_manager.score;
        canvas.setUp();

        foreach (GameObject r in rings)
        {
            Destroy(r);
        }
        normal_display_cam.SetActive(true);

    }
}
