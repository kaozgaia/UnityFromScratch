using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRotation : MonoBehaviour {

    public Vector3 rotationVector = new Vector3(0f,1f,0f);
    public float speed = 5f;
    public bool local = false;

    private Color randColor;
	// Use this for initialization
	void Start () {
        randColor = new Color(Random.value, Random.value, Random.value, 1.0f);
    }
	
	// Update is called once per frame
	void Update () {
        if(!local) transform.Rotate(rotationVector, speed * Time.deltaTime, Space.World);
        else transform.Rotate(rotationVector, speed * Time.deltaTime, Space.Self);
        Debug.DrawLine(transform.position, transform.position + transform.forward, randColor);
    }
}
