using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static  AudioClip Gunshot, Sword;

    static AudioSource audioSrc;
    void Start()
    {
        Gunshot = Resources.Load<AudioClip>("gunshot");
        Sword = Resources.Load<AudioClip>("sword");
        audioSrc = GetComponent<AudioSource> ();
        audioSrc.volume = 0.2f;
       
    }

    public static void PlaySound(string clip) {
        switch(clip) {
            case "gunshot":
                audioSrc.PlayOneShot(Gunshot);
                break;
            case "sword":
                audioSrc.PlayOneShot(Sword);
                break;
            default:
                return;
        }
    }
}
