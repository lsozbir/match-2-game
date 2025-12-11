using System.Collections.Generic;
using Boot;
using Cubes;
using Reflex.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static Cubes.CubeType;

namespace UI
{
    public class LevelSelectionView : MonoBehaviour
    {
        [SerializeField] private Button starterLevelButton;
        [SerializeField] private Button duckLandButton;
        [SerializeField] private Button PartyHardButton;
        [SerializeField] private Button ExpertLevelButton;
    
        // Start is called before the first frame update
        private void Awake()
        {
            starterLevelButton.onClick.AddListener((() => LoadGameWithLevelData(CreateStarterLevelData())));
            duckLandButton.onClick.AddListener((() => LoadGameWithLevelData(CreateDuckLandLevelData())));
            PartyHardButton.onClick.AddListener((() => LoadGameWithLevelData(CreatePartyHardLevelData())));
            ExpertLevelButton.onClick.AddListener((() => LoadGameWithLevelData(CreateExpertLevelData())));
        }
    
        private void LoadGameWithLevelData(LevelData levelData)
        {
            void InstallExtra(Scene scene, ContainerBuilder builder)
            {
                builder.AddSingleton(levelData, typeof(LevelData));
            }
            
            SceneScope.OnSceneContainerBuilding += InstallExtra;
        
            SceneManager.LoadSceneAsync("Game")!.completed += operation =>
            {
                SceneScope.OnSceneContainerBuilding -= InstallExtra;
            };
        }
    
        private LevelData CreateStarterLevelData()
        {
            return new LevelData(
                levelStartingGrid: new CubeType[,]
                {
                    {Red, Red, Red, Red, Red, Red, Red},
                    {Green, Green, Green, Green, Green, Green, Green},
                    {Red, Red, Red, Red, Red, Red, Red},
                    {Green, Green, Green, Green, Green, Green, Green},
                    {Red, Red, Red, Red, Red, Red, Red},
                    {Green, Green, Green, Green, Green, Green, Green},
                    {Red, Red, Red, Red, Red, Red, Red},
                },
                levelMoveCount: 20,
                levelSpawnableCubeTypes: new CubeType[] {Red, Green},
                levelGoal: new Dictionary<CubeType, int>()
                {
                    {Red, 24},
                    {Green, 24}
                });
        }
    
        private LevelData CreateDuckLandLevelData()
        {
            return new LevelData(
                levelStartingGrid: new CubeType[,]
                {
                    {Duck, Duck, Duck, Duck, Duck, Duck, Duck, Duck, Duck},
                    {Duck, Duck, Duck, Duck, Duck, Duck, Duck, Duck, Duck},
                    {Duck, Duck, Duck, Duck, Duck, Duck, Duck, Duck, Duck},
                    {Duck, Duck, Duck, Duck, Duck, Duck, Duck, Duck, Duck},
                    {Duck, Duck, Duck, Duck, Blue, Blue, Blue, Blue, Blue},
                    {Duck, Duck, Duck, Red, Red, Red, Red, Red, Red},
                    {Duck, Duck, Blue, Blue, Blue, Blue, Blue, Blue, Blue},
                    {Duck, Red, Red, Red, Red, Red, Red, Red, Red},
                    {Blue, Blue, Blue, Blue, Blue, Blue, Blue, Blue, Blue},
                },
                levelMoveCount: 20,
                levelSpawnableCubeTypes: new CubeType[] {Blue, Red, Duck},
                levelGoal: new Dictionary<CubeType, int>()
                {
                    {Duck, 40}
                });
        }
    
        private LevelData CreatePartyHardLevelData()
        {
            return new LevelData(
                levelStartingGrid: new CubeType[,]
                {
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                    {Balloon, Red, Balloon, Balloon, Balloon, Balloon, Balloon, Red, Balloon},
                },
                levelMoveCount: 30,
                levelSpawnableCubeTypes: new CubeType[] {Red, Balloon, Balloon},
                levelGoal: new Dictionary<CubeType, int>()
                {
                    {Balloon, 120}
                });
        }
        
        private LevelData CreateExpertLevelData()
        {
            return new LevelData(
                levelStartingGrid: new CubeType[,]
                {
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                    {Green, Green, Green, Green, Green, Green, Green, Green, Green , Green, Green, Green, Green, Green},
                },
                levelMoveCount: 15,
                levelSpawnableCubeTypes: new CubeType[] {Red, Red, Yellow, Yellow, Purple, Purple, Duck, Balloon},
                levelGoal: new Dictionary<CubeType, int>()
                {
                    {Duck, 20},
                    {Balloon, 20},
                    {Red, 20}
                });
        }
    }
}
