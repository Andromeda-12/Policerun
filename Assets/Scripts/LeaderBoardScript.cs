using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Linq;

public class LeaderBoardScript : MonoBehaviour
{
    public FirebaseDatabase dbRef;
    [Header("Text")]
    public Text text;
    public void Start()
    {
        var players = DataHolder.GetPlayers();  // онксвюел яохянй хцпнйнб
        foreach (var p in players.OrderBy(p => -p.score).Take(10))  // аепел х бшбндхл рно 10 йпюяюбвхйнб он нвйюл
            text.text += $"хЛЪ: {p.name} | яВ╦Р: {p.score} | хЦПШ: {p.plays}\n";

    }
}
