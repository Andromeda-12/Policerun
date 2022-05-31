using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour
{
    private List<GameObject> ReadyEnvironments = new List<GameObject>();

    [Header("Препятствия")]
    public List<GameObject> prefabList = new List<GameObject>();

    [Header("Бустеры")]
    public List<GameObject> boostersList = new List<GameObject>();

    [Header("Двигать")]
    public bool isRun;

    [Header("Шанс спавна бустеров")]
    public int boosterSpawnChance;

    [Header("Все участки окружения дороги")]
    public GameObject[] Environments;
    [Header("Текущая длина дороги")]
    public int currentEnvironmentLength = 0;
    [Header("Максимальная длина дороги")]
    public int maximumEnvironmentLength = 3;
    [Header("Дистанция между дорогами")]
    public float distanceBetweenEnvironments = 10;
    [Header("Скорость дороги")]
    public float speedEnv = 10;
    [Header("Позиция Х при которой удаляется дорога")]
    public float maximumPositionX = -15;
    [Header("Зона ожидания")]
    public Vector3 waitingZone = new Vector3(-40, 0, 0);
    [Header("Статус генерации")]
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

        if ((Random.Range(0, 100) < boosterSpawnChance) && isRun)
            CreateBooster();

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

            //var boosters = ReadyEnvironments[0].gameObject

            CreateObstracles(ReadyEnvironments[ReadyEnvironments.Count - 1]);

            ReadyEnvironments[0].transform.localPosition = waitingZone;
            ReadyEnvironments.RemoveAt(0);
            currentEnvironmentLength--;
        }
    }

    private void CreateObstracles(GameObject environment)
    {
        if (Random.Range(0, 100) <= 80)
        {
            GameObject prefab = GetRandomObstracle();
            var envPosition = environment.transform.position;

            var deltaX = Random.Range(-distanceBetweenEnvironments / 4, distanceBetweenEnvironments / 4);
            var deltaY = Random.Range(0f, 3f);
            var position = new Vector3(envPosition.x + deltaX, envPosition.y + deltaY, envPosition.z);

            var obstracles = environment.gameObject.transform.Find("Obstracles");
            var obj = Instantiate(prefab, position, Quaternion.identity);
            obj.transform.SetParent(obstracles.transform);
        }
    }

    private GameObject GetRandomObstracle()
    {
        return prefabList[Random.Range(0, prefabList.Count)];
    }

    private GameObject GetRandomBooster()
    {
        return boostersList[Random.Range(0, boostersList.Count)];
    }

    public void DeleteObstacles()
    {
        foreach (var env in ReadyEnvironments)
        {
            var obstracles = env.gameObject.transform.Find("Obstracles");
            for (int j = 0; j < obstracles.transform.childCount; j++)
            {
                Destroy(obstracles.transform.GetChild(j).gameObject);
            }
        }
    }

    public void CreateBooster()
    {
        if (ReadyEnvironments.Count == 0)
            return;

        GameObject environment = ReadyEnvironments[ReadyEnvironments.Count - 1];

        for (int i = 0; i < environment.transform.childCount; i++)
        {
            if (environment.transform.GetChild(i).tag == "Booster") // если на участке дороги уже есть бустре
                return;
        }

        GameObject booster = GetRandomBooster();
        var envPosition = environment.transform.position;

        var deltaX = Random.Range(-distanceBetweenEnvironments / 4, distanceBetweenEnvironments / 4);
        var deltaY = Random.Range(0, 4);
        var position = new Vector3(envPosition.x + deltaX, envPosition.y + deltaY, envPosition.z);

        var obj = Instantiate(booster, position, Quaternion.identity);
        obj.tag = "Booster";
        obj.transform.SetParent(environment.transform);
    }

    public void DeleteBoosters()
    {
        foreach (var env in ReadyEnvironments)
        {
            for (int i = 0; i < env.transform.childCount; i++)
            {
                if (env.transform.GetChild(i).tag == "Booster")
                    Destroy(env.transform.GetChild(i).gameObject);
            }
        }
    }
}
