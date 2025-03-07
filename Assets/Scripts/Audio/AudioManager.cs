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

    private float musicVolume = 1f;
    private float sfxVolume = 1f;


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

    public void setMusicVolume(float vol) {
        this.musicVolume = vol;
        musicSource.volume = vol;
    }
    public float getMusicVolume() {
        return this.musicVolume;
    }
    public void setSFXVolume(float vol) {
        this.sfxVolume = vol;
        sfxSource.volume = vol;
    }
    public float getSFXVolume() {
        return this.sfxVolume;
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

        //sfxSource.clip = s.clip;
        sfxSource.PlayOneShot(s.clip);
    }

    public void playSFX(Sound s) {
        if (s == null) {
            Debug.Log("Sound Not Found");
            return;
        }

        //sfxSource.clip = s.clip;
        sfxSource.PlayOneShot(s.clip);
    }

    public void playButtonClick() {
        playSFX("button_click_1");
    }

    public void playCashSound() {
        playSFX("cash_register");
    }

    public void playUpgradeSound() {
        playSFX("upgrade_sound");
    }

    public void playDidntWorkSound() {
        playSFX("didnt_work");
    }

    public void playLostGameSound() {
        playSFX("lost_game_sound");
    }

    public void playGameWinSound() {
        playSFX("game_win");
    } 

    public void playWaveCompleteSound() {
        playSFX("wave_complete");
    } 

    public void playWaveStartSound() {
        playSFX("wave_start_1");
        playSFX("wave_start_2");
    }
    public void playEnemyDead() {
        playSFX("enemy_dead");
    }

}
