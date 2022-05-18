using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    private List<GameObject> ReadyEnvironments = new List<GameObject>();

    [Header("�����������")]
    public List<GameObject> prefabList = new List<GameObject>();

    [Header("�������")]
    public bool isRun;

    [Header("��� ������� ��������� ������")]
    public GameObject[] Environments;
    [Header("������� ����� ������")]
    public int currentEnvironmentLength = 0;
    [Header("������������ ����� ������")]
    public int maximumEnvironmentLength = 3;
    [Header("��������� ����� ��������")]
    public int distanceBetweenEnvironments = 10;
    [Header("�������� ������")]
    public float speedEnv = 10;
    [Header("������� � ��� ������� ��������� ������")]
    public float maximumPositionX = -15;
    [Header("���� ��������")]
    public Vector3 waitingZone = new Vector3(-40, 0, 0);
    [Header("������ ���������")]
    public string envGenerationStatus = "Generation";
    public bool[] envNumbers;

    private int currentEnvNumber = -1;
    private int lastEnvNumber = -1;

    private void FixedUpdate()
    {
        if (envGenerationStatus == "Generation")
        {
            if (currentEnvironmentLength != maximumEnvironmentLength)
            {
                currentEnvNumber = Random.Range(0, Environments.Length);

                if (currentEnvNumber != lastEnvNumber)
                {
                    if (currentEnvNumber < Environments.Length / 2)
                    {
                        if (envNumbers[currentEnvNumber] != true)
                        {
                            if (lastEnvNumber != (Environments.Length / 2) + currentEnvNumber)
                            {
                                EnvironmentCreation();
                            }
                            else if (lastEnvNumber == (Environments.Length / 2) + currentEnvNumber && currentEnvironmentLength == Environments.Length - 1)
                            {
                                EnvironmentCreation();
                            }
                        }
                    }
                    else if (currentEnvNumber >= Environments.Length / 2)
                    {
                        if (envNumbers[currentEnvNumber] != true)
                        {
                            if (lastEnvNumber != currentEnvNumber - (Environments.Length / 2))
                            {
                                EnvironmentCreation();
                            }
                            else if (lastEnvNumber == currentEnvNumber - (Environments.Length / 2) && currentEnvironmentLength == Environments.Length - 1)
                            {
                                EnvironmentCreation();
                            }
                        }
                    }
                }
            }

            MovingEnvironment();

            if (ReadyEnvironments.Count != 0)
            {
                RemoveEnvironment();
            }
        }
    }

    private void EnvironmentCreation()
    {
        if (ReadyEnvironments.Count > 0)
        {
            Environments[currentEnvNumber].transform.localPosition = ReadyEnvironments[ReadyEnvironments.Count - 1].transform.position + new Vector3(distanceBetweenEnvironments, 0f, 0f);
        }
        else if (ReadyEnvironments.Count == 0)
        {
            Environments[currentEnvNumber].transform.localPosition = new Vector3(0f, 0f, 0f);
        }

        Environments[currentEnvNumber].GetComponent<Environment>().number = currentEnvNumber;



        envNumbers[currentEnvNumber] = true;
        lastEnvNumber = currentEnvNumber;
        ReadyEnvironments.Add(Environments[currentEnvNumber]);
        currentEnvironmentLength++;
    }

    private void MovingEnvironment()
    {
        if (isRun)
        {
            foreach (GameObject readyEnv in ReadyEnvironments)
            {
                readyEnv.transform.localPosition -= new Vector3(speedEnv * Time.fixedDeltaTime, 0f, 0f);
            }
        }
    }

    private void RemoveEnvironment()
    {
        if (ReadyEnvironments[0].transform.localPosition.x < maximumPositionX)
        {
            int i;
            i = ReadyEnvironments[0].GetComponent<Environment>().number;
            envNumbers[i] = false;

            var obstracles = ReadyEnvironments[0].gameObject.transform.Find("Obstracles");
            for (int j = 0; j < obstracles.transform.childCount; j++)
            {
                Destroy(obstracles.transform.GetChild(j).gameObject);
            }

            CreateObstracles(ReadyEnvironments[ReadyEnvironments.Count - 1]);

            ReadyEnvironments[0].transform.localPosition = waitingZone;
            ReadyEnvironments.RemoveAt(0);
            currentEnvironmentLength--;
        }
    }

    private void CreateObstracles(GameObject environment)
    {
        GameObject prefab = GetRandomObstracle();
        var envPosition = environment.transform.position;

        var deltaX = Random.Range(-distanceBetweenEnvironments / 4, distanceBetweenEnvironments / 4);
        var deltaY = Random.Range(0, 4);
        var position = new Vector3(envPosition.x + deltaX, envPosition.y + deltaY, envPosition.z);

        var obstracles = environment.gameObject.transform.Find("Obstracles");
        var obj = Instantiate(prefab, position, Quaternion.identity);
        obj.transform.SetParent(obstracles.transform);
    }

    private GameObject GetRandomObstracle()
    {
        return prefabList[Random.Range(0, prefabList.Count)];
    }
}
