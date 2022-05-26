using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour
{
    public GameObject paint;
    public Color paint_color;
    public Color corner_color;
    public GameObject cam;
    private bool painting = false;
    // Start is called before the first frame update

    private Vector3 drawing_center = Vector3.zero;
    private List<Vector3> drawing;
    private List<Vector3> corners;

    private Quaternion cam_original_rot;
    private Vector3 cam_original_fwd;
    private Vector3 cam_original_up;
    private Vector3 cam_original_right;
    void Start()
    {
        drawing = new List<Vector3>();
        corners = new List<Vector3>();
    }

    // Update is called once per frame
    public void Update()
    {
        if ( painting )
        {
            Draw();
            Vector3 relative_pos = cam.transform.position - transform.position;
            float cam_coordinates_fwd = Vector3.Dot(relative_pos, cam_original_fwd);
            float cam_coordinates_up = Vector3.Dot(relative_pos, cam_original_up);
            float cam_coordinates_right = Vector3.Dot(relative_pos, cam_original_right);
            drawing.Add(new Vector3(cam_coordinates_right, cam_coordinates_up, cam_coordinates_fwd));
        }
        
        // button was released
        if (SteamVR_Actions.default_DrawBrush.GetChanged(SteamVR_Input_Sources.Any) && !SteamVR_Actions.default_DrawBrush.GetState(SteamVR_Input_Sources.Any))
        {
            if (drawing_center == Vector3.zero)
            {
                drawing_center = cam.transform.position - transform.position;
                cam_original_rot = cam.transform.rotation;
                cam_original_fwd = cam_original_rot * Vector3.forward;
                cam_original_up = cam_original_rot * Vector3.up;
                cam_original_right = cam_original_rot * Vector3.right;
            }
            else if (corners.Count < 4)
            {
                painting = true;
                corners.Add(transform.position);
            }
            else
            {
                painting = false; 
                print("=============================\nFinished the frame\n==================================");
                // Calculate the frame max and mins of the plane normal to fwd of the cam
                float max_right = 0;
                float max_left = 0;
                float max_up = 0;
                float max_down = 0;
                foreach (Vector3 point in drawing)
                {
                    if (max_right < point.x) max_right = point.x;
                    if (max_left > point.x) max_left = point.x;
                    if (max_up < point.y) max_up = point.y;
                    if (max_down < point.y) max_right = point.y;
                }
                print(".\n\tMax_right: " + max_right + "\n\tMax_left: " + max_left + "\n\tMax_up: " + max_up + "\n\tMax_down: " + max_down);

                foreach (Vector3 corner in corners)
                {
                    GameObject instantiated_corner = Instantiate(paint, corner, transform.rotation);
                    instantiated_corner.transform.GetComponent<MeshRenderer>().material.color = corner_color;
                    instantiated_corner.transform.localScale = new Vector3(.3f, .3f, .3f);
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
