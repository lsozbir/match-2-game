using System;
using UnityEngine;
using UnityEngine.UI;

namespace Services
{
    // ScreenService class checks if canvas size is changed and invokes related events that are used to scale camera/UI
    public class CanvasService : MonoBehaviour
    {
        [SerializeField] private GameObject mainCanvasGameObject;
        [SerializeField] private GameObject backGroundCanvasGameObject;
        [SerializeField] private GameObject topUIBar;
        [SerializeField] private GameObject bottomUIBar;
        private RectTransform _botUIRectTransform;
        private RectTransform _topUIRectTransform;
        private Canvas _mainCanvas;
        private RectTransform _canvasRectTransform;
        private float _lastCanvasWidth;
        private float _lastCanvasHeight;
        public event EventHandler CanvasSizeChanged;
        public event EventHandler UISizeChanged;

        private void Awake()
        {
            _canvasRectTransform = mainCanvasGameObject.GetComponent<RectTransform>();
            _mainCanvas = mainCanvasGameObject.GetComponent<Canvas>();
            _botUIRectTransform = bottomUIBar.GetComponent<RectTransform>();
            _topUIRectTransform = topUIBar.GetComponent<RectTransform>();
        }

        public void InvokeUISizeChanged(object sender, EventArgs e)
        {
            UISizeChanged?.Invoke(sender, e);
        }

        public Canvas GetMainCanvas()
        {
            return _mainCanvas;
        }

        public void BindCamera(Camera cam)
        {
            var backGroundCanvas = backGroundCanvasGameObject.GetComponent<Canvas>();
            var mainCanvas = mainCanvasGameObject.GetComponent<Canvas>();
            
            mainCanvas.worldCamera = cam;
            backGroundCanvas.worldCamera = cam;
            
            mainCanvas.sortingLayerID = SortingLayer.NameToID("UI");
            backGroundCanvas.sortingLayerID = SortingLayer.NameToID("Background");
        }

        public (float availableHeightPercent, float screenHeightCenterPercent) GetAvailableScreenPercentAndOffset()
        {
            var topRectPixel = RectTransformUtility.PixelAdjustRect(_topUIRectTransform, _mainCanvas);
            var botRectPixel = RectTransformUtility.PixelAdjustRect(_botUIRectTransform, _mainCanvas);
            
            var availableHeight = (Screen.height) - (topRectPixel.height + botRectPixel.height);
            
            return (availableHeight / Screen.height, 
                (botRectPixel.height / Screen.height) + availableHeight / Screen.height / 2);
        }

        private void Update()
        {
            CheckCanvasSizeChange();
        }

        private void CheckCanvasSizeChange()
        {
            if (Mathf.Approximately(_lastCanvasWidth, _canvasRectTransform.rect.width) && 
                Mathf.Approximately(_lastCanvasHeight, _canvasRectTransform.rect
                    .height)) return;
            _lastCanvasWidth = _canvasRectTransform.rect.width;
            _lastCanvasHeight = _canvasRectTransform.rect.height;
            CanvasSizeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
