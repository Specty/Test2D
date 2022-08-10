using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTest : MonoBehaviour
{
    public GameObject prefabToSpawn;
    Quaternion qua = new Quaternion(0, 0, 0, 0);
    // Start is called before the first frame update
    public void SpawnSpots()
    {
        Vector3 pos = new Vector3(10, -10, 0);
        Instantiate(prefabToSpawn, pos, qua);
    }
}
