using System.Collections.Generic;
using Cubes;
using DG.Tweening;
using Grid;
using Reflex.Core;
using Services;
using UnityEngine;

namespace DI
{
    public class GameInstaller : MonoBehaviour, IInstaller
    {
        [SerializeField] private GameObject gameCameraPrefab;
        [SerializeField] private GameObject gameUICanvasPrefab;
        [SerializeField] private GameObject cubeFactoryPrefab;
        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private GameObject inputManagerPrefab;
        [SerializeField] private GameObject goalManagerPrefab;
        [SerializeField] private GameObject moveManagerPrefab;
        [SerializeField] private GameObject audioManagerPrefab;
        [SerializeField] private GameObject particleFactoryPrefab;
        [SerializeField] private GameObject gameManagerPrefab;
        
        public void InstallBindings(ContainerBuilder builder)
        {
            DOTween.SetTweensCapacity(500, 100);
            var gameCamera = Instantiate(gameCameraPrefab);
            builder.AddSingleton(gameCamera.GetComponent<CameraController>(), typeof(CameraController));
            
            var cubeFactory = Instantiate(cubeFactoryPrefab);
            builder.AddSingleton(cubeFactory.GetComponent<CubeFactory>(), typeof(CubeFactory));
            
            var gameUI = Instantiate(gameUICanvasPrefab);
            builder.AddSingleton(gameUI.GetComponent<CanvasService>(), typeof(CanvasService));
            builder.AddSingleton(gameUI.GetComponent<MoveView>(), typeof(MoveView));
            builder.AddSingleton(gameUI.GetComponent<GoalView>(), typeof(GoalView));
            
            var gridController = Instantiate(gridPrefab);
            builder.AddSingleton(gridController.GetComponent<GridController>(), typeof(GridController));
            
            var inputManager = Instantiate(inputManagerPrefab);
            builder.AddSingleton(inputManager.GetComponent<InputManager>(), typeof(InputManager));
            
            var goalManager = Instantiate(goalManagerPrefab);
            builder.AddSingleton(goalManager.GetComponent<GoalManager>(), typeof(GoalManager));
            
            var moveManager = Instantiate(moveManagerPrefab);
            builder.AddSingleton(moveManager.GetComponent<MoveManager>(), typeof(MoveManager));
            
            var audioManager = Instantiate(audioManagerPrefab);
            builder.AddSingleton(audioManager.GetComponent<AudioManager>(), typeof(AudioManager));
            
            var particleFactory = Instantiate(particleFactoryPrefab);
            builder.AddSingleton(particleFactory.GetComponent<ParticleFactory>(), typeof(ParticleFactory));
            
            var gameManager = Instantiate(gameManagerPrefab);
            builder.AddSingleton(gameManager.GetComponent<GameManager>(), typeof(GameManager));
            
            builder.AddSingleton(typeof(GridData), typeof(GridData));
        }
    }
}
