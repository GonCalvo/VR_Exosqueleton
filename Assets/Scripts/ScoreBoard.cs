using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    private int _score;
    private int _targets;
    private int _max_spree;

    private Transform _camera_transform;

    public int DISTANCE = 30;


    public int score
    {
        get { return _score; }
        set 
        { 
            _score = value;
            for ( int i = 0; i < transform.childCount; i++)
            {
                Transform currentItem = transform.GetChild(i);
                if ( currentItem.name.Equals("score") )
                {
                    currentItem.GetComponent<TextMeshPro>().text = "Puntuación: " + _score;
                }
            }
        }
    }

    public int targets
    {
        get { return _targets; }
        set
        {
            _targets = value;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform currentItem = transform.GetChild(i);
                if (currentItem.name.Equals("targets"))
                {
                    currentItem.GetComponent<TextMeshPro>().text = "Dianas totales: " + _targets;
                }
            }
        }
    }
    public int max_spree
    {
        get { return _max_spree; }
        set
        {
            _max_spree = value;
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform currentItem = transform.GetChild(i);
                if (currentItem.name.Equals("spree"))
                {
                    currentItem.GetComponent<TextMeshPro>().text = "Maximo seguidos: " + _max_spree;
                }
            }
        }
    }

    public Transform camera_transform
    {
        get { return _camera_transform; }
        set
        {
            _camera_transform = value;
            move_in_front(_camera_transform);
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        MonoBehaviour.print("I created a Scoreboard");
    }

    // Update is called once per frame
    void Update()
    {
        move_in_front(camera_transform);
    }

    void move_in_front(Transform t)
    { 
        //put the score before the camera
        transform.position = t.position + (t.rotation * Vector3.forward) * DISTANCE;
        
        //Make the score face the camera
        Vector3 relativePos = transform.position - t.position;
        transform.rotation = Quaternion.LookRotation(relativePos);


    }
}
