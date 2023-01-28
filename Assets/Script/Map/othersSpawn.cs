using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class othersSpawn : MonoBehaviour
{
    public LayerMask isRooms;
    public mapGeneration level;

    private void Update()
    {
        Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, isRooms);
        if(roomDetection == null && level.startSpawn == true)
        {
            int rand = Random.Range(0,level.rooms.Length);
            Instantiate(level.rooms[rand], transform.position, Quaternion.identity);
            Destroy(gameObject);//确保周围房间只生成一次
        }
    }
}
