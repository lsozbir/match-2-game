using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BottomUIView : MonoBehaviour
{
    [SerializeField] Button mainMenuButton;
    
    void Awake()
    {
        mainMenuButton.onClick.AddListener((() => SceneManager.LoadScene("Boot")));
    }
    
}
