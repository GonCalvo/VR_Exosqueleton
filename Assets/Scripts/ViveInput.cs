using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Valve.VR;
using TMPro;

public class ViveInput : MonoBehaviour
{
    public GameObject paint;
    public Color corner_color;
    public GameObject cam;

    public GameObject normal_cam_display;
    public TextMeshProUGUI textMeshProUGUI;
    void Start()
    {
        if (DataHandler.Instance.Session.range_fwd != -1)
        {
            normal_cam_display.SetActive(true);
        }
    }

    // Update is called once per frame
    public void Update()
    {
        // button was released
        if (SteamVR_Actions.default_DrawBrush.GetChanged(SteamVR_Input_Sources.Any) && !SteamVR_Actions.default_DrawBrush.GetState(SteamVR_Input_Sources.Any))
        {
            if ( textMeshProUGUI.text == string.Empty)
            {
                textMeshProUGUI.text = "Centro";
            }
            else if ( DataHandler.Instance.Session.range_fwd == -1)
            {
                DataHandler.Instance.Session.range_fwd = Vector3.Distance(cam.transform.position, transform.position);
                textMeshProUGUI.text = "Abajo";
            }
            else if (DataHandler.Instance.Session.range_bottom == -1)
            {
                DataHandler.Instance.Session.range_bottom = Vector3.Distance(cam.transform.position, transform.position);
                textMeshProUGUI.text = "Arriba";
            }
            else if (DataHandler.Instance.Session.range_top == -1)
            {
                DataHandler.Instance.Session.range_top = Vector3.Distance(cam.transform.position, transform.position);
                textMeshProUGUI.text = "Izquierda";
            }
            else if (DataHandler.Instance.Session.range_left == -1)
            {
                DataHandler.Instance.Session.range_left = Vector3.Distance(cam.transform.position, transform.position);
                textMeshProUGUI.text = "Derecha";
            }
            else if (DataHandler.Instance.Session.range_right == -1)
            {
                DataHandler.Instance.Session.range_right = Vector3.Distance(cam.transform.position, transform.position);
                textMeshProUGUI.text = "Pulsa una última vez!";
            }
            else if ( !normal_cam_display.activeInHierarchy )
            {
                Debug.Log("Revealing the usual display");
                normal_cam_display.SetActive(true);

                DataHandler.SQLmanager.insertSession(DataHandler.Instance.Session);
                Debug.Log("SessionID: " + DataHandler.Instance.Session.id);
                return;
            }
            Draw();

        }

    }

    private void Draw()
    {

        //draw our brush here using instantiating

        GameObject instantiated_paint = Instantiate(paint, transform.position, transform.rotation);
        instantiated_paint.transform.GetComponent<MeshRenderer>().material.color = corner_color;
    }
}
