using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Boot;
using Cubes;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

public class GoalManager : MonoBehaviour
{
    private Dictionary<CubeType, int> _goal;
    [Inject] private GoalView _goalView;
    [Inject] private LevelData _levelData;
    public event EventHandler GoalReached; 

    public void SubtractFromGoal(CubeType t)
    {
        if(!_goal.ContainsKey(t)) return;
        
        _goal[t] = Mathf.Max(0, _goal[t] - 1);
        _goalView.SetGoalTextOfType(t, _goal[t]);

        if (IsGoalReached())
        {
            GoalReached?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool IsGoalReached()
    {
        return _goal.All(kvp => kvp.Value <= 0);
    }

    private void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
        _goal = _levelData.LevelGoal;
        _goalView.InitializeGoalView(_goal);
    }

    public bool CanSubtractFromGoal(CubeType cubeType)
    {
        if(!_goal.ContainsKey(cubeType)) return false;
        return _goal[cubeType] > 0;
    }
}
