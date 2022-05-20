using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    public int number;

    public GameObject GetBooster()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            print(transform.GetChild(i).tag);
            if (transform.GetChild(i).tag == "Booster")
                return transform.GetChild(i).gameObject;
        }

        return null;
    }
}
