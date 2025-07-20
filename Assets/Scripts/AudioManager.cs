using UnityEngine;


public enum SoundType
{
    Flip,
    Match,
    Fail
}
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    public AudioSource audioSource;
    public AudioClip flipSound, matchSound, failSound;

    private void Awake()
    {
        Instance = this;
    }

    public void Play(SoundType soundType)
    {
        switch (soundType)
        {
           case SoundType.Flip:
                audioSource.PlayOneShot(flipSound);
                break;
            case SoundType.Match:
                audioSource.PlayOneShot(matchSound);
                break;
            case SoundType.Fail:
                audioSource.PlayOneShot(failSound);
                break;
            default:
                Debug.LogWarning("Unknown sound type: " + soundType);
                break;
        }
    }
}
