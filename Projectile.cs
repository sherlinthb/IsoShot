using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

    public LayerMask collisionMask;
    float speed = 10;
    float damage = 1;
    float skinWidth = .1f;
    public int type = 0;
    //type 0 default bullet
    //type 1 shotgun bullet
    //type 2 rifle bullet
    public float lifeTime = 1.0f;

    public void Start()
    {
        Destroy(gameObject, lifeTime);
        Collider[] initialCollisions = Physics.OverlapSphere(transform.position, .1f, collisionMask);
        //if(initialCollisions.Length > 0)
       // {
       //     OnHitObject(initialCollisions[0], transform.position);
      //  }

        if(type == 1)
        {
            damage = 5;
            speed = 50;
        }
        if(type == 2)
        {
            damage = 10;
            speed = 100;
        }
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
	
	void FixedUpdate () {
        float moveDistance = speed * Time.deltaTime;
        CheckCollisions(moveDistance);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
	}

    void CheckCollisions(float moveDistance)
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, moveDistance + skinWidth, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit.collider, hit.point);
        }
    }


    void OnHitObject(Collider c , Vector3 hitPoint)
    {
        IDamageable damageableObject = c.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeHit(damage, hitPoint, transform.forward);
        }
        GameObject.Destroy(gameObject); 
    }
}
