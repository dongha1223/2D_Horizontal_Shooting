using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public float speed;
    public int health;
    public int enemyScore;
    public Sprite[] sprites;

    public float maxShootDelay;
    public float curShootDelay;

    public GameObject player;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemBoom;
    public GameObject itemPower;
    public ObjectManager objectManager;
    public GameManager gameManager;


    SpriteRenderer spriteRenderer;
    Animator anim;

    public int patternIndex;
    public int currentPatternCount;
    public int[] maxPatternCount;


    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>(); 

        if(enemyName == "B")
        {
            anim = GetComponent<Animator>();
        }
    }

    void OnEnable()
    {
        switch (enemyName)
        {
            case "L":
                health = 30;
                break;
            case "M":
                health = 6;
                break;
            case "S":
                health = 10;
                break;
            case "B":
                health = 3000;
                Invoke("Stop", 2);
                break;
        }
    }

    void Stop()
    {
        if(!gameObject.activeSelf)
        {
            return;
        }

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 1.5f);
    }

    void Think()
    {
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;
        currentPatternCount = 0;
        switch (patternIndex)
        {
            case 0:
                FireFoward();
                return;
            case 1:
                FireShot();
                return;
            case 2:
                FireArc();
                return;
            case 3:
                FireAround();
                return;
        }
    }

    void FireFoward()
    {
        if (health <= 0) return;
        //FireFoward ÆÐÅÏ
        GameObject bulletL = objectManager.MakeObj("BulletBossB");
        bulletL.transform.position = transform.position + Vector3.left * 0.2f;
        GameObject bulletLL = objectManager.MakeObj("BulletBossB");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;
        GameObject bulletR = objectManager.MakeObj("BulletBossB");
        bulletR.transform.position = transform.position + Vector3.right * 0.2f;
        GameObject bulletRR = objectManager.MakeObj("BulletBossB");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;


        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();

        rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);


        currentPatternCount++;

        if(currentPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireFoward", 1);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void FireShot()
    {
        if (health <= 0) return;
        for (int index = 0; index < 5; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyB");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0, 2f));
            dirVec += ranVec;
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }

        currentPatternCount++;

        if (currentPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireShot", 1);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void FireArc()
    {
        if (health <= 0) return;
        GameObject bullet = objectManager.MakeObj("BulletEnemyA");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;
        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 15 * currentPatternCount/maxPatternCount[patternIndex]), -1);
        rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);

        currentPatternCount++;

        if (currentPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireArc", 0.05f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void FireAround()
    {
        if (health <= 0) return;
        int roindNum = Random.Range(30, 40);

        for (int index = 0; index < roindNum; index++)
        {
            GameObject bullet = objectManager.MakeObj("BulletBossA");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roindNum), Mathf.Sin(Mathf.PI * 2 * index / roindNum));
            rigid.AddForce(dirVec.normalized * 2, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roindNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }
        currentPatternCount++;

        if (currentPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireAround", 1.2f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }



    void Update()
    {
        if(enemyName == "B")
        {
            return;
        }
        Fire();
        Reload();
    }

    void Fire()
    {
        if (curShootDelay < maxShootDelay)
        {
            return;
        }

        if (enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("BulletEnemyA");
            bullet.transform.position = transform.position;
            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 derVec = player.transform.position - transform.position;
            rigid.AddForce(derVec.normalized * 5, ForceMode2D.Impulse);
        }
        else if (enemyName == "L")
        {
            GameObject bulletL = objectManager.MakeObj("BulletEnemyB");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
            Vector3 derVecL = player.transform.position - transform.position ;
            rigidL.AddForce(derVecL.normalized * 3, ForceMode2D.Impulse);

            GameObject bulletR = objectManager.MakeObj("BulletEnemyB");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;
            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Vector3 derVecR = player.transform.position - transform.position ;
            rigidR.AddForce(derVecR.normalized * 3, ForceMode2D.Impulse);
        }

        curShootDelay = 0;
    }
    void Reload()
    {
        curShootDelay += Time.deltaTime;
    }

    public void OnHit(int dmg)
    {
        if (health <= 0)
            return;

        health -= dmg;
        if (enemyName == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.2f);
        }

        if(health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScore;

            //Random Ratio Item Drop
            int ran = enemyName == "B" ? 0 :Random.Range(0, 10);
            if (ran > 8)
            {
                GameObject itemCoin = objectManager.MakeObj("ItemCoin");
                itemCoin.transform.position = transform.position;
            }
            else if (ran > 5)
            {
                GameObject itemPower = objectManager.MakeObj("ItemPower");
                itemPower.transform.position = transform.position;
            }
            else if (ran > 4)
            {
                GameObject itemBoom = objectManager.MakeObj("ItemBoom");
                itemBoom.transform.position = transform.position;
            }

            objectManager.DeleteAllObj("Boss");

            gameObject.SetActive(false);

            gameManager.CallExplosion(transform.position, enemyName);

            if(enemyName == "B")
            {
                gameManager.StageEnd();
            }
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet" && enemyName != "B")
        {
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();

            OnHit(bullet.dmg);
            
            
            collision.gameObject.SetActive(false);
        }
    }
}
