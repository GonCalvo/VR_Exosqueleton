using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour
{
    public GameObject paint;
    public Color paint_color;
    public GameObject cam;
    private bool painting = false;
    // Start is called before the first frame update

    private Vector3 drawing_center = Vector3.zero;
    private List<Vector3> drawing;

    private Quaternion cam_original_rot;
    void Start()
    {
        drawing = new List<Vector3>();
    }

    // Update is called once per frame
    public void Update()
    {
        if ( painting )
        {
            Draw();
            print("Adding " + (cam.transform.position - transform.position));
            drawing.Add(cam.transform.position - transform.position);
        }
        
        // button was released
        if (SteamVR_Actions.default_DrawBrush.GetChanged(SteamVR_Input_Sources.RightHand) && !SteamVR_Actions.default_DrawBrush.GetState(SteamVR_Input_Sources.RightHand))
        {

            if ( drawing_center == Vector3.zero)
            {
                drawing_center = cam.transform.position - transform.position;
                cam_original_rot = cam.transform.rotation;
                // TODO: Calculate drawing points in respect to the camera original rotation center of coordinates.
                print("Center = " + drawing_center);
            } 
            else
            {
                print("Displaying Brush!"); 
                painting = !painting; // toggle painting
                if ( !painting )
                {
                    print("=============================\nFinished the frame\n==================================");

                }
            }
        }

    }

    private void Draw()
    {

        //draw our brush here using instantiating

        GameObject instantiated_paint = Instantiate(paint, transform.position, transform.rotation);
        instantiated_paint.transform.GetComponent<MeshRenderer>().material.color = paint_color;

    }
}
