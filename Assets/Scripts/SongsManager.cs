using UnityEngine;

public class SongsManager : MonoBehaviour
{
    public static SongsManager instance;
    
    public AudioSource audioSource;

    [SerializeField]
    private AudioClip[] songs = null;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = songs[Random.Range(0, songs.Length)];
            audioSource.Play();
        }
    }
}
