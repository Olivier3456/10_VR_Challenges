using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;


    [SerializeField] private float spawnRate = 1.5f;

    private float speedOfCubes = 1;
    private float scaleOfCubes = 0.3f;


    void Start()
    {
        StartCoroutine(spawnCube());
        StartCoroutine(AddDifficulty());
    }


    IEnumerator AddDifficulty()
    {
        yield return new WaitForSeconds(5);
        speedOfCubes += 0.07f;
        spawnRate -= 0.04f;
        if (scaleOfCubes > 0.1f) scaleOfCubes -= 0.001f;
        StartCoroutine(AddDifficulty());
    }

    private IEnumerator spawnCube()
    {
        yield return new WaitForSeconds(spawnRate);
        GameObject newCube = Instantiate(cubePrefab);
        newCube.GetComponent<Cube>().speed = speedOfCubes;
        newCube.transform.localScale = new Vector3(scaleOfCubes, scaleOfCubes, scaleOfCubes);

        int orientation = Random.Range(0, 4) * 90;
        newCube.transform.Rotate(newCube.transform.forward, orientation);

        StartCoroutine(spawnCube());
    }
}
