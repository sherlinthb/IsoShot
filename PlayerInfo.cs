using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class PlayerInfo : MonoBehaviour {

    public static PlayerInfo info;
   
    Text text;

    public float playerHealth = 0;


    // Use this for initialization
     void Start() {
     
        if (info == null)
        {
            DontDestroyOnLoad(gameObject);
            info = this;
            text = GetComponent<Text>();
        }else if(info != this)
        {
            Destroy(gameObject);
        }
	}

   
	
	// Update is called once per frame
	void Update () {
        
        playerHealth = Player.health;
        text.text = "Health: " + playerHealth + "/ " + Player.startingHealth;
	}
}
