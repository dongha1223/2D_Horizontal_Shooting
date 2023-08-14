using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed;
    public float maxShootDelay;
    public float curShootDelay;
    public int power;
    public int maxPower;
    public int boom;
    public int maxBoom;

    public int life;
    public int score;

    public bool isTouchTop;
    public bool isTouchBottom;
    public bool isTouchLeft;
    public bool isTouchRight;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject boomEffect;

    public Enemy enemy;

    public GameManager gameManager;
    public ObjectManager objectManager;

    public bool isHit;
    public bool isBoomTime;

    public GameObject[] followers;
    public bool isRespawnTime;

    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        Unbeatable();
        Invoke("Unbeatable",3);
    }

    void Unbeatable()
    {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            for(int index=0; index<followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            }
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);

            for (int index = 0; index < followers.Length; index++)
            {
                followers[index].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
            }
        }


    }
    void Update()
    {
        Move();
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();
        }
        Boom();
        Reload();
    }

    void Move ()
    {
        float h = Input.GetAxisRaw("Horizontal");
        if ((isTouchRight && h == 1) || (isTouchLeft && h == -1))
            h = 0;
        float v = Input.GetAxisRaw("Vertical");
        if ((isTouchTop && v == 1) || (isTouchBottom && v == -1))
            v = 0;
        Vector2 curPos = transform.position;
        Vector2 nextPos = new Vector2(h, v) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void Fire()
    {
        if (curShootDelay < maxShootDelay)
        {
            return;
        }
        switch(power)
        {
            case 1:
                GameObject bullet = objectManager.MakeObj("BulletPlayerA");
                bullet.transform.position = transform.position ;
                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = objectManager.MakeObj("BulletPlayerA");
                bulletR.transform.position = transform.position + Vector3.right*0.2f;
                GameObject bulletL = objectManager.MakeObj("BulletPlayerA");
                bulletL.transform.position = transform.position + Vector3.left * 0.2f;
                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletRR = objectManager.MakeObj("BulletPlayerA");
                bulletRR.transform.position = transform.position + Vector3.right * 0.2f;
                GameObject bulletLL = objectManager.MakeObj("BulletPlayerA");
                bulletLL.transform.position = transform.position + Vector3.left * 0.2f;
                GameObject bulletCC = objectManager.MakeObj("BulletPlayerA");
                bulletCC.transform.position = transform.position;
                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 4:
                GameObject bullet1 = objectManager.MakeObj("BulletPlayerB");
                bullet1.transform.position = transform.position;
                Rigidbody2D rigid1 = bullet1.GetComponent<Rigidbody2D>();
                rigid1.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 5:
                GameObject bulletC1 = objectManager.MakeObj("BulletPlayerB");
                bulletC1.transform.position = transform.position;
                GameObject bulletR1 = objectManager.MakeObj("BulletPlayerA");
                bulletR1.transform.position = transform.position + Vector3.right * 0.2f;
                GameObject bulletL1 = objectManager.MakeObj("BulletPlayerA");
                bulletL1.transform.position = transform.position + Vector3.left * 0.2f;
                Rigidbody2D rigidC1 = bulletC1.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidR1 = bulletR1.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL1 = bulletL1.GetComponent<Rigidbody2D>();
                rigidC1.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidR1.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL1.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            default :
                GameObject bulletR2 = objectManager.MakeObj("BulletPlayerB");
                bulletR2.transform.position = transform.position + Vector3.right * 0.2f;
                GameObject bulletL2 = objectManager.MakeObj("BulletPlayerB");
                bulletL2.transform.position = transform.position + Vector3.left * 0.2f;
                Rigidbody2D rigidR2 = bulletR2.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL2 = bulletL2.GetComponent<Rigidbody2D>();
                rigidR2.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidL2.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;


        }

        curShootDelay = 0;
    }

    void Reload()
    {
        curShootDelay += Time.deltaTime;
    }

    void Boom()
    {
        if (!Input.GetKey(KeyCode.Z)) {
            return;
        }
        if (isBoomTime)
            return;

        if (boom == 0)
            return;

        boom--;
        isBoomTime = true;
        gameManager.UpdateBoomImage(boom);

        {
            //Effect Visible
            boomEffect.SetActive(true);
            Invoke("OffBoomEffect", 3);

            //Remove Enemy
            GameObject[] enemiesS = objectManager.GetPool("EnemyS");
            GameObject[] enemiesM = objectManager.GetPool("EnemyM");
            GameObject[] enemiesL = objectManager.GetPool("EnemyL");

            for (int index = 0; index < enemiesL.Length; index++)
            {
                if (enemiesL[index].activeSelf)
                {
                    Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                    enemyLogic.OnHit(1000);
                }
            }
            for (int index = 0; index < enemiesM.Length; index++)
            {
                if (enemiesM[index].activeSelf)
                {
                    Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                    enemyLogic.OnHit(1000);
                }
            }
            for (int index = 0; index < enemiesS.Length; index++)
            {
                if (enemiesS[index].activeSelf)
                {
                    Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                    enemyLogic.OnHit(1000);
                }
            }

            //Remove Enemy Bullet
            GameObject[] bulletsA = objectManager.GetPool("BulletEnemyA");
            GameObject[] bulletsB = objectManager.GetPool("BulletEnemyB");
            for (int index = 0; index < bulletsA.Length; index++)
            {
                if(bulletsA[index].activeSelf)
                {
                    bulletsA[index].SetActive(false);
                }
            }
            for (int index = 0; index < bulletsB.Length; index++)
            {
                if (bulletsB[index].activeSelf)
                {
                    bulletsB[index].SetActive(false);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = true;
                    break;
                case "Bottom":
                    isTouchBottom = true;
                    break;
                case "Left":
                    isTouchLeft = true;
                    break;
                case "Right":
                    isTouchRight = true;
                    break;
            }
        }
        else if (collision.gameObject.tag == "EnemyBullet" || collision.gameObject.tag == "Enemy")
        {
            if (isRespawnTime)
                return;

            if (isHit)
            {
                return;
            }

            isHit = true;
            life--;
            gameManager.UpdateLifeImage(life);
            gameManager.CallExplosion(transform.position, "P");

            if (life == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.RespawnPlayer();
            }
            gameObject.SetActive(false);
            

        }

        else if (collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (power == maxPower)
                    {
                        score += 500;
                    }
                    else
                    {
                        power++;
                        Addfollower();
                    }
                    break;
                case "Boom":
                    if (boom == maxBoom)
                        score += 500;
                    else
                    {
                        boom++;
                        gameManager.UpdateBoomImage(boom);
                    }
                        
                    break;
            }
            collision.gameObject.SetActive(false);
        }
    }

    void Addfollower()
    {
        if (power == 7)
        {
            followers[0].SetActive(true);
        }
        else if (power == 8)
            followers[1].SetActive(true);
        else if (power == 9)
            followers[2].SetActive(true);
    }
    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top":
                    isTouchTop = false;
                    break;
                case "Bottom":
                    isTouchBottom = false;
                    break;
                case "Left":
                    isTouchLeft = false;
                    break;
                case "Right":
                    isTouchRight = false;
                    break;
            }
        }
    }
}
