using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

    public Transform target;
    public float camDist = 10;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(target != null)
        {

            transform.position = target.position - transform.forward * camDist;
            transform.LookAt(target);
        
        }
	}
}
