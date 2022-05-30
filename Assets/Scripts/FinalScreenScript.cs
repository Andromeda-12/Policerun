using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalScreenScript : MonoBehaviour
{
    // Start is called before the first frame update
    public void GoMenuButtonClick(string scene)
    {
        SceneManager.LoadScene(scene); // ÇÀÃĞÓÆÀÅÌ ÑÖÅÍÓ ÏÎ ÍÀÇÂÀÍÈŞ (ÇÀÃĞÓÇÈÒ ÒÎËÜÊÎ ÃËÀÂÍÎÅ ÌÅÍŞ)
    }
}
