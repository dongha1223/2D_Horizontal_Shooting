using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public float maxShootDelay;
    public float curShootDelay;

    public ObjectManager objectManager;

    public Vector3 followPos;
    public int followDelay;
    public Transform parent;
    public Queue<Vector3> parentPos;

    void Awake()
    {
        //Queue 먼저 들어가면 먼저 나온다 FIFO
        parentPos = new Queue<Vector3>();
    }
    void Update()
    {
        Watch();
        Follow();
        if (Input.GetKey(KeyCode.Space))
        {
            Fire();

            Reload();
        }
    }
    void Watch()
    {
        //Input Pos
        if(!parentPos.Contains(parent.position))
            parentPos.Enqueue(parent.position);

        //Output Pos
        if (parentPos.Count > followDelay)
            followPos = parentPos.Dequeue();
        else if (parentPos.Count < followDelay)
            followPos = parent.position;
    }
    void Follow()
    {
            transform.position = followPos;
            
    }

    void Fire()
    {
        if (curShootDelay < maxShootDelay)
        {
            return;
        }

        GameObject bullet = objectManager.MakeObj("BulletFollower");
        bullet.transform.position = transform.position;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
        rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

        curShootDelay = 0;
    }
    void Reload()
    {
        curShootDelay += Time.deltaTime;
    }
}

