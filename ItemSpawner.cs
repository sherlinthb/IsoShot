using UnityEngine;
using System.Collections;


public class ItemSpawner : MonoBehaviour {

    public GameObject[] guns;
    public GameObject[] items;
    public GameObject newItemText;
    public Transform[] spawnPoints;

    float nextSpawnTime;
    float spawnTime;
    int randItemNum;
    int randSpawnNum;
    int itemCount;

    void Start()
    {
       
        nextSpawnTime = Random.Range(2000, 3000);
        spawnTime = 0;
        itemCount = 0;
        ItemScript.ItemCollected += OnItemCollect;
        newItemText.SetActive(false);
    }

    void Update()
    {
  
        randItemNum = Random.Range(0, guns.Length);
        randSpawnNum = Random.Range(0, spawnPoints.Length);
        spawnTime++;
        if(spawnTime > nextSpawnTime)
        {
            spawn();
            spawnTime = 0 ;
        }
    }

    void OnItemCollect()
    {
        itemCount--;
    }

    void newItemInActive()
    {
        newItemText.SetActive(false);
    }

    void spawn()
    {
        if(itemCount < 2)
        {
            Instantiate(guns[randItemNum], spawnPoints[randSpawnNum].transform.position, transform.rotation);
            newItemText.SetActive(true);
            AudioManager.instance.PlaySound2D("Wave Complete");
            Invoke("newItemInActive", 1f);
            itemCount++;

        }
    }
}
