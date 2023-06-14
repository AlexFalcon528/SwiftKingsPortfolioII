using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class waveSpawner : MonoBehaviour
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
        
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    } 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isSpawning && numberSpawned < numberToSpawn && gameManager.instance.nextWave && playerInRange) //Code for wave-based survival mode
        {
            if (numberSpawned == 0 && gameManager.instance.nextWave)
            {
                gameManager.instance.UpdateGameGoal(numberToSpawn/* * (StatManagerVariables.instance.difficulty / 2)*/); //Is this supposed to change how many enemies spawn? If so, it's not correct
            }
            StartCoroutine(spawn());

        }
        if(numberSpawned == numberToSpawn)
        {
            gameManager.instance.nextWave = false;
            numberSpawned = 0;
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
