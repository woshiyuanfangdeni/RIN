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
        instance.transform.parent = transform;//���Ӽ���Ƭ�븸������ֿ�
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
