using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private const float AudioCooldown = 0.05f;
    [SerializeField] private AudioClip cubeCollectSound;
    [SerializeField] private AudioClip cubeExplodeSound;
    [SerializeField] private AudioClip duckSound;
    [SerializeField] private AudioClip balloonSound;
    [SerializeField] private AudioSource audioSource;
    [Inject] private CameraController _cameraController;
    private Dictionary<AudioClip, float> _timeSinceAudioClipPlayed;

    private void Awake()
    {
        _timeSinceAudioClipPlayed = new Dictionary<AudioClip, float>()
        {
            { cubeCollectSound, 0f },
            { cubeExplodeSound, 0f },
            { duckSound, 0f },
            { balloonSound, 0f }
        };
    }

    private void Start()
    {
        AttributeInjector.Inject(this, gameObject.scene.GetSceneContainer());
    }

    private float PlaySound(AudioClip audioClip)
    {
        if (_timeSinceAudioClipPlayed[audioClip] < AudioCooldown) return audioClip.length;
        _timeSinceAudioClipPlayed[audioClip] = 0f;
        if(audioSource) audioSource.PlayOneShot(audioClip);
        return audioClip.length;
    }

    public float PlayCubeCollectSound()
    {
        return PlaySound(cubeCollectSound);
    }

    public float PlayCubeExplodeSound()
    {
        return PlaySound(cubeExplodeSound);
    }
    
    public float PlayDuckSound()
    {
        return PlaySound(duckSound);
    }

    public float PlayBalloonSound()
    {
        return PlaySound(balloonSound);
    }
    
    private void Update()
    {
        var keys = _timeSinceAudioClipPlayed.Keys.ToArray();
        foreach (var key in keys)
        {
            _timeSinceAudioClipPlayed[key] += Time.deltaTime;
        }
    }
}
