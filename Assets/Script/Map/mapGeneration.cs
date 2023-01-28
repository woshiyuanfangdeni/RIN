using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapGeneration : MonoBehaviour
{
    public Transform[] startPos;//������������
    public GameObject[] rooms;//������������

    private int direction;
    public float moveAmount;

    public float startTime = 0.3f;
    private float beginBetweenTime;//��������ʱ��

    public float minX;
    public float maxX;
    public float minY;
    public LayerMask room;
    public bool startSpawn = false;//���ڵ�ͼ�������
    private int downAmount = 0;
    void Start()
    {
        int startRandPos = Random.Range(0, startPos.Length);//��ʼλ�õ������
        transform.position = startPos[startRandPos].position;
        Instantiate(rooms[0],transform.position,Quaternion.identity);//����Ŀ��

        direction = Random.Range(1, 6);
    }
    private void Update()
    {
        if (beginBetweenTime <= 0&&startSpawn == false)
        {
            Move();
            beginBetweenTime = startTime;//��ʼ��ʱ��
        }else
        {
            beginBetweenTime -= Time.deltaTime;
        }
    }

    void Move()
    {
        if(direction == 1||direction == 2)
        {
            if(transform.position.x < maxX)//����λ������߾�������
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
                }//ʱ�����ߵķ��䲻������������
            }
            else { direction = 5; }

            
            
        }else if(direction == 3||direction == 4)//����λ�����ұ߾�������
        {
            if(transform.position.x > minX)
            {
                downAmount= 0;

                Vector2 nowPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = nowPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6);//����ķ��䲻������������
            }
            else { direction = 5; }

            
            
        }else if(direction == 5)
        {
            downAmount++;

            if(transform.position.y> minY)//����λ�����ϱ߲���������
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
