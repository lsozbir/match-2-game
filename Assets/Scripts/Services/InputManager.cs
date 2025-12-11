using System;
using Cubes;
using Grid;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [Inject] private CameraController _cameraController;
    [Inject] private GridController _gridController;

    private void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && _gridController.AnimationCount == 0)
        {
            Vector2 mousePos = _cameraController.GetCamera().ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePos, Vector2.zero);
            if (!hit.collider) return;
            
            if (hit.collider.gameObject.TryGetComponent<Cube>(out var cube))
            {
                cube.OnCubeClicked();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Boot");
        }
    }
}