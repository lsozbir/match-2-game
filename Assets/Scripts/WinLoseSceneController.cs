using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinLoseSceneController : MonoBehaviour
{
    private float _elapsedTime;
    
    void Update()
    {
        _elapsedTime += Time.deltaTime;
        if (_elapsedTime > 3f)
            SceneManager.LoadScene("Boot");
    }
}
