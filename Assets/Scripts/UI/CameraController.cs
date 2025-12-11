using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Services;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Inject] private GridController _gridController;
    [Inject] private CanvasService _canvasService;
    private Camera _camera;
    
    // How much offset there should be from screen borders and UI (0 - 1)
    private const float AvailableSpaceOffset = 0.98f;
    
    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    public Camera GetCamera()
    {
        return _camera;
    }

    private void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
        
        _canvasService.UISizeChanged += OnUISizeChanged;
        
        // Assign this camera to Screen-Space-Cameras in Canvases
        _canvasService.BindCamera(_camera);
    }

    private void OnDestroy()
    {
        _canvasService.UISizeChanged -= OnUISizeChanged;
    }
    
    private void OnUISizeChanged(object sender, EventArgs eventArgs)
    {
        FitGridToScreen();
    }

    // Resize and move orthographic camera so it perfectly fits the grid into the available space in screen.
    private void FitGridToScreen()
    {
        var gridBounds = _gridController.gridWrapper.GetComponent<Renderer>().bounds;
        var (heightPercent, screenHeightCenterPercent) = _canvasService.GetAvailableScreenPercentAndOffset();
        
        var aspectRatio = (float) Screen.width / Screen.height;
        var verticalSize = gridBounds.size.y / (2f * heightPercent * AvailableSpaceOffset);
        var horizontalSize = (gridBounds.size.x / aspectRatio) / (2f * AvailableSpaceOffset);
        _camera.orthographicSize = Mathf.Max(verticalSize, horizontalSize);
        
        var offsetFromCenter = (screenHeightCenterPercent - 0.5f) * (2f * _camera.orthographicSize);
        var newCameraPos = _camera.transform.position;
        newCameraPos.y = gridBounds.center.y - offsetFromCenter;
        _camera.transform.position = newCameraPos;
    }
}
