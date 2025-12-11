using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cubes;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Services;
using UnityEngine;

public class GoalView : MonoBehaviour
{
    [SerializeField] private GameObject goalArea;
    [SerializeField] private GameObject goalElementPrefab;
    [Inject] private CubeFactory _cubeFactory;
    [Inject] private CameraController _cameraController;
    private Dictionary<CubeType, GoalElementView> _goalElementDictionary;

    
    private void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
    }

    public void InitializeGoalView(Dictionary<CubeType, int> goal)
    {
        _goalElementDictionary = new Dictionary<CubeType, GoalElementView>();
        
        foreach (var kvp in goal)
        {
            var goalElementView = 
                Instantiate(goalElementPrefab, goalArea.transform, false).GetComponent<GoalElementView>();
            goalElementView.SetGoalImage(_cubeFactory.ReturnSpriteOfType(kvp.Key));
            goalElementView.SetGoalText(kvp.Value);
            _goalElementDictionary.Add(kvp.Key, goalElementView);
        }
    }

    public void SetGoalTextOfType(CubeType goalCubeType, int goalCount)
    {
        if (_goalElementDictionary.ContainsKey(goalCubeType))
        {
            _goalElementDictionary[goalCubeType].SetGoalText(goalCount);
        }
    }

    public Vector3 GetPositionOfGoalElementFromType(CubeType goalCubeType)
    {
        return _goalElementDictionary.ContainsKey(goalCubeType) ? 
            _goalElementDictionary[goalCubeType].GetGoalElementWorldPosition() : Vector3.zero;
    }
}
