using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {
    public List<AudioClip> hitSandClips = new List<AudioClip> ();
    public List<AudioClip> explosionClips = new List<AudioClip> ();
    public List<AudioClip> getDamageClips = new List<AudioClip> ();
    public List<AudioClip> getSmallBonusClips = new List<AudioClip> ();
    public List<AudioClip> getScrollClips = new List<AudioClip> ();
    public List<AudioClip> winClips = new List<AudioClip> ();
    public List<AudioClip> loseClips = new List<AudioClip> ();
    public List<AudioClip> jumpClips = new List<AudioClip> ();
    public List<AudioClip> sayClips = new List<AudioClip> ();

    public AudioSource source;

    public void Play (List<AudioClip> oneOf) {
        source.PlayOneShot (oneOf[Random.Range (0, oneOf.Count)]);
    }
}