using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager I { get; private set; }

    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource; // BGM用
    [SerializeField] private AudioSource seSource;  // SE用（PlayOneShotで多重可）
    
    [Header("Audio Clips")]
    public AudioClip buttonClickClip;
    public AudioClip resultClip;
    public AudioClip swipeClip;
    
    [Header("BGM")]
    public AudioClip titleBgmClip;
    public AudioClip inGameBgmClip;

    void Awake()
    {
        if (I && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);
    }

    // ===== BGM =====
    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (!clip) return;
        bgmSource.clip = clip;
        bgmSource.loop = loop;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    // ===== SE =====
    public void PlaySE(AudioClip clip)
    {
        if (!clip) return;
        seSource.PlayOneShot(clip);
    }
}