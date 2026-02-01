using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public AudioClip OnCardPickup;
    public AudioClip OnCardDrop;
    
    public static AudioPlayer Instance;
    private AudioSource audioSource;

    private void Awake()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }
}
