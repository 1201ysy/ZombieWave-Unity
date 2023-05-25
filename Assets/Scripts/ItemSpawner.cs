using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] items;
    [SerializeField] private Transform[] spawnPoints;

    [SerializeField] private float spawnInterval = 20f;
    // Start is called before the first frame update
    void Start()
    {
        StartItemSpawning();
    }


    private void StartItemSpawning()
    {
        StartCoroutine("SpawnItemRoutine");
    }

    public void StopItemSpawning()
    {
        StopCoroutine("SpawnItemRoutine");
    }

    IEnumerator SpawnItemRoutine()
    {
        yield return new WaitForSeconds(spawnInterval);
        while (true)
        {
            for (int j = 0; j < 3; j++)
            {
                SpawnItem();
            }
            yield return new WaitForSeconds(spawnInterval);
        }


    }

    private void SpawnItem()
    {
        int spawnPointIndex = Random.Range(0, spawnPoints.Length);
        int itemIndex = Random.Range(0, items.Length);


        for (int i = 0; i < 100; i++)
        {

            if (isObjectHere(spawnPoints[spawnPointIndex].position))
            {
                spawnPointIndex = Random.Range(0, spawnPoints.Length);
            }
            else
            {
                Instantiate(items[itemIndex], spawnPoints[spawnPointIndex].position, Quaternion.identity);
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
            if (tag == "Item")
            {
                return true;
            }
        }

        return false;
    }

}
