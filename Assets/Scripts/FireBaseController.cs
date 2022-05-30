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
        if(DataHolder.players.Exists(p=> p.name == DataHolder.Name))  // еякх хцпнй сфе хцпюк б хцпс рн
        {
            var player = DataHolder.players.First(p => p.name == DataHolder.Name);
            var plays = player.plays;
            var score = player.score;
            if (DataHolder.Score >= 100000)
            {
                DataHolder.CountGame = ++plays; // опх опнунфдемхх хцпш сбекхвхбюел ецн явервхй
            }
            else if(DataHolder.Score < score)
            {
                DataHolder.Score = score;  // еякх мюапюк лемэье нвйнб, вел б опнькши пюг, ме слемэьюел нвйх
            }
        }
        // мхфе днаюбкемхе хцпнйнб б рюакхжс PLAYERS
        dbRef.GetReference("Players").Child(DataHolder.Name).SetValueAsync(DataHolder.Name);
        dbRef.GetReference("Players").Child(DataHolder.Name).Child("Score").SetValueAsync(DataHolder.Score);
        dbRef.GetReference("Players").Child(DataHolder.Name).Child("Plays").SetValueAsync(DataHolder.CountGame);
    }
    public void LoadData()
    {
        // онксвюел яохянй бяеу хцпнйнб хг рюакхжш PLAYERS
        dbRef.GetReference("Players").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                // цпсярхл
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                foreach (DataSnapshot data in snapshot.Children)
                {
                    DataHolder.AddPlayer(data.Key.ToString(), Convert.ToUInt32(data.Child("Score").Value), Convert.ToInt32(data.Child("Plays").Value));
                } // гюонкмъел яохянй хцпнйнб
            };
        });
    }
    public void Start()
    {
        dbRef = FirebaseDatabase.DefaultInstance; // гбнмхл ад
        dbRef.SetPersistenceEnabled(false); // ме рпнцюрэ, мюдн, врнаш пюанрюкн
        DataHolder.players = new List<player>(); // хмхжхюкхгюжхъ йнккейжхх
        LoadData();
    }
}
