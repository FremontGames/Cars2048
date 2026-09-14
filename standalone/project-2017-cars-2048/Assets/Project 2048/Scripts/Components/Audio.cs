using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class MusicAudio : MonoBehaviour
{
    public AudioSource AudioSource;

    void Start()
    {
        AudioClip AudioClip = Resources.Load<AudioClip>("Musics/Loyalty_Freak_Music_-_08_-_Yippee_ (online-audio-converter.com)");

        AudioSource.clip = AudioClip;
        AudioSource.loop = true;
        AudioSource.volume = 0.6f;
        AudioSource.Play();
    } 
}

class SoundAudio : MonoBehaviour
{
    public AudioSource AudioSource;
    public bool PlayIntroAtStartup = false;

    AudioClip IntroAudioClip;
    AudioClip MergeAudioClip;
    AudioClip WrongAudioClip;
    AudioClip WinAudioClip;
    AudioClip LossAudioClip;

    void Start()
    {
        MergeAudioClip = Resources.Load<AudioClip>("Sounds/Blop-Mark_DiAngelo-79054334");
        WrongAudioClip = Resources.Load<AudioClip>("Sounds/366103__original-sound__error-wooden");
        IntroAudioClip = Resources.Load<AudioClip>("Sounds/145434__soughtaftersounds__old-music-box-1");
        WinAudioClip = Resources.Load<AudioClip>("Sounds/171670__fins__success-2");
        LossAudioClip = Resources.Load<AudioClip>("Sounds/342756__rhodesmas__failure-01");

        AudioSource.volume = 0.7f;

        if (PlayIntroAtStartup)
            AudioSource.PlayOneShot(IntroAudioClip);
    }

    public void PlayMove()
    {

    }
    public void PlayMerge()
    {
        AudioSource.PlayOneShot(MergeAudioClip);
    }
    public void PlayWrong()
    {
        AudioSource.PlayOneShot(WrongAudioClip);
    }

    public void PlayWin()
    {
        AudioSource.PlayOneShot(WinAudioClip);
    }
    public void PlayLoss()
    {
        AudioSource.PlayOneShot(LossAudioClip);
    }
}
