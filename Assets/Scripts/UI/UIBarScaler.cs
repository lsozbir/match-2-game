using System;
using System.Collections;
using System.Collections.Generic;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Services;
using UnityEngine;
using UnityEngine.UI;

public class UIBarScaler : MonoBehaviour
{
    [Range(0f, 1f)] public float screenPercentage = 0.5f;
    [Inject] private CanvasService _canvasService;
    private RectTransform _rectTransform;
    private Image _image;

    void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    private void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
        _canvasService.CanvasSizeChanged += OnCanvasSizeChanged;
    }

    private void OnDestroy()
    {
        _canvasService.CanvasSizeChanged -= OnCanvasSizeChanged;
    }

    private void OnCanvasSizeChanged(object sender, EventArgs eventArgs)
    {
        ApplyScaling();
    }

    // Scale the UI without ruining aspect ratio or going out of bounds of the screen
    private void ApplyScaling()
    {
        var targetSize = Screen.height * screenPercentage;
        var imageAspect = _image.sprite.rect.width / _image.sprite.rect.height;
        var newSize = new Vector2(targetSize * imageAspect, targetSize);

        // If either width or height is larger than screen, scale it down proportionally
        var widthRatio = Screen.width / newSize.x;
        var heightRatio = Screen.height / newSize.y;
        var scaleFactor = Mathf.Min(1f, widthRatio, heightRatio);

        newSize *= scaleFactor;

        _rectTransform.sizeDelta = newSize;
        
        _canvasService.InvokeUISizeChanged(this, EventArgs.Empty);
    }
}
