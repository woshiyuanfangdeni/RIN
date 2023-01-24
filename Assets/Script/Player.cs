using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player player;
    Rigidbody2D rb;
    public float speed = 5f;
    public float h = 5;
    public float g = 10f;
    float v;
    Vector2 inputpos;
    public static Vector2 transfor;
    Vector3 Playertran;
    bool isdash = false;//是否冲刺
    float dashtime = 0.2f;
    float dashtimeleft, dashCD = 1f, dashLast = -10f;
    float dashspeed = 40f;
    int Dashdirection;
    float Force = 10f;
    bool Wantdash = false;
    bool isattack=false;
    //以下为跳跃检测
    [Range(1, 10)]
    private float jumpSpeed = 8f;
    private bool moveJump;//判断是否按下跳跃
    private bool isGround;//判断是否在地面上
    public Transform groundCheck;//地面检测
    public LayerMask ground;
    //以下为跳跃优化
    public float fallMultiplier = 1000f;//大跳的重力
    public float lowJumpMultiplier = 600f;//小跳的重力
    public float fallMultiplierElse = 3f;//重力补足
    //以下为多段跳功能的实现
    public int jumpCount = 2;//跳跃次数
    private bool isJump;//表示跳跃状态
    //以下为远程敌人攻击到玩家时玩家受击后撤的功能实现
    public Transform player_0;//获取玩家的位置组件
    public Transform farAttackEnemy;//获取敌人的位置组件

    public Animator anim;//静态解决方案
    public GameObject N_hitbox_1, N_hitbox_2, N_hitbox_3;
    private void Awake()
    {
        player = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        v = Mathf.Sqrt(2 * h / g);
        rb = gameObject.GetComponent<Rigidbody2D>();
        inputpos = new Vector2();
        Playertran = rb.transform.localScale;
    }
    void Update()
    {
        transfor = this.gameObject.transform.position;
        if (!isdash)//玩家行动在这里面写0.0
        {
            PlayerJumpByTwice();
            MoveObject();
            if (Input.GetKeyDown(KeyCode.J))//攻击
            {
                isattack = true;
                anim.SetBool("PrepareAttack", true);
                Debug.Log("succeesful attack");
            } 
            //正在攻击
            NormalAttack();
            Debug.Log(isattack);
        }
        if (Input.GetKeyDown(KeyCode.L))
            if (Time.time >= (dashLast + dashCD))
                Wantdash = true;
        if (Wantdash)
            Dash();
    }
    private void FixedUpdate()//固定为每秒50次检测的固定补足更新
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);//地面检测
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
                this.gameObject.layer = 6;
            }
        }
    }
    void Rush()
    {
        if (isdash)
            if (dashtimeleft >= 0)
            //transform.Translate(transform.right * Time.deltaTime * dashtime*Dashdirection*dashspeed);
            {
                this.gameObject.layer = 9;
                rb.velocity = new Vector2(dashspeed * Dashdirection, 0);
            }
    }
    void Dashready()
    {
        isdash = true;
        dashtimeleft = dashtime;
        dashLast = Time.time;
    }
    /*void NormalAttack()
    {
        anim.SetBool("isAttack",true);
        Hitbox_use(N_hitbox_1);
 
        if (Input.GetKey(KeyCode.J) && anim.GetBool("isAttack"))
        {
            anim.SetBool("isAttack_1", true);
            Hitbox_use(N_hitbox_2);
        
            if (Input.GetKey(KeyCode.J) && anim.GetBool("isAttack_1"))
            {
                anim.SetBool("isAttack_2", true );
                Hitbox_use(N_hitbox_3);
                anim.SetBool("isAttack_2", false);
              
            }
            anim.SetBool("isAttack_1", false);
        }      
        anim.SetBool("isAttack", false);
    }*/
    void PlayerJumpByTwice()//二段跳
    {
        moveJump = Input.GetButtonDown("Jump");
        JumpDetectionByTwice();
        //我是分界线，以下为优化跳跃手感内容
        if (Input.GetButtonDown("Jump") && rb.velocity.y < 0 && jumpCount > 0)
        {
            rb.velocity = Vector2.up * 7;
        }
        else if (rb.velocity.y < 0 && !Input.GetButtonDown("Jump"))
        {
            if (jumpCount > 0) rb.velocity += Vector2.up * Physics2D.gravity.y * fallMultiplier * Time.deltaTime;
            if (jumpCount == 0) rb.gravityScale = fallMultiplier;
        }
        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            rb.gravityScale = lowJumpMultiplier;
        }
        else
        {
            rb.gravityScale = 1.5f;
        }
    }
    void JumpDetectionByTwice()//二段跳检测
    {
        if (moveJump && jumpCount > 0)
        {
            isJump = true;
        }
        if (isGround)//判断是否在地面
        {
            jumpCount = (int)2f;//四舍五入为2
        }
        if (isJump)
        {
            jumpCount--;//这里有点问题，第一次跳跃时无法检测到跳跃，二段跳时才能检测到，如果要使用二段跳动画的话直接在这里加上调用二段跳动画就可以了
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumpCount--;
            isJump = false;
        }
    }
    void Hitbox_use(GameObject hitbox)
    {
        hitbox.SetActive(true);
    }
    void Hitbox_clear(GameObject hitbox)
    {
        hitbox.SetActive(false);
    }
    void NormalAttack()//攻击
    {
        switch (anim.GetFloat("ATTACK"))
        {
            case 1:
                {
                    Hitbox_clear(N_hitbox_3);
                    Hitbox_use(N_hitbox_1);
                    break;
                }
            case 2:
                {
                    Hitbox_clear(N_hitbox_1);
                    Hitbox_use(N_hitbox_2);
                    break;
                }
            case 3:
                {
                    Hitbox_clear(N_hitbox_1);
                    Hitbox_clear(N_hitbox_2);
                    Hitbox_use(N_hitbox_3);
                    break;
                }
                default:               
                isattack = false;
                Hitbox_clear(N_hitbox_3);
                Hitbox_clear(N_hitbox_2);
                Hitbox_clear(N_hitbox_1);             
                break;
                
        }
    }
    public void beAttackedByBullet()//检测受到子弹的攻击
    {
        if (player_0.position.x >= farAttackEnemy.position.x)
        {
            Debug.Log("向右飞");
            rb.AddForce(new Vector2(2000, 250), ForceMode2D.Force);
        }
        else if(player_0.position.x < farAttackEnemy.position.x)
        {
            Debug.Log("向左飞");
            rb.AddForce(new Vector2(-2000, 250), ForceMode2D.Force);
        }
    }
}
   
