using UnityEngine;
using System.Collections;

public class AudioFader : MonoBehaviour {

    private AudioSource source;

    private float fadeDuration;
    private float fadeCurrent;
    private float volume;
    private bool fadingOut, fadingIn;


    void Start() {
        source = GetComponent<AudioSource>();
    }

    public void FadeOut(float duration) {
        volume = source.volume;

        fadeDuration = duration;
        fadeCurrent = duration;
        fadingOut = true;
        fadingIn = false;
    }

    private void DoFadeOut() {
        if (!fadingOut)
            return;
        if (fadeCurrent > 0) {
            fadeCurrent -= Time.deltaTime;
            source.volume = volume * fadeCurrent / fadeDuration;
        } else {
            source.volume = 0;
            //source.Stop();
            fadingOut = false;
        }
    }

    void Update() {
        DoFadeOut();
        //DoFadeIn(); // not implemented yet
    }


    // TODO: not implemented yet
    //public void FadeIn(float duration, float targetVolume) {
    //    this.volume = targetVolume;
    //    FadeIn(duration);
    //}

    //public void FadeIn(float duration) {
    //    fadeDuration = duration;
    //    fadeCurrent = 0;

    //    if (!source.isPlaying)
    //        source.volume = 0;

    //    fadingOut = false;
    //    fadingIn = true;
    //}

    //private void DoFadeIn() {
    //    if (fadeCurrent < fadeDuration) {

    //    } else {
    //        fadingIn = false;
    //    }
    //}
}
