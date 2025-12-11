using System;
using System.Collections;
using System.Collections.Generic;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MoveView : MonoBehaviour
{
    [SerializeField] private TMP_Text remainingMovesText;
    [Inject] private MoveManager _moveManager;
    
    private void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
        _moveManager.MovesChanged += OnMovesChanged;
        UpdateMoveText();
    }

    private void OnDestroy()
    {
        _moveManager.MovesChanged -= OnMovesChanged;
    }

    private void OnMovesChanged(object sender, EventArgs eventArgs)
    {
        UpdateMoveText();
    }

    private void UpdateMoveText()
    {
        remainingMovesText.text = _moveManager.GetMoves().ToString();
    }
}
