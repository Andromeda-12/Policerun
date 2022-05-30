using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    [Header("Î÷êè èãðîêà")]
    public ScoreCounter scoreCounter;

    [Header("Ãåíåðàòîð îêðóæåíèÿ")]
    public EnvironmentGenerator environmentGenerator;

    //public FinalScreen

    [Header("Èãðîê")]
    public Player player;

    private bool isGameRun;
    private float scoreBoosterTimer;
    private float jumpBoosterTimer;

    FireBaseController db;
    [ContextMenu("StartGame")]
    public void StartGame()
    {

        isGameRun = true;
        environmentGenerator.isRun = true;
        player.isRun = true;
    }

    [ContextMenu("EndGame")]
    public void EndGame()
    {
        isGameRun = false;
        environmentGenerator.isRun = false;
        player.isRun = false;
        player.Stop();
        DataHolder.Score = scoreCounter.Score; // ÓÑÒÀÍÀÂËÈÂÀÅÌ ÑÊÎËÜÊÎ Î×ÊÎÂ ÍÀÁÐÀË ÈÃÐÎÊ
        var obj = GameObject.Find("Canvas");
        var childobj = obj.transform.Find("FinalScreen").gameObject;
        childobj.SetActive(true);  // ÏÎÊÀÇÛÂÀÅÌ ÔÈÍÀËÜÍÛÉ ÝÊÐÀÍ
        db.SaveData();  // ÑÎÕÐÀÍßÅÌ ÄÀÍÍÛÅ Â ÁÄ
        enabled = false; // ÂÛÐÓÁÀÅÌ ÑÊÐÈÏÒ
    }

    private void FixedUpdate()
    {
        if (isGameRun)
            scoreCounter.IncreaseScore();

        CheckScore();
        CheckPlayerForCollisionWithBooster();
        CheckJumpBooster();

        bool isPlayerHitAnObstacle = CheckPlayerForCollision();
        if (isPlayerHitAnObstacle)
            EndGame();

    }

    private void CheckScore()
    {
        if (player.isHasScoreBooster && (Time.realtimeSinceStartup - scoreBoosterTimer > 5f))
        {
            player.isHasScoreBooster = false;
            scoreCounter.isScoreBooster = false;
            environmentGenerator.speedEnv = 10;
            player.SetSpeed(1);
        }

        if (!player.isHasScoreBooster)
        {
            if (scoreCounter.Score > 500)
                scoreCounter.ScoreFactor = 2;

            if (scoreCounter.Score > 1000)
                scoreCounter.ScoreFactor = 3;

            if (scoreCounter.Score > 10000)
                scoreCounter.ScoreFactor = 4;

            if (scoreCounter.Score > 25000)
                scoreCounter.ScoreFactor = 5;

            if (scoreCounter.Score >= 100000)
                EndGame();

        }
        else
        {
            scoreCounter.ScoreFactor = 100;
            scoreCounter.isScoreBooster = true;
            environmentGenerator.speedEnv = 15;
            player.SetSpeed(1.2f);
        }

    }

    private bool CheckPlayerForCollision()
    {
        if (player.isHitAnObstacle)
        {
            if (player.isHasSecondLife)
            {
                player.isHitAnObstacle = false;
                player.isHasSecondLife = false;
                environmentGenerator.DeleteObstacles();
                return false;
            }

            return true;
        }

        return false;
    }

    private void CheckPlayerForCollisionWithBooster()
    {
        if (player.isHasCollisionWithBooster)
        {
            player.isHasCollisionWithBooster = false;
            string effect = player.playerEffect;

            if (effect == "SecondLife")
            {
                player.isHasSecondLife = true;
            }

            if (effect == "ScoreBooster")
            {
                player.isHasScoreBooster = true;
                scoreBoosterTimer = Time.realtimeSinceStartup;
            }

            if (effect == "JumpBooster")
            {
                player.isHasJumpBooster = true;
                jumpBoosterTimer = Time.realtimeSinceStartup;
                player.setJumpForce(18);
            }

            environmentGenerator.DeleteBoosters();
        }
    }

    private void CheckJumpBooster()
    {
        if (player.isHasJumpBooster && (Time.realtimeSinceStartup - jumpBoosterTimer > 5f))
        {
            player.isHasJumpBooster = false;
            player.setJumpForce(12);
        }
    }
    public void Start()
    {
        db = GetComponent<FireBaseController>();
        db.dbRef = FirebaseDatabase.DefaultInstance;
        StartGame();
    }
}
