using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;
    public Rigidbody2D rb;
    public float speed = 5f;
    int n;
    bool jumpable = true;
    public float h = 5;
    public float g = 10f;
    float v;
    Vector2 inputpos;
    public static Vector2 transfor;
    Vector3 Playertran;
    bool isdash;
    float dashtime=0.2f;
    float dashtimeleft,dashCD=1f,dashLast=-10f;
    float dashspeed=40f;
    int Dashdirection;
    float Force = 10f;
    bool Wantdash = false;

    private void Awake()
    {
        player = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;
        v = Mathf.Sqrt(2 * h / g);
        rb = gameObject.GetComponent<Rigidbody2D>();
        inputpos = new Vector2();
        Playertran = rb.transform.localScale;
    }


    void Update()
    {

        transfor = this.gameObject.transform.position;
        if(!isdash)//玩家行动在这里面写0.0
        {
            if (Input.GetKeyDown(KeyCode.Space))
            Jump();
            MoveObject();
            rb.velocity += Vector2.down * g * Time.deltaTime;          
        }
        if (Input.GetKeyDown(KeyCode.C))
            if (Time.time >= (dashLast + dashCD))
                Wantdash =true;
        if(Wantdash)
            Dash();
    }
    public void MoveObject()
    {
        inputpos = rb.velocity;
        inputpos.x = Input.GetAxisRaw("Horizontal") * speed;
        rb.velocity = inputpos;
        if (inputpos.x < 0)
        {
            Playertran.x = -Mathf.Abs(Playertran.x);
            Dashdirection = -1;
        }
        if (inputpos.x > 0)
        {
            Playertran.x = Mathf.Abs(Playertran.x);
            Dashdirection = 1;
        }
        rb.transform.localScale = Playertran;
    }
    public void Jump()
    {
        if (jumpable)
        {
            n = 2;
        }
        if (n > 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 6);
            n--;
        };
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            jumpable = true;
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" )
        {
            jumpable = false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        int direction_e_p = 0;
        if (collision.gameObject.transform.position.x - this.gameObject.transform.position.x >= 0)
            direction_e_p = -1;
        if (collision.gameObject.transform.position.x - this.gameObject.transform.position.x < 0)
            direction_e_p = 1;
        if (collision.gameObject.tag == "enemy")
        {
            rb.AddForce(new Vector2(direction_e_p * Force * 250, 200f), ForceMode2D.Force);
        }
    }
    void Dash()
    {
        {
            if (Time.time >= (dashLast + dashCD))
            {
                Dashready();
            }
            Rush();
            
        }
        if (isdash)
        {
            dashtimeleft -= Time.deltaTime * 2;
            if (dashtimeleft < 0)
            {
                isdash = false;
                Wantdash = false;
            }
        }
    }
    void Rush()
    {
        if (isdash)
            if (dashtimeleft >= 0)
            //transform.Translate(transform.right * Time.deltaTime * dashtime*Dashdirection*dashspeed);
            {
                rb.velocity = new Vector2(dashspeed * Dashdirection, 0);
            }           
    }
    void Dashready()
    {
        isdash = true;
        dashtimeleft = dashtime;
        dashLast = Time.time;
    }
    void Attack()
    {

    }
}
