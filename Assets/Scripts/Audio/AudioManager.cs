using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] music, sfx;
    public Sound mainTitle;

    public AudioSource musicSource, sfxSource;

    public bool isLevelMusic;

    public static AudioManager main;

    private void Awake() {
        if (main == null) {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("Start");
        if (isLevelMusic) {
            playLevelMusic();
        }else {
            playMainTheme();
        }
    }

    
    public void playLevelMusic() {
        if (isLevelMusic) {
            musicSource.loop = false;
            int randomIndex = UnityEngine.Random.Range(0, music.Length);
            StartCoroutine(playRandomLevelTrack(randomIndex)); // Starte die Coroutine, um einen zufälligen Track abzuspielen
        }
    }

    private IEnumerator playRandomLevelTrack(int levelMusicIndex)
    {
        Sound selectedTrack = music[levelMusicIndex];

        playMusic(selectedTrack);

        while (musicSource.isPlaying)
        {
            yield return null;
        }

        if (isLevelMusic)
        {
            StartCoroutine(playRandomLevelTrack((levelMusicIndex + 1) % music.Length));
        }
    }

    public void playMainTheme() {
        playMusic(mainTitle);
        musicSource.loop = true;
    }

    private void playMusic(Sound s) {

        if (s == null) {
            Debug.Log("Sound Not Found");
            return;
        }

        musicSource.clip = s.clip;
        musicSource.Play();
    }

    private void playSFX(string name) {
        Sound s = Array.Find(sfx, x => x.name == name);

        if (s == null) {
            Debug.Log("Sound Not Found");
            return;
        }

        sfxSource.clip = s.clip;
        sfxSource.Play();
    }

}
