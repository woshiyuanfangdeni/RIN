using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomSpawn : MonoBehaviour
{
    public GameObject[] obejects;
    void Start()
    {
        int rand = Random.Range(0,obejects.Length);
        GameObject instance = Instantiate(obejects[rand],transform.position,Quaternion.identity);
        instance.transform.parent = transform;//将子集瓦片与父级房间分开
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
