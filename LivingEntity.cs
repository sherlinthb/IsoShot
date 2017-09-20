using UnityEngine;
using System.Collections;

public class LivingEntity : MonoBehaviour, IDamageable {

    //public float startingHealth;
   // public float health;
    protected bool dead;
    public GameObject impactParticle;

    
    Color originalColor;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        

        //health = startingHealth;
    }

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {

        TakeDamage(damage);


    }

    public virtual void TakeDamage(float damage)
    {

        Instantiate(impactParticle, transform.position, transform.rotation);
    }

    public virtual void GiveHealth(float heal)
    {
       
    }


    public virtual void Die()
    {
        dead = true;
        if(OnDeath != null)
        {
            OnDeath();
        }
        //GameObject.Destroy(gameObject);
    }
}
