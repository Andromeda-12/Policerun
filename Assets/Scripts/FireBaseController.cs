using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FireBaseController : MonoBehaviour
{
    public FirebaseDatabase dbRef;
    public void SaveData()
    {
        if(DataHolder.players.Exists(p=> p.name == DataHolder.Name))  // ���� ����� ��� ����� � ���� ��
        {
            var player = DataHolder.players.First(p => p.name == DataHolder.Name);
            var plays = player.plays;
            var score = player.score;
            if (DataHolder.Score >= 100000)
            {
                DataHolder.CountGame = ++plays; // ��� ����������� ���� ����������� ��� �������
            }
            else if(DataHolder.Score < score)
            {
                DataHolder.Score = score;  // ���� ������ ������ �����, ��� � ������� ���, �� ��������� ����
            }
        }
        // ���� ���������� ������� � ������� PLAYERS
        dbRef.GetReference("Players").Child(DataHolder.Name).SetValueAsync(DataHolder.Name);
        dbRef.GetReference("Players").Child(DataHolder.Name).Child("Score").SetValueAsync(DataHolder.Score);
        dbRef.GetReference("Players").Child(DataHolder.Name).Child("Plays").SetValueAsync(DataHolder.CountGame);
    }
    public void LoadData()
    {
        // �������� ������ ���� ������� �� ������� PLAYERS
        dbRef.GetReference("Players").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // �������
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    DataHolder.AddPlayer(data.Key.ToString(), Convert.ToUInt32(data.Child("Score").Value), Convert.ToInt32(data.Child("Plays").Value));
                } // ��������� ������ �������
            };
        });
    }
    public void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance; // ������ ��
        dbRef.SetPersistenceEnabled(false); // �� �������, ����, ����� ��������
        DataHolder.players = new List<player>(); // ������������� ���������
        LoadData();
    }
}
