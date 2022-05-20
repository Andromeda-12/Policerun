using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SensorObstacle : MonoBehaviour
{
    private int m_ColCount = 0;

    private float m_DisableTimer;

    public bool isHasCollision;
    public bool isHasCollisionWithBooster;

    public string playerEffect;

    private void OnEnable()
    {
        m_ColCount = 0;
    }

    public bool State()
    {
        //if (m_DisableTimer > 0)
        //    return false;
        //return m_ColCount > 0;
        return isHasCollision;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Obstacle")
        {
            isHasCollision = true;
            print("Ouch");
        }

        if(other.tag == "Booster")
        {
            isHasCollisionWithBooster = true;

            if (other.name.Contains("SecondLife"))
                playerEffect = "SecondLife";

            if (other.name.Contains("ScoreBooster"))
                playerEffect = "ScoreBooster";

            if (other.name.Contains("JumpBooster"))
                playerEffect = "JumpBooster";
        }
    }

    //private void OnCollisionEnter2D(Collision2D other)
    //{
    //    if (other.transform.tag == "Booster")
    //    {
    //        other.collider.
    //        isHasCollisionWithBooster = true;
    //        if (other.transform.name.Contains("SecondLife"))
    //            playerEffect = "SecondLife";
    //    }
    //}

    void OnTriggerExit2D(Collider2D other)
    {
        m_ColCount--;
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        m_DisableTimer = duration;
    }
}
