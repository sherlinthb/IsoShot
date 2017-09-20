using UnityEngine;
using System.Collections;

public class spawner : MonoBehaviour {

    public Wave[] waves;
    public Enemy[] enemy;
    public Transform[] spawnPoints;


    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    void Start()
    {
        NextWave();
    }
    void Update()
    {
        int location0 = Random.Range(0, spawnPoints.Length);
        int location1 = Random.Range(0, spawnPoints.Length);
        int location2 = Random.Range(0, spawnPoints.Length);


       //- print("enemiesRemainingAlive " + enemiesRemainingAlive);
        if(enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.timeBetweenSpawns;

            if(currentWave.enemyCount0 > 0)
            {
               Enemy spawnedEnemy = Instantiate(enemy[0], spawnPoints[location0].transform.position, Quaternion.identity) as Enemy;
                currentWave.enemyCount0--;
                spawnedEnemy.OnDeath += OnEnemyDeath;
            }

            if (currentWave.enemyCount1 > 0)
            {
                Enemy spawnedEnemy1 = Instantiate(enemy[1], spawnPoints[location1].transform.position, Quaternion.identity) as Enemy;
                currentWave.enemyCount1--;
                spawnedEnemy1.OnDeath += OnEnemyDeath;
            }

            if (currentWave.enemyCount2 > 0)
            {
                Enemy spawnedEnemy2 = Instantiate(enemy[2], spawnPoints[location2].transform.position, Quaternion.identity) as Enemy;
                currentWave.enemyCount2--;
                spawnedEnemy2.OnDeath += OnEnemyDeath;
            }
            if (currentWave.enemyCount3 > 0)
            {
                Enemy spawnedEnemy3 = Instantiate(enemy[3], spawnPoints[location2].transform.position, Quaternion.identity) as Enemy;
                currentWave.enemyCount3--;
                spawnedEnemy3.OnDeath += OnEnemyDeath;
            }
        }


    }

    void OnEnemyDeath()
    {
        enemiesRemainingAlive--;

        if(enemiesRemainingAlive == 0)
        {
            NextWave();
        }
    }

    void NextWave()
    {
        if(currentWaveNumber > 0)
        {
            AudioManager.instance.PlaySound2D("Wave Complete");
        }
        currentWaveNumber++;
        if(currentWaveNumber-1 < waves.Length)
        {
            currentWave = waves[currentWaveNumber - 1];

            enemiesRemainingToSpawn = currentWave.enemyCount0 + currentWave.enemyCount1 + currentWave.enemyCount2 + currentWave.enemyCount3;
            enemiesRemainingAlive = enemiesRemainingToSpawn;

         

        }
    }

    [System.Serializable]
    public class Wave
    {
        public int enemyCount0;
        public int enemyCount1;
        public int enemyCount2;
        public int enemyCount3;
        public float timeBetweenSpawns;

    }
}
