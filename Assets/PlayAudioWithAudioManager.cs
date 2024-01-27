using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioWithAudioManager : MonoBehaviour
{
    public void PlayClip(AudioClip clip)
    {
        AudioManager.instance.Play(clip);
    }
}
