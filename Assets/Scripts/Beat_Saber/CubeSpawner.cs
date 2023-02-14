using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cubePrefab;

    private float spawnRate = 0.5f;

    private float speedOfCubes = 2;

    [SerializeField] int numberOfCubesInThePool = 30;
    private List<GameObject> disabledCubes = new List<GameObject>();


    void Start()
    {
        InstantiateCubes();

        StartCoroutine(spawnCube());
        StartCoroutine(AddDifficulty());
    }


    IEnumerator AddDifficulty()
    {
        yield return new WaitForSeconds(5);
        speedOfCubes += 0.05f;
        StartCoroutine(AddDifficulty());
    }

    private IEnumerator spawnCube()
    {
        float timeBeforeNextSpawn = Random.Range(spawnRate, spawnRate * 3);
        if (spawnRate >= 0.2f) spawnRate -= 0.001f;
        yield return new WaitForSeconds(timeBeforeNextSpawn);

        if (disabledCubes.Count > 0)
        {
            GameObject cubeToSpawn = disabledCubes[0];
            disabledCubes.RemoveAt(0);
            Cube cubeClassOfCubeToSpawn = cubeToSpawn.GetComponent<Cube>();
            cubeClassOfCubeToSpawn.speed = speedOfCubes;
            cubeClassOfCubeToSpawn.ChoseColor();
            int orientation = Random.Range(0, 4) * 90;
            cubeClassOfCubeToSpawn.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f), 10);
            cubeClassOfCubeToSpawn.isTouched = false;
            cubeClassOfCubeToSpawn.isTouchedFromRightDirection = false;
            cubeClassOfCubeToSpawn.isActive = true;
            cubeClassOfCubeToSpawn.orientation = orientation;
            cubeToSpawn.transform.rotation = Quaternion.identity;
            cubeToSpawn.transform.Rotate(cubeToSpawn.transform.forward, orientation);
            cubeToSpawn.SetActive(true);
        }
        StartCoroutine(spawnCube());
    }


    public void AddCubeToDisabledList(GameObject cubeToAdd)
    {
        disabledCubes.Add(cubeToAdd.gameObject);
    }

    private void InstantiateCubes()
    {
        for (int i = 0; i < numberOfCubesInThePool; i++)
        {
            disabledCubes.Add(Instantiate(cubePrefab));
        }
    }
}
