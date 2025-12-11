using System.Collections;
using System.Collections.Generic;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GoalElementView : MonoBehaviour
{
    [SerializeField] private GameObject goalElementGameObject;
    [SerializeField] private Image goalElementImage;
    [SerializeField] private TMP_Text goalElementText;
    [Inject] private CameraController _cameraController;
    private RectTransform _goalElementRectTransform;

    private void Awake()
    {
        _goalElementRectTransform = goalElementGameObject.GetComponent<RectTransform>();
    }

    private void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
    }

    public void SetGoalText(int goalCount)
    {
        this.goalElementText.text = goalCount.ToString();
    }

    public void SetGoalImage(Sprite sprite)
    {
        goalElementImage.sprite = sprite;
    }

    public Vector3 GetGoalElementWorldPosition()
    {
        Vector3 screenPoint = RectTransformUtility.WorldToScreenPoint(
            _cameraController.GetCamera(), _goalElementRectTransform.position);

        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
                _goalElementRectTransform,
                screenPoint,
                _cameraController.GetCamera(),
                out Vector3 worldPos))
        {
            return worldPos;
        }

        Debug.LogWarning("Failed to get world position of UI element.");
        return Vector3.zero;
    }
}
