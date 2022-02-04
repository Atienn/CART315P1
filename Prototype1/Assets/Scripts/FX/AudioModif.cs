using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioModif : Singleton<AudioModif>
{
    public AudioSource modifies;
    public AudioMixerGroup modifier;
    public float duration;

    public void AddAudioModif() {
        //StopAllCoroutines();
        modifies.outputAudioMixerGroup = modifier;
        //StartCoroutine(WaitForRemove());
    }

    //IEnumerator<WaitForSeconds> WaitForRemove() {
    //    yield return new WaitForSeconds(duration);
    //    RemoveAudioModif();
    //}

    public void RemoveAudioModif() {
        modifies.outputAudioMixerGroup = null;
    }
}
