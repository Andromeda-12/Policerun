using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    public void Scenes(int numberScenes)
    {
        SceneManager.LoadScene(numberScenes); // ��������� ����� �� ������� � �������
    }
    public void SetName(string name) // ������������� ��� ������
    {
        DataHolder.Name = name;
    }
}
