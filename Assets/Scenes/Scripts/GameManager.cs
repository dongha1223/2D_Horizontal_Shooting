using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    public int stage;
    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;
    public Transform playerPos;

    public string[] enemyObjs;
    public Transform[] spawnPoint;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] BoomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "EnemyS", "EnemyM", "EnemyL","EnemyB" };
        StageStart();
    }

    public void StageStart()
    {
        //Stage UI Load
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = "Stage " + stage + "\nStart";
        clearAnim.GetComponent<Text>().text = "Stage " + stage + "\nEnd";
        //Enemy Spawn File Read
        ReadSpawnFile();

        //Fade In
        fadeAnim.SetTrigger("In");
    }

    public void StageEnd()
    {
        //Clear UI Load
        clearAnim.SetTrigger("On");
        //Fade Out
        fadeAnim.SetTrigger("Out");
        //Player Repos
        player.transform.position = playerPos.position;
        //Stage Increment
        stage++;
        if (stage > 2)
            Invoke("GameOver", 6);
        else
            Invoke("StageStart", 5);
    }

    void ReadSpawnFile()
    {
        //변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;

        //리스폰 파일 읽기
        TextAsset textFile = Resources.Load("Stage" + stage) as TextAsset;
        StringReader stringReader = new StringReader(textFile.text);

        //한 줄씩 데이터 저장 
        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            Debug.Log(line);

            if (line == null)
                break;

            //리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        stringReader.Close();

        //첫번째 스폰 딜레이 적용
        nextSpawnDelay = spawnList[0].delay;
    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if(curSpawnDelay > nextSpawnDelay && !spawnEnd)
        {
            SpawnEnemy();
            curSpawnDelay = 0; 
        }

        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}",playerLogic.score);
        
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }
   
        int enemyPoint = spawnList[spawnIndex].point;
        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoint[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();
        enemyLogic.player = player;
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;

        if (enemyPoint == 5 || enemyPoint == 6)
        {
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }
        else if (enemyPoint == 7 || enemyPoint == 8)
        {
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }
        else
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed*(-1));
        }

        //리스폰 인덱스 증가
        spawnIndex++;
        if(spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        //다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;

    }

    public void RespawnPlayer()
    {

        Invoke("RespawnPlayerExe", 2f);
    }
    void RespawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }
    public void UpdateLifeImage(int life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }
   
        for (int index = 0;index < life; index++ ) {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void UpdateBoomImage(int boom)
    {
        for (int index = 0; index < 3; index++)
        {
            BoomImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < boom; index++)
        {
            BoomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion explosionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        explosionLogic.StartExplosion(type);
    }
    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(0);
    }
}
