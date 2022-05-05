
#define DEBUG

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RingManager : MonoBehaviour
{
   
    public int FIRST_DISTANCE = 120;

    public GameObject RING;
    public GameObject SCORE_BOARD;
    public int simultaneus_rings = 3;
    public float distance_between_rings = 20;
    public int total_rings = 10;
    public float max_delta_x = 2;
    public float max_delta_y = 10;
    public float ring_size = 5;

    private float speed = 0.3f;
    private int score = 0;

    private int spree = 0;
    private int max_spree = 0;

    private Transform cam_transform;
    private Vector3 initial_cam_pos;
    private int rings_created;
    private int rings_passed;
    private void OnEnable()
    {
        float auxf = 0f;
        int auxi = 0;
        Debug.Log("Player_name = " + PlayerPrefs.GetString("name"));
        
        //auxi = PlayerPrefs.GetInt("target_number");
        if (auxi != 0) total_rings = auxi;
        Debug.Log("target_number = " + total_rings);

        //auxf =  PlayerPrefs.GetFloat("flight_speed");
        if (auxf != 0) speed = auxf;
        Debug.Log("speed = " + speed);

        //auxf = PlayerPrefs.GetFloat("target_size");
        if (auxf != 0) ring_size = auxf;
        Debug.Log("ring_size = " + ring_size);


        //auxf = PlayerPrefs.GetFloat("target_x_max_distance");
        if (auxf != 0) max_delta_x = auxf;
        Debug.Log("max_delta_x = " + max_delta_x);

        //auxf = PlayerPrefs.GetFloat("target_y_max_distance");
        if (auxf != 0) max_delta_y = auxf;
        Debug.Log("max_delta_y = " + max_delta_y);

        //auxf = PlayerPrefs.GetFloat("target_z_distance");
        if (auxf != 0) distance_between_rings = auxf;
        Debug.Log("distance_between_rings = " + distance_between_rings);

        //auxi = PlayerPrefs.GetInt("target_size");
        if (auxi != 0) ring_size = auxi;
        Debug.Log("ring_size = " + ring_size);

    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting Rings");
        if (simultaneus_rings > total_rings) simultaneus_rings = total_rings;

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentItem = transform.GetChild(i);
            if (currentItem.name.Equals("Camera"))
            {
                cam_transform = currentItem.transform;
            }
        }

        initial_cam_pos = cam_transform.transform.position;

        for (int i = 0; i < simultaneus_rings; i++)
        {
            rings_created++;
            CreateRing(initial_cam_pos + new Vector3(Random.Range(-max_delta_x, max_delta_x), Random.Range(-max_delta_y, max_delta_x), FIRST_DISTANCE + (i + 1) * distance_between_rings));
        }

    }

    Vector3 NewRingPos( Vector3 main_pos, float max_delta_x, float max_delta_y )
    {
        float new_x = Random.Range(-max_delta_x, max_delta_x);
        float new_y = Random.Range(-max_delta_y, max_delta_x);
        return main_pos + new Vector3(new_x,new_y, (simultaneus_rings+1) * distance_between_rings);
    }

    void CreateRing(Vector3 position)
    {
        GameObject ring = Instantiate(RING);
        ring.transform.position = position;

        Ring ring_script = ring.GetComponent<Ring>();
        ring_script.ring_manager = this;
        ring_script.speed = speed;
        ring_script.player = cam_transform;
        ring_script.ring_size = ring_size;
    }

    public void RingScore( bool hit, Ring ring )
    {
        if (hit)
        {
            score++;
            spree++;
            MonoBehaviour.print("Player scored a point");
        } 
        else
        {
            if (spree > max_spree) max_spree = spree;
            spree = 0;
        }

        if (rings_created < total_rings)
        {
            rings_created++;
            ring.transform.position = NewRingPos(initial_cam_pos, max_delta_x, max_delta_y);
        }


        rings_passed++;
        MonoBehaviour.print("Rings passed: " + rings_passed);
        if ( rings_passed == total_rings )
        {
            MonoBehaviour.print( "Game finished! Score: " + score + "/" + total_rings + " targets, max_spree = "+max_spree );
            rings_passed++; // to prevent the previous condition to activate again
            gameObject.GetComponent<TerrainTiler>().speed = 0.05f;
            GameObject scoreboard = Instantiate(SCORE_BOARD);
            ScoreBoard scoreboard_script = scoreboard.GetComponent<ScoreBoard>();
            scoreboard_script.camera_transform = cam_transform;
            scoreboard_script.score = score;
            scoreboard_script.targets = total_rings;
            scoreboard_script.max_spree = max_spree;
        }

    }
}
