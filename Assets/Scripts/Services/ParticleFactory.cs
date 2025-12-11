using System.Collections;
using System.Collections.Generic;
using Cubes;
using UnityEngine;

public class ParticleFactory : MonoBehaviour
{
    [SerializeField] private GameObject cubeExplosionParticlePrefab;
    // Used to determine colors in particle system
    private Dictionary<CubeType, Color> _cubeColorMap;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Awake()
    {
        _cubeColorMap = new Dictionary<CubeType, Color>
        {
            { CubeType.Red, Color.red },
            { CubeType.Blue, Color.blue },
            { CubeType.Green, Color.green },
            { CubeType.Yellow, Color.yellow },
            { CubeType.Purple, new Color(0.5f, 0f, 0.5f, 1f) }
        };
    }
    
    private Color GetColorOfCubeType(CubeType cubeType)
    {
        if(_cubeColorMap.TryGetValue(cubeType, out var value)) return value;
        return new Color(0f, 0f, 0f, 1f);
    }

    public void CreateCubeExplosionParticle(CubeType cubeType, Vector3 position)
    {
        var particleInstance = Instantiate(cubeExplosionParticlePrefab, position, Quaternion.identity);
        var ps = particleInstance.GetComponent<ParticleSystem>();
        var main = ps.main;
        main.startColor = _cubeColorMap[cubeType];
        Destroy(particleInstance, ps.main.duration);
    }
}
