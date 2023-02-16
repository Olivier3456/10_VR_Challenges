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
    private List<Cube> disabledCubesClassCube = new List<Cube>();
    private List<BoxCollider> disabledCubesColliders = new List<BoxCollider>();


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

            Cube cubeClass = disabledCubesClassCube[0];
            disabledCubesClassCube.RemoveAt(0);

            BoxCollider cubeCollider = disabledCubesColliders[0];
            disabledCubesColliders.RemoveAt(0);

            cubeClass.speed = speedOfCubes;
            cubeClass.ChoseColor();
            int orientation = Random.Range(0, 4) * 90;
            cubeToSpawn.transform.position = new Vector3(Random.Range(-1f, 1f), Random.Range(0.5f, 1.5f), 10);
            cubeClass.isTouched = false;
            cubeClass.isTouchedFromRightDirection = false;
            cubeClass.isActive = true;
            cubeCollider.enabled = true;
            cubeClass.orientation = orientation;
            cubeToSpawn.transform.rotation = Quaternion.identity;
            cubeToSpawn.transform.Rotate(cubeToSpawn.transform.forward, orientation);            
            cubeToSpawn.SetActive(true);
        }
        StartCoroutine(spawnCube());
    }


    public void AddCubeToDisabledList(GameObject cubeToAdd, Cube cubeClass, BoxCollider collider)
    {
        disabledCubes.Add(cubeToAdd);
        disabledCubesClassCube.Add(cubeClass);
        disabledCubesColliders.Add(collider);
    }

    private void InstantiateCubes()
    {
        for (int i = 0; i < numberOfCubesInThePool; i++)
        {
            GameObject newCube = Instantiate(cubePrefab);
            disabledCubes.Add(newCube);
            disabledCubesClassCube.Add(newCube.GetComponent<Cube>());
            disabledCubesColliders.Add(newCube.GetComponent<BoxCollider>());
        }
    }
}
