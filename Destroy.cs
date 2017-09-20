using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour {
    public float lifetime = 1f;
	// Use this for initialization
	void Start () {
        Destroy(gameObject, lifetime);
	}
	

}
