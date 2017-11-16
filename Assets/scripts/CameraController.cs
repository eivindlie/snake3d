using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public SnakeController player;
    float offset;

	// Use this for initialization
	void Start () {
        offset = (transform.position - player.segments[0].transform.position).magnitude;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        transform.position = player.segments[0].transform.position - player.segments[0].transform.forward * offset;
        transform.rotation = Quaternion.LookRotation(player.segments[0].transform.position - transform.position);
	}
}
