using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Valve.VR.InteractionSystem;


public class Test : MonoBehaviour
{

#define DEBUG

    public SteamVR_TrackedObject controller1;
    public SteamVR_TrackedObject controller2;
    private SteamVR_Controller.Device controller_right { get { return SteamVR_Controller.Input((int)controller1.index); } }
    private SteamVR_Controller.Device controller_left { get { return SteamVR_Controller.Input((int)controller2.index); } }
    private Valve.VR.EVRButtonId trigger = Valve.VR.EVRButtonId.k_EButton_SteamVR_Trigger;

    // Start is called before the first frame update
    void Start()
    {
        bool left_pressed = controller_left.GetPressUp(trigger);
        bool right_pressed =  controllerright.GetPressUp(trigger);
    }

    // Update is called once per frame
    void Update()
    {
        bool left_pressed = controller_left.GetPressUp(trigger);
        bool right_pressed =  controllerright.GetPressUp(trigger);
        
        #if DEBUG
            Console.WriteLine($"Left button pressed: {left_pressed ? "Yes" : "No"}");
            Console.WriteLine($"Right button pressed: {right_pressed ? "Yes" : "No"}");
        #endif

        
    }
}
