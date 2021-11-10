using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    void target_reached() {
        this.GetComponent<Renderer>().material.SetColor("_Color", Color.red);

    }
}
