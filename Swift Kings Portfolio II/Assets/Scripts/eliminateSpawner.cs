using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eliminateSpawner : MonoBehaviour
{


    [SerializeField] GameObject[] objectToSpawn;
    [SerializeField] Transform[] spawnPositions;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] int numberToSpawn;

    int numberSpawned;
    bool playerInRange;
    bool isSpawning;


    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.UpdateGameGoal(numberToSpawn); //update goal for non-wave
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && !isSpawning && numberSpawned < numberToSpawn) //Code for non-wave gamemode
        {
            StartCoroutine(spawn());
        }
        if (numberSpawned == numberToSpawn)
        {
            gameObject.SetActive(false);
        }
        
    }


    IEnumerator spawn()
    {
        isSpawning = true;

        Instantiate(objectToSpawn[Random.Range(0, objectToSpawn.Length)], spawnPositions[Random.Range(0, spawnPositions.Length)].position, transform.rotation);
        numberSpawned++;

        yield return new WaitForSeconds(timeBetweenSpawns);

        isSpawning = false;
    }
}
