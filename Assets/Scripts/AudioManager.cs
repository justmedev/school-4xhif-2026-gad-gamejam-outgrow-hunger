using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> harvestClips;
    [SerializeField] private List<AudioClip> plantClips;
    [SerializeField] private List<AudioClip> stepClips;
    [SerializeField] private AudioClip badNightClip;
    [SerializeField] private AudioClip midNightClip;
    [SerializeField] private AudioClip goodNightClip;
    [SerializeField] private AudioClip backgroundMusic;
    private AudioSource _audioSource;

    public AudioClip BadNightClip => badNightClip;
    public AudioClip MidNightClip => midNightClip;
    public AudioClip GoodNightClip => goodNightClip;

    public void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.clip = backgroundMusic;
        _audioSource.loop = true;
        _audioSource.Play();
        // AudioManager is also responsible for loading audio clips
        DontDestroyOnLoad(this);
    }

    private void OnDestroy()
    {
        harvestClips.ForEach(clip => clip.UnloadAudioData());
    }

    public AudioClip GetRandomHarvestClip() => GetRandomClip(harvestClips);
    public AudioClip GetRandomPlantClip() => GetRandomClip(plantClips);
    public AudioClip GetRandomStepClip() => GetRandomClip(stepClips);

    private static AudioClip GetRandomClip([NotNull] List<AudioClip> clips)
    {
        return clips[Random.Range(0, clips.Count - 1)];
    }

    public void DuckBackgroundMusic()
    {
        _audioSource.volume = .2f;
    }

    public void UnDuckBackgroundMusic()
    {
        _audioSource.volume = 1f;
    }

    public void LoadAllAudioClips()
    {
        harvestClips.ForEach(clip => clip.LoadAudioData());
        plantClips.ForEach(clip => clip.LoadAudioData());
        stepClips.ForEach(clip => clip.LoadAudioData());
        badNightClip.LoadAudioData();
        midNightClip.LoadAudioData();
        goodNightClip.LoadAudioData();
    }
}