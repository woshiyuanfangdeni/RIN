using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGeneration : MonoBehaviour
{
    public Transform[] startPos;//构建坐标数组
    public GameObject[] rooms;//构建房间数组

    private int direction;
    public float moveAmount;

    public float startTime = 0.3f;
    private float beginBetweenTime;//房间生成时间

    public float minX;
    public float maxX;
    public float minY;
    public LayerMask room;
    public bool startSpawn = false;//用于地图生成与否
    private int downAmount = 0;
    void Start()
    {
        int startRandPos = Random.Range(0, startPos.Length);//初始位置的随机化
        transform.position = startPos[startRandPos].position;
        Instantiate(rooms[0],transform.position,Quaternion.identity);//生成目标

        direction = Random.Range(1, 6);
    }
    private void Update()
    {
        if (beginBetweenTime <= 0&&startSpawn == false)
        {
            Move();
            beginBetweenTime = startTime;//初始化时间
        }else
        {
            beginBetweenTime -= Time.deltaTime;
        }
    }

    void Move()
    {
        if(direction == 1||direction == 2)
        {
            if(transform.position.x < maxX)//自身位置在左边就往右走
            {
                downAmount = 0;

                Vector2 nowPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = nowPos;

                int rand = Random.Range(0,rooms.Length);
                Instantiate(rooms[rand],transform.position,Quaternion.identity);

                direction = Random.Range(1, 6);
                if (direction == 3)
                {
                    direction = 1;
                }
                else if (direction == 4)
                {
                    direction = 2;
                }//时往右走的房间不会再往左生成
            }
            else { direction = 5; }

            
            
        }else if(direction == 3||direction == 4)//自身位置在右边就往左走
        {
            if(transform.position.x > minX)
            {
                downAmount= 0;

                Vector2 nowPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = nowPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);//往左的房间不会再往右生成
            }
            else { direction = 5; }

            
            
        }else if(direction == 5)
        {
            downAmount++;

            if(transform.position.y> minY)//自身位置在上边才能往下走
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);
                if(roomDetection.GetComponent<roomType>().type!=1&&
                    roomDetection.GetComponent<roomType>().type != 3)
                {
                    if(downAmount >= 2)
                    {
                        roomDetection.GetComponent<roomType>().roomDestroy();
                        Instantiate(rooms[3],transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<roomType>().roomDestroy();

                        int randBottonRoom = Random.Range(1, 4);
                        if (randBottonRoom == 2)
                        {
                            randBottonRoom = 1;
                        }
                        Instantiate(rooms[randBottonRoom], transform.position, Quaternion.identity);
                    }

                }

                Vector2 nowPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = nowPos;

                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand],transform.position, Quaternion.identity);

                direction= Random.Range(1, 6);
            }
            else
            {
                startSpawn = true;
            }
            
        }

    }
}
