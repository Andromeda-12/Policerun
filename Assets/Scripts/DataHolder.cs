using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{

    // гдеяэ упюмъряъ дюммше н рейсыел хцпнйе

    public static string Name { get; set; } = "Player"; // ецн хлъ
    public static uint Score { get; set; } // ецн явер
    public static int CountGame { get; set; } = 0; // яйнкэйн пюг опнь╗к хцпс
    public static List<player> players { get; set; } // яохянй бяеу хцпнйнб, йнцдю-кхан хцпюбьху б хцпс
                                                    // онвелс нмн гдеяэ ≈ ме бюфмн
    public static void AddPlayer(string name, uint score, int plays)  // днаюбкемхе мнбнцн хцпнйю б яохянй
    {
        players.Add(new player()
        {
            name = name,
            score = score,
            plays = plays
        });
    }
    public static List<player> GetPlayers()  // онксвемхе яохяйю хцпнйнб
    {
        return players;
    }
}
public class player  // лндекэ хцпнйю
{
    public string name;
    public uint score;
    public int plays;
    public override string ToString()
    {
        return $"{name}   {score}   {plays}";
    }
}
