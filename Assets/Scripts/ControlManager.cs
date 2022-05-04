using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public Vector2 calibrated_pos; // relative to the rig.
    public float THRESHOLD_X; // Distance the controller must move in the X axis before it is considered that the controller is being used to move in that axis
    public float THRESHOLD_Y; // Distance the controller must move in the Y axis before it is considered that the controller is being used to move in that axis
    public float manouver_speed; // speed at which the object will move in the xy axis

    private Transform cam_transform;
    private Transform controller_transform;

    private Vector3 initial_pos;

    private void OnEnable()
    {
        manouver_speed = PlayerPrefs.GetFloat("manouver_speed");
        THRESHOLD_X = PlayerPrefs.GetFloat("x_threshold");
        THRESHOLD_Y = PlayerPrefs.GetFloat("y_threshold");
    }


    // Start is called before the first frame update
    void Start()
    {
        cam_transform = this.GetComponentsInParent<Transform>()[0];
        controller_transform = this.GetComponentsInParent<Transform>()[1];

        initial_pos = cam_transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 controller_relative_pos = get_relative_position(cam_transform.position, controller_transform.position); // Now it's relative to the rig

        //Vector2 relative_pos = new Vector2(controller_transform.position.x - calibrated_pos.x, controller_transform.position.y - calibrated_pos.y);
        Vector2 relative_pos = new Vector2(controller_relative_pos.x - calibrated_pos.x, controller_relative_pos.y - calibrated_pos.y);

        Vector2 movement = new Vector2(0,0);
        if ( Math.Abs(relative_pos.x) > THRESHOLD_X )
        {
            float mod = THRESHOLD_X;
            if (relative_pos.x < 0) mod = -THRESHOLD_X;
            movement.x = relative_pos.x + mod;
        }

        if (Math.Abs(relative_pos.y) > THRESHOLD_Y)
        {
            float mod = THRESHOLD_Y;
            if (relative_pos.y < 0) mod = -THRESHOLD_Y;
            movement.x = relative_pos.y + mod;
        }

        cam_transform.position = Vector3.Lerp(cam_transform.position, cam_transform.position + new Vector3(movement.x, movement.y, 0), manouver_speed);

    }

    private Vector3 get_relative_position( Vector3 v1, Vector3 v2)
    {
        // returns the position of the second vector related to te first.
        return v2 - v1;
    }
}
