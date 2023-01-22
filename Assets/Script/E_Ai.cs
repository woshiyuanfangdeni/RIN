using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class E_Ai : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb_e;
    public Rigidbody2D player0;
    public bool attack = false;
    float speed1 = 1.5f;
    private float tick1 = 2f;
    int temp = 1;
    bool beattack=false;

    void Start()
    {
        rb_e = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb_e.velocity += Vector2.down * 9.8f * Time.deltaTime;
        if (attack)
        {
            rb_e.velocity = new Vector2((new Vector2((player0.position.x - rb_e.position.x), 0).normalized.x) * speed1, rb_e.velocity.y);
            Vector3 enemy_scale = rb_e.transform.localScale;
            float ttemp = player0.position.x - rb_e.position.x;
            if (ttemp > 0)
            {
                enemy_scale.x = Mathf.Abs(enemy_scale.x);
                temp = 1;
            }
            if (ttemp < 0)
            {
                enemy_scale.x = Mathf.Abs(enemy_scale.x) * -1;
                temp = 0;
            }
            rb_e.transform.localScale = enemy_scale;
            tick1 = 2f;
        }
        if (!attack)
        {
            if (tick1 >= 0)
            {
                tick1 -= Time.deltaTime;
                if (temp == 1)
                {
                    rb_e.velocity = new Vector2(1f, rb_e.velocity.y);
                }
                if (temp == 0)
                    rb_e.velocity = new Vector2(-1f, rb_e.velocity.y);

            }
            else

            {
                tick1 = 2f;
                temp = (temp + 1) % 2;
                Vector3 enemyscale = rb_e.transform.localScale;
                if (temp == 1)
                {
                    enemyscale = new Vector3(Mathf.Abs(enemyscale.x), enemyscale.y, enemyscale.z);
                }
                if (temp == 0)
                {
                    enemyscale = new Vector3(Mathf.Abs(enemyscale.x) * -1, enemyscale.y, enemyscale.z);
                }
                rb_e.transform.localScale = enemyscale;
            }
        }
<<<<<<< HEAD
=======



        }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player") 
        {
           player0 = collision.GetComponent<Rigidbody2D>();
            attack= true;
        }
>>>>>>> dc0dcd2115d320f57302634a08ba3cff7d1a7f70
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
<<<<<<< HEAD
        if (collision.gameObject.tag == "Hitbox")
=======
        if (collision.tag == "Player")
>>>>>>> dc0dcd2115d320f57302634a08ba3cff7d1a7f70
        {

            if (collision.transform.position.x > this.transform.position.x)
            {
                rb_e.AddForce(new Vector2(-100, 5)*20, ForceMode2D.Force);
                Debug.Log("1");
            }
            else
            {
                rb_e.AddForce(new Vector2(100, 5)*20, ForceMode2D.Force);
                Debug.Log("2");
            }
        }
        void Attack()
        {

        }
    }
}

