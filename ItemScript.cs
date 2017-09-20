using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {


    public static event System.Action ItemCollected;

    public int type = 0;
    //type 0 - medkit
    //type 1 - shotgun
    //type 2 - rifle
    //type 3 - machine gun 

    public int heal = 12;
    public GameObject particle;
        
	void Start () {

        //player = GetComponent<Player>();
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            AudioManager.instance.PlaySound2D("ItemCollected");
            if(ItemCollected != null)
            {
                ItemCollected();
            }
            IDamageable damageableObject = other.GetComponent<IDamageable>();
            if (damageableObject != null)
            {
                if(type == 0)
                {
                    damageableObject.GiveHealth(heal);
                    Instantiate(particle, transform.position, transform.rotation);

                }
                if(type == 1)
                {
                    Player.EquipTheShotgun();
                }
                if(type == 2)
                {
                    Player.EquipTheRifle();
                }
                if(type == 3)
                {
                    Player.EquipTheMachinegun();
                }
            }
            GameObject.Destroy(gameObject);
        }
    }

    
}
