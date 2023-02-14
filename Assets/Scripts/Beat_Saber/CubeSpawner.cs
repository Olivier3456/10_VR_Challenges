using System.Collections;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;

 //   [SerializeField] private float spawnRate = 1.5f;

    private float speedOfCubes = 2; 


    void Start()
    {
        StartCoroutine(spawnCube());
        StartCoroutine(AddDifficulty());
    }


    IEnumerator AddDifficulty()
    {
        yield return new WaitForSeconds(5);
        speedOfCubes += 0.05f;
     //   spawnRate -= 0.04f;
        //   if (scaleOfCubes > 0.1f) scaleOfCubes -= 0.001f;
        StartCoroutine(AddDifficulty());
    }

    private IEnumerator spawnCube()
    {
        float timeBeforeNextSpawn = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(timeBeforeNextSpawn);
        GameObject newCube = Instantiate(cubePrefab);
        Cube cubeClassOfNewCube = newCube.GetComponent<Cube>();
        cubeClassOfNewCube.speed = speedOfCubes;

        int orientation = Random.Range(0, 4) * 90;
        cubeClassOfNewCube.orientation = orientation;
        newCube.transform.Rotate(newCube.transform.forward, orientation);
        

        StartCoroutine(spawnCube());
    }
}
