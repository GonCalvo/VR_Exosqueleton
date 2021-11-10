#define DEBUG

using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class Tracker : MonoBehaviour {

    public bool tracking_pos;
    public bool tracking_rot;
    public bool tracking_vel;
    public string file_name = "";

    public SteamVR_TrackedObject controller1;
    public SteamVR_TrackedObject controller2;
    private SteamVR_Controller.Device controller_right { get { return SteamVR_Controller.Input((int)controller1.index); } }
    private SteamVR_Controller.Device controller_left { get { return SteamVR_Controller.Input((int)controller2.index); } }
    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;
    
    public RigidBody rb;

    private integer counter;

    // Start is called before the first frame update
    void Start() {

        bool left_pressed = controller_left.GetPressUp(trigger);
        bool right_pressed =  controller_right.GetPressUp(trigger);

        if ( file_name != "" ) {
            DateTime localDate = DateTime.Now;
            file_name = $"./Records/{file_name}/{localDate.Day}-{localDate.Month}_{localDate.Hour}-{localDate.Minute}_{this.Name}";
        }

        rb = GetComponent<Rigidbody>();
        if ( rb == null ) tracking_vel = false;

        counter = 0;
    }

    // Update is called once per frame
    void Update()
    {
        bool left_pressed = controller_left.GetPressUp(trigger);
        bool right_pressed =  controller_right.GetPressUp(trigger);
        
        #if DEBUG
            Debug.Log( $"Left button pressed: {(left_pressed ? "Yes" : "No")}" );
            Debug.Log( $"Right button pressed: {(right_pressed ? "Yes" : "No")}" );
        #endif
        if (file_name != "") {
            using ( System.IO.StreamWriter file = new System.IO.StreamWriter(file_name) ) {
                file.Write($"{counter}_{Time.time}"+"{");
                if ( tracking_pos ) this.record_measure_position(file, transform.pos );

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
        f.Write( "Position:" + rot.ToString() + ";" );
    }

    void record_measure_velocity ( StreamWriter f, Vector3 vel ) {
        f.Write( "Position:" + vel.ToString() + ";" );
    } 



}
