using UnityEngine;
using System.Collections;

public class ShockWaveScript : MonoBehaviour {

    public float scale = 1f;
    public int damage = 5;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.localScale += new Vector3(scale, scale, scale);
	}
    
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag =="Enemy")
        {
            IDamageable damageableObject = other.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                damageableObject.TakeDamage(5);
            }
        }
    }

}
