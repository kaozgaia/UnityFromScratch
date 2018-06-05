using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField]
    private float moveSpeed = 5;

    private Rigidbody _body;
    private Camera _camera;
    private Vector3 velocity;

	// Use this for initialization
	void Start () {
        _body = GetComponent<Rigidbody>();
        _camera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mousePos = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.transform.position.y));
        transform.LookAt(mousePos + Vector3.up * transform.position.y);
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed;
    }

    void FixedUpdate()
    {
        _body.MovePosition(_body.position + velocity * Time.deltaTime);
    }
}
