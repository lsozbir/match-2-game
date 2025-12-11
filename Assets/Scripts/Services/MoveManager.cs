using System;
using System.Collections;
using System.Collections.Generic;
using Boot;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
    public event EventHandler MovesExpired;
    public event EventHandler MovesChanged;
    [Inject] LevelData _levelData;
    private int _remainingMoves;

    private int RemainingMoves
    {
        get => _remainingMoves;
        set
        {
            _remainingMoves = value;
            MovesChanged?.Invoke(this, EventArgs.Empty);
        } 
    }
    
    public int GetMoves()
    {
        return RemainingMoves;
    }

    public void DecrementMoves()
    {
        RemainingMoves = Mathf.Max(0, RemainingMoves - 1);
        if(RemainingMoves == 0) 
            MovesExpired?.Invoke(this, EventArgs.Empty);
    }

    private void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
        RemainingMoves = _levelData.LevelMoveCount;
    }
}
