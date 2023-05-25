using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private Enemy[] enemies;
    [SerializeField] private Enemy boss;
    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private float spawnInterval = 10f;


    // Start is called before the first frame update
    void Start()
    {
        StartEnemySpawning();
    }


    private void StartEnemySpawning()
    {
        StartCoroutine("SpawnEnemyRoutine");
    }

    public void StopEnemySpawning()
    {
        StopCoroutine("SpawnEnemyRoutine");
    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < 2; i++)
        {
            // Number of Waves
            for (int j = 0; j < 7; j++)
            {
                // Number of enemies per wave
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
        SpawnBoss();

        for (int i = 0; i < 100; i++)
        {
            // Number of Waves
            for (int j = 0; j < 3; j++)
            {
                // Number of enemies per wave
                SpawnEnemy();
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnEnemy()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int enemyIndex = Random.Range(0, enemies.Length);


        for (int i = 0; i < 100; i++)
        {

            if (isObjectHere(spawnPoints[spawnPointIndex].position))
            {
                spawnPointIndex = Random.Range(0, spawnPoints.Length);
            }
            else
            {
                Instantiate(enemies[enemyIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
                break;
            }
        }
    }

    private void SpawnBoss()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        for (int i = 0; i < 100; i++)
        {

            if (isObjectHere(spawnPoints[spawnPointIndex].position))
            {
                spawnPointIndex = Random.Range(0, spawnPoints.Length);
            }
            else
            {
                Instantiate(boss, spawnPoints[spawnPointIndex].position, Quaternion.identity);
                break;
            }
        }
    }

    private bool isObjectHere(Vector3 position)
    {

        RaycastHit2D[] hits = Physics2D.RaycastAll(position, new Vector3(0, 0, 1f));
        foreach (RaycastHit2D hit in hits)
        {
            string tag = hit.collider.gameObject.tag;
            //Debug.Log(tag);
            if (tag == "Enemy")
            {
                return true;
            }
        }

        return false;
    }



}
