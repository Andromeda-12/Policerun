using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataHolder
{

    // ����� �������� ������ � ������� ������

    public static string Name { get; set; } = "Player"; // ��� ���
    public static uint Score { get; set; } // ��� ����
    public static int CountGame { get; set; } = 0; // ������� ��� ���ب� ����
    public static List<player> players { get; set; } // ������ ���� �������, �����-���� �������� � ����
                                                    // ������ ��� ����� � �� �����
    public static void AddPlayer(string name, uint score, int plays)  // ���������� ������ ������ � ������
    {
        players.Add(new player()
        {
            name = name,
            score = score,
            plays = plays
        });
    }
    public static List<player> GetPlayers()  // ��������� ������ �������
    {
        return players;
    }
}
public class player  // ������ ������
{
    public string name;
    public uint score;
    public int plays;
    public override string ToString()
    {
        return $"{name}   {score}   {plays}";
    }
}
