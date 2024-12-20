using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Global Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambienceSource;
    [SerializeField] private AudioSource sfx2DSource;
    

    [Header("Audio Source Pool for 3D Clips")]
    [SerializeField] private GameObject audioSourcePrefab; 
    [SerializeField] private int poolSize = 10;

    private Queue<AudioSource> audioSourcePool;

    private void Awake()
    {
        // Basic Singleton Setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSourcePool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    
    // Creates a pool of AudioSources objects for 3D playback.
    private void InitializeAudioSourcePool()
    {
        audioSourcePool = new Queue<AudioSource>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject sourceGO = Instantiate(audioSourcePrefab, transform);
            AudioSource source = sourceGO.GetComponent<AudioSource>();
            sourceGO.SetActive(false);
            audioSourcePool.Enqueue(source);
        }
    }

    
    // Plays single 2D sound effect (not positional).
    public void Play2D(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfx2DSource == null) return;
        sfx2DSource.PlayOneShot(clip, volume);
    }

    
    // Plays a 3D sound at the specified world position or attached to a parent object.
    public void Play3D(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource source = GetAvailableAudioSource();
        source.transform.position = position;
        source.volume = volume;
        source.spatialBlend = 1f;     // Full 3D
        source.loop = false;
        source.clip = clip;
        source.gameObject.SetActive(true);
        source.Play();
        StartCoroutine(DeactivateAfterPlayback(source));
    }

    
    //Plays a 3D sound attached to a parent transform (follows its postion)
    public void Play3D(AudioClip clip, Transform parent, float volume = 1f)
    {
        if (clip == null || parent == null) return;
        AudioSource source = GetAvailableAudioSource();
        source.transform.SetParent(parent);
        source.transform.localPosition = Vector3.zero;
        source.volume = volume;
        source.spatialBlend = 1f;     // Full 3D
        source.loop = false;
        source.clip = clip;
        source.gameObject.SetActive(true);
        source.Play();
        StartCoroutine(DeactivateAfterPlayback(source));
    }

    public AudioSource Play3DLooping(AudioClip clip, Transform parent, float volume = 1f)
    {
        // Similar to Play3D but sets loop = true and returns the source
        AudioSource source = GetAvailableAudioSource();
        source.transform.SetParent(parent);
        source.transform.localPosition = Vector3.zero;
        source.volume = volume;
        source.spatialBlend = 1f;
        source.loop = true;
        source.clip = clip;
        source.gameObject.SetActive(true);
        source.Play();
        return source;
    }
    
    public void StopLoopingAudio(AudioSource source)
    {
        if (source != null)
        {
            source.Stop();
            source.clip = null;
            source.loop = false;
            source.gameObject.SetActive(false);
            source.transform.SetParent(transform);
        }
    }

   
    public void PlayMusic(AudioClip musicClip, float volume = 1f, bool loop = true)
    {
        if (musicClip == null || musicSource == null) return;
        musicSource.clip = musicClip;
        musicSource.volume = volume;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void PlayAmbience(AudioClip ambienceClip, float volume = 1f, bool loop = true)
    {
        if (ambienceSource == null || ambienceClip == null) return;
        ambienceSource.clip = ambienceClip;
        ambienceSource.volume = volume;
        ambienceSource.loop = loop;
        ambienceSource.Play();
    }

    private AudioSource GetAvailableAudioSource()
    {
        // If all sources are in use, reuse the oldest
        AudioSource source = audioSourcePool.Dequeue();
        // Reset parent & position
        source.transform.SetParent(transform);
        source.transform.localPosition = Vector3.zero;
        source.gameObject.SetActive(false);
        audioSourcePool.Enqueue(source);
        return source;
    }

    
   
    //Coroutine to deactivate AudioSource after finished playing.
    private System.Collections.IEnumerator DeactivateAfterPlayback(AudioSource source)
    {
        yield return new WaitUntil(() => !source.isPlaying);
        source.gameObject.SetActive(false);
        source.clip = null;
        source.transform.SetParent(transform);  // Reset parent to AudioManager
    }
}
