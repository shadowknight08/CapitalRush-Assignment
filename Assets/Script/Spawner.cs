using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // The prefab of the cop
    public GameObject copPrefab;

    // The size of the room
    public float roomSize = 20f;

    // The minimum required space between each cop
    public float minSpace = 1f;

    void Start()
    {
        // The radius around the player within which cops should not spawn
        float playerRadius = 2f;

        // Instantiate 10 cops
        for (int i = 0; i < 10; i++)
        {
            // Generate a random position within the room
            Vector3 randomPos = new Vector3(Random.Range(-roomSize / 2, roomSize / 2), 0, Random.Range(-roomSize / 2, roomSize / 2));

            // Check if the generated position is too close to any other cop or to the player
            bool tooClose = false;
            foreach (GameObject cop in GameObject.FindGameObjectsWithTag("Cops"))
            {
                if (Vector3.Distance(randomPos, cop.transform.position) < minSpace)
                {
                    tooClose = true;
                    break;
                }
            }
            if (Vector3.Distance(randomPos, Vector3.zero) < playerRadius)
            {
                tooClose = true;
            }

            // If the generated position is not too close to any other cop or to the player, instantiate the cop at that position
            if (!tooClose)
            {
               GameObject gm =  Instantiate(copPrefab, randomPos, Quaternion.identity);
                gm.name = "cube" + i;
            }
            // If the generated position is too close, try again with a new random position
            else
            {
                i--;
            }
        }
    }
}