using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;

   
    [SerializeField] private float spawnRate = 1;

   
    void Start()
    {
        StartCoroutine(spawnCube());
    }

    private IEnumerator spawnCube()
    {
        yield return new WaitForSeconds(spawnRate);
        Instantiate(cubePrefab);
        StartCoroutine(spawnCube());
    }
}
