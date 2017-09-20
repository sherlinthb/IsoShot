using UnityEngine;
using System.Collections;

public class HurtPlayer : MonoBehaviour{

    public LayerMask collisionMask;
    float speed = 5f;
    float damage = 1f;
    void FixedUpdate()
    {
        float moveDistance = speed * Time.deltaTime;
        checkCollisions();
    }   
    void checkCollisions()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        if (Physics.Raycast(ray, out hit, 0, collisionMask, QueryTriggerInteraction.Collide))
        {
            OnHitObject(hit);
        }
    }

    void OnHitObject(RaycastHit hit)
    {
        IDamageable damageableObject = hit.collider.GetComponent<IDamageable>();
        if (damageableObject != null)
        {
            damageableObject.TakeDamage(damage);
        }
        //GameObject.Destroy(gameObject);
    }

    
}
