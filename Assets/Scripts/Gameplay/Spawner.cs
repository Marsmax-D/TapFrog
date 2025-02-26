using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> spawnObjects;
    public int direction; 

    private void Start()
    {
        InvokeRepeating(nameof(Spawn), 0.2f, Random.Range(7f, 9f));
    }

    private void Spawn()
    {

        var index = Random.Range(0, spawnObjects.Count);
        var target = Instantiate(spawnObjects[index], transform.position, Quaternion.identity, transform);

        target.GetComponent<MoveForward>().dir = direction;

    }

}
