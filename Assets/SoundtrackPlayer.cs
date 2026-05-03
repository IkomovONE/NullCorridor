using UnityEngine;

//This class handles the soundtracks of the game.
public class RandomMusicPlayer : MonoBehaviour
{
    public AudioClip[] tracks;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayRandomTrack();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayRandomTrack();
        }
    }

    void PlayRandomTrack()
    {
        int index = Random.Range(0, tracks.Length);
        audioSource.clip = tracks[index];
        audioSource.Play();
    }
}