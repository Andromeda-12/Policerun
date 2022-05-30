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
        var players = DataHolder.GetPlayers();  // �������� ������ �������
        foreach (var p in players.OrderBy(p => -p.score).Take(10))  // ����� � ������� ��� 10 ����������� �� �����
            text.text += $"���: {p.name} | ����: {p.score} | ����: {p.plays}\n";
    }
}
