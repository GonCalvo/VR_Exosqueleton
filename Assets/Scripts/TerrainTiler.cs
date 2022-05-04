using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainTiler : MonoBehaviour
{

    public float speed = 10f;
    public Terrain terrain;

    private Terrain next_terrain;
    private Terrain third_terrain;
    private void OnEnable()
    {
        speed = PlayerPrefs.GetFloat("flight_speed");
    }


    // Start is called before the first frame update
    void Start()
    {
        if (terrain == null) throw new System.Exception("Missing the starting terrain");


        next_terrain = Instantiate(terrain);
        terrain.SetNeighbors(null, next_terrain, null, null);
        next_terrain.transform.position = new Vector3(0, 0, 1000);

        third_terrain = Instantiate(terrain);
        next_terrain.SetNeighbors(null, third_terrain, null, terrain);
        third_terrain.transform.position = new Vector3(0, 0, 2000);
        third_terrain.SetNeighbors(null, null, null, next_terrain);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 change = new Vector3(0, 0, -speed);
        terrain.transform.position += change;
        next_terrain.transform.position += change;
        third_terrain.transform.position += change;

        if (terrain.transform.position.z <= -1000)
        {
            Terrain aux = terrain;
            terrain = next_terrain;
            next_terrain = third_terrain;
            third_terrain = aux;

            next_terrain.SetNeighbors(null, third_terrain, null, terrain);
            third_terrain.SetNeighbors(null, null, null, next_terrain);
            third_terrain.transform.position = new Vector3(0, 0, 2000);
        }
    }
}
