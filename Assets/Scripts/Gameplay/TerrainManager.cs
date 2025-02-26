using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    public float offsetY;
    public List<GameObject> TerrainObjects;
    private GameObject spawnObject;

    private int lastIndex;

    //private void Start()
    //{
    //    CheckPosition();
    //}


    private void OnEnable()
    {
        EventHandler.GetPointEvent += OnGetPointEvent;

    }

    private void OnDisable()
    {
        EventHandler.GetPointEvent -= OnGetPointEvent;

    }

    private void OnGetPointEvent(int obj)
    {
        CheckPosition();
    }

    public void CheckPosition()
    {
        if (transform.position.y - Camera.main.transform.position.y < offsetY / 2)
        {
            transform.position = new Vector3(0, Camera.main.transform.position.y + offsetY, 0);
            SpawnTerrain();
        }
    }

    private void SpawnTerrain()
    {
        var randomIndex = Random.Range(0, TerrainObjects.Count);
        while (lastIndex == randomIndex)    //避免生成连续相同的场景
        {
            randomIndex = Random.Range(0, TerrainObjects.Count);
        }
        lastIndex = randomIndex;
        spawnObject = TerrainObjects[randomIndex];

        Instantiate(spawnObject, transform.position, Quaternion.identity);
    }

}
