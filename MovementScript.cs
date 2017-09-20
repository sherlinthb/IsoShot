using UnityEngine;
using System.Collections;

public class MovementScript : MonoBehaviour {

    public int speed = 1;
    public int type = 0;
    //type 0 = rotate z axis
    //type 1 = rotate y axis

    //type 2 = move up

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(type == 0)
        {
            transform.Rotate(0, 0, speed);
        }
        if(type == 1)
        {
            transform.Rotate(0, speed, 0);
        }
        if(type == 2)
        {
            transform.Translate(Vector3.up * speed , Space.World);
        }
	}
}
