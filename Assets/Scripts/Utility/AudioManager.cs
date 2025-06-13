using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource sfxSource;
    [Header("효과음")]
    public AudioClip effectSound;
    public AudioClip blockDropSound;
    public AudioClip simulationStartSound;
    public AudioClip errorSound;

    void Awake()
    {
        // 2) 인스턴스 중복 방지
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // 씬 전환에도 파괴되지 않도록
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public void PlayEffectSound()
    {
        if (effectSound == null) return;
        sfxSource.PlayOneShot(effectSound);
    }

    public void PlayBlockDropSound()
    {
        if (effectSound == null) return;
        sfxSource.PlayOneShot(effectSound);
    }

    public void PlaySimulationStartSound()
    {
        if (simulationStartSound == null) return;
        for (int i = 0; i < 5; ++i)
            sfxSource.PlayOneShot(simulationStartSound);
    }

    public void PlayErrorSound()
    {
        if (errorSound == null) return;
        sfxSource.PlayOneShot(errorSound);
    }
}