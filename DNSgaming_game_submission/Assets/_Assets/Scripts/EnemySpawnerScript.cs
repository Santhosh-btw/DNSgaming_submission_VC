using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float unitHorizontalRange;
    [SerializeField] private float unitVerticalRange;

    [Header("Rovers")]
    [SerializeField] private GameObject[] rovers;

    private float timeBetweenWaves = 6f;
    public int enemyCount;
    private ObjectPooler enemyPool;
    bool waveIsDone = true;


    private void Start() {
        enemyPool = GameObject.FindGameObjectWithTag("EnemyPoolManagerTag").GetComponent<ObjectPooler>();
        
        SpawnEnemy();
    }

    void Update(){
        if(waveIsDone == true){
            StartCoroutine(WaveSpawner());
        }
    }

    IEnumerator WaveSpawner(){
        waveIsDone = false;

        for (int i = 0; i < enemyCount; i++){
           SpawnEnemy();
           yield return new WaitForSeconds(spawnRate) ;
        }

        spawnRate -= 0.1f;
        enemyCount += 3;

        yield return new WaitForSeconds(timeBetweenWaves);

        waveIsDone = true;
    }

    private void SpawnEnemy(){
        GameObject currEnemy = enemyPool.GetGameObject(transform);
        
        float xPos = UnityEngine.Random.Range(-unitHorizontalRange, unitHorizontalRange);
        float zPos = UnityEngine.Random.Range(-unitVerticalRange, unitVerticalRange);

        Vector3 offset = new Vector3(xPos, transform.position.y + 0.05f, zPos);
        Vector3 newPos = currEnemy.transform.position + offset;

        currEnemy.transform.position = newPos;
    }
}
