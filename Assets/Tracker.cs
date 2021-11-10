#define DEBUG

using System;
using System.IO;
using UnityEngine;
using Valve.VR;

public class Tracker : MonoBehaviour {

    public bool tracking_pos;
    public bool tracking_rot;
    public bool tracking_vel;
    public string file_name = "";

    
    private Rigidbody rb;

    private int counter;

    // Start is called before the first frame update
    void Start() {


        if ( file_name != "" ) {
            DateTime localDate = DateTime.Now;
            file_name = $"./Records/{file_name}/{localDate.Day}-{localDate.Month}_{localDate.Hour}-{localDate.Minute}_{this.name}";
        }

        rb = GetComponent<Rigidbody>();
        if ( rb == null ) tracking_vel = false;

        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (file_name != "") {
            using ( System.IO.StreamWriter file = new System.IO.StreamWriter(file_name) ) {
                file.Write($"{counter}_{Time.time}"+"{");
                if ( tracking_pos ) this.record_measure_position(file, transform.position );

                if ( tracking_rot ) this.record_measure_rotation(file, transform.rotation );

                if ( tracking_vel ) this.record_measure_velocity(file, rb.velocity );
                file.Write("}\n");   
                counter++;         
            }
        }    
    }

    void record_measure_position ( StreamWriter f, Vector3 pos ) {
        f.Write( "Position:" + pos.ToString() + ";" );
    }

    void record_measure_rotation ( StreamWriter f, Quaternion rot ) {
        f.Write( "Rotation:" + rot.ToString() + ";" );
    }

    void record_measure_velocity ( StreamWriter f, Vector3 vel ) {
        f.Write( "Velocity:" + vel.ToString() + ";" );
    } 



}
