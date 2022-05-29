using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
    public void Scenes(int numberScenes)
    {
        SceneManager.LoadScene(numberScenes); // ÇÀÃĞÓÆÀÅÒ ÑÖÅÍÓ ÏÎ ÈÍÄÅÊÑÓ Â ÏĞÎÅÊÒÅ
    }
    public void SetName(string name) // ÓÑÒÀÍÀÂËÈÂÀÅÌ ÈÌß ÈÃĞÎÊÀ
    {
        DataHolder.Name = name;
    }
}
