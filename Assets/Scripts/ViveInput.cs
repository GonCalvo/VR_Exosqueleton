using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInput : MonoBehaviour
{
    public GameObject paint;
    public Color paint_color;
    private bool painting = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    public void Update()
    {
        if ( painting )
        {
            Draw();
        }
        
        
        if (SteamVR_Actions.default_DrawBrush.GetState(SteamVR_Input_Sources.Any)) //Check if we are pressing the trigger and invoking the Draw Brush Action
        {
            print("Displaying Brush!");
            painting = !painting; // toggle painting
        }

    }

    private void Draw()
    {

        //draw our brush here using instantiating

        GameObject instantiated_paint = Instantiate(paint, transform.position, transform.rotation);
        instantiated_paint.transform.GetComponent<MeshRenderer>().material.color = paint_color;

    }
}
