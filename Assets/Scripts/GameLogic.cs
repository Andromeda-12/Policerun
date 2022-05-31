using Firebase.Database;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public Text text1;
    public Text text2;
    public Text text3;

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
        if (scoreCounter.Score >= 100000)
            EndGame();

        if (player.isHasScoreBooster && (Time.realtimeSinceStartup - scoreBoosterTimer > 5f))
        {
            player.isHasScoreBooster = false;
            scoreCounter.isScoreBooster = false;
            environmentGenerator.speedEnv = 7;
            player.SetSpeed(1);
            text1.text = string.Empty;
        }

        if (!player.isHasScoreBooster)
        {
            if (scoreCounter.Score > 500)
                scoreCounter.ScoreFactor = 2;

            if (scoreCounter.Score > 1000)
                scoreCounter.ScoreFactor = 3;

            if (scoreCounter.Score > 5000)
                scoreCounter.ScoreFactor = 4;

            if (scoreCounter.Score > 10000)
                scoreCounter.ScoreFactor = 5;

            if (scoreCounter.Score > 20000)
                scoreCounter.ScoreFactor = 6;
        }
        else
        {
            scoreCounter.ScoreFactor = 100;
            scoreCounter.isScoreBooster = true;
            environmentGenerator.speedEnv = 9;
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
                text2.text = string.Empty;
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
                text2.text = "Second life";
            }

            if (effect == "ScoreBooster")
            {
                player.isHasScoreBooster = true;
                scoreBoosterTimer = Time.realtimeSinceStartup;
                text1.text = "x100";
            }

            if (effect == "JumpBooster")
            {
                player.isHasJumpBooster = true;
                jumpBoosterTimer = Time.realtimeSinceStartup;
                player.setJumpForce(16);
                text3.text = "Jump booster";
            }

            environmentGenerator.DeleteBoosters();
        }
    }

    private void CheckJumpBooster()
    {
        if (player.isHasJumpBooster && (Time.realtimeSinceStartup - jumpBoosterTimer > 5f))
        {
            player.isHasJumpBooster = false;
            player.setJumpForce(11);
            text3.text = string.Empty;
        }
    }

    public void Start()
    {
        db = GetComponent<FireBaseController>();
        db.dbRef = FirebaseDatabase.DefaultInstance;
        StartGame();
    }
}
