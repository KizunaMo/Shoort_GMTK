using UnityEngine;

public class AudioController : MonoBehaviour
{
    
    public AudioSource audioSourceBGM;
    public AudioSource audioSourceSoundEffect;

    public AudioClip audioClipCutHair;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    public void PlayBGM()
    {
        audioSourceBGM.Play();
    }
    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }

    public void PlaySoundEffect_CutHair()
    {
        audioSourceSoundEffect.PlayOneShot(audioClipCutHair);
    }


}
