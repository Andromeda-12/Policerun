using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ScoreCounter : MonoBehaviour
{
    public uint Score = 0;
    public int ScoreFactor = 0;
    public bool isScoreBooster;

    private float deltaScore = 0;
    private float realScore = 0;

    private void Start()
    {
        Score = 0;
        realScore = 0;
        deltaScore = 0.5f;
        ScoreFactor = 1;
    }

    public void IncreaseScore()
    {
        realScore += deltaScore * ScoreFactor;
        Score = Convert.ToUInt32(realScore);
    }

    void OnGUI()
    {
        GUIStyle style = new GUIStyle(EditorStyles.label)
        {
            richText = true,
            fontSize = Screen.height / 20,
            fontStyle = FontStyle.Bold,
        };
        style.normal.textColor = Color.white;

        int offsetX = 30 + Score.ToString().Length * 10;

        Rect rect_Label = new Rect(Screen.width - offsetX, 10, 1000, 20);
        GUI.Label(rect_Label, $"{Score}", style);

        if (isScoreBooster)
        {
            int offsetXBooster = 30 + ScoreFactor.ToString().Length * 10 + 1;
            Rect scoreBoosterRect = new Rect(Screen.width - offsetXBooster, 10 + 20, 1000, 20);
            GUI.Label(scoreBoosterRect, $"x{ScoreFactor}", style);
        }
    }
}
