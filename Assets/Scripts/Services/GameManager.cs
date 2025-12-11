using System;
using System.Collections;
using System.Collections.Generic;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Inject] private GoalManager _goalManager;
    [Inject] private MoveManager _moveManager;
    
    void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
        _goalManager.GoalReached += OnGoalReached;
        _moveManager.MovesExpired += OnMovesExpired;
    }

    private void OnDestroy()
    {
        _goalManager.GoalReached -= OnGoalReached;
        _moveManager.MovesExpired -= OnMovesExpired;
    }

    private void OnGoalReached(object sender, EventArgs eventArgs)
    {
        LoadWinScene();
    }

    private void OnMovesExpired(object sender, EventArgs eventArgs)
    {
        LoadLoseScene();
    }

    private void LoadWinScene()
    {
        SceneManager.LoadScene("Win");
    }

    private void LoadLoseScene()
    {
        SceneManager.LoadScene("Lose");
    }
}
