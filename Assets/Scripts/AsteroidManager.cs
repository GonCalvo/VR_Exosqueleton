using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AsteroidManager : MonoBehaviour
{
    public List<GameObject> asteroids_prefabs;
    public int asteroid_num = 32;
    public float speed = 0.3f;
    public float max_delta_x = 10;
    public float max_delta_y = 5;
    public float FIRST_DISTANCE = 120;
    public int total_targets = 10;

    private Transform cam_transform;
    private Vector3 initial_cam_pos;
    private List<GameObject> asteroids;
    private float distance_between_rings;

    private void OnEnable()
    {
        float auxf = 0f;
        int auxi = 0;
        Debug.Log("Player_name = " + PlayerPrefs.GetString("name"));

        auxf = PlayerPrefs.GetFloat("flight_speed");
        if (auxf != 0) speed = auxf;
        Debug.Log("speed = " + speed);

        auxf = PlayerPrefs.GetFloat("target_x_max_distance");
        if (auxf != 0) max_delta_x = auxf;
        Debug.Log("max_delta_x = " + max_delta_x);

        auxf = PlayerPrefs.GetFloat("target_y_max_distance");
        if (auxf != 0) max_delta_y = auxf;
        Debug.Log("max_delta_y = " + max_delta_y);

        auxi = PlayerPrefs.GetInt("target_number");
        if (auxi != 0) total_targets = auxi;
        Debug.Log("target_number = " + total_targets);

        auxf = PlayerPrefs.GetFloat("target_z_distance");
        if (auxf != 0) distance_between_rings = auxf;
        Debug.Log("distance_between_rings = " + distance_between_rings);
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Starting Asteroids");

        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentItem = transform.GetChild(i);
            if (currentItem.name.Equals("Camera"))
            {
                cam_transform = currentItem.transform;
            }
        }

        initial_cam_pos = cam_transform.transform.position;
        asteroids = new List<GameObject>();

        for (int i = 0; i < asteroid_num; i++)
        {
            float x = Random.Range(max_delta_x, max_delta_x * 2) * Mathf.Pow(-1, Random.Range(0,2));
            float y = Random.Range(max_delta_y, max_delta_y * 2) * Mathf.Pow(-1, Random.Range(0, 2));
            float z = Random.Range(FIRST_DISTANCE, (total_targets + 1) * distance_between_rings);
            CreateAsteroid(initial_cam_pos + new Vector3(x, y, z));
        }


    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject asteroid in asteroids) {
            if (asteroid.transform.position.z > 0)
            {
                asteroid.transform.position = asteroid.transform.position + new Vector3(0, 0, -speed);
            }
            else
            {

                float x = Random.Range(max_delta_x, max_delta_x * 2) * Mathf.Pow(-1, Random.Range(0, 2));
                float y = Random.Range(max_delta_y, max_delta_y * 2) * Mathf.Pow(-1, Random.Range(0, 2));
                float z = (total_targets + 1) * distance_between_rings;
                asteroid.transform.position = new Vector3(x, y, z);
            }
        }
    }

    private void CreateAsteroid(Vector3 position)
    {
        int prefab_pos = Random.Range(0, asteroids_prefabs.Count);
        GameObject prefab = asteroids_prefabs[prefab_pos];
        GameObject asteroid = Instantiate(prefab, position, Quaternion.Euler(Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f), Random.Range(0.0f, 360.0f)) );
        asteroids.Add(asteroid);
    }
}
