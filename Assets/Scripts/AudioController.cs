using ManagerDomain;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSourceBGM;
    public AudioSource audioSourceSoundEffect;

    public AudioClip audioClipMenuBGM;
    public AudioClip audioClipMainGameBGM;
    public AudioClip audioClipGameOverBGM;

    public AudioClip audioClipCutHair;
    public AudioClip audioClipSuccess;
    public AudioClip audioClipFail;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UIManager.Instance.OnCut += PlaySoundEffect_CutHair;
    }

    private void OnDestroy()
    {
        UIManager.Instance.OnCut -= PlaySoundEffect_CutHair;
    }

    public void PlayBGM()
    {
        audioSourceBGM.Play();
    }

    public void StopBGM()
    {
        audioSourceBGM.Stop();
    }

    public void PlayMenuBGM()
    {
        audioSourceBGM.resource = audioClipMenuBGM;
        audioSourceBGM.Play();
    }

    public void PlayMainGameBGM()
    {
        audioSourceBGM.resource = audioClipMainGameBGM;
        audioSourceBGM.Play();
    }

    public void PlayGameOverBGM()
    {
        // Amo.Instance.Log($"Playing game over bgm. {audioClipGameOverBGM}");
        audioSourceBGM.resource = audioClipGameOverBGM;
        audioSourceBGM.Play();
    }

    public void PlaySoundEffect_CutHair()
    {
        audioSourceSoundEffect.PlayOneShot(audioClipCutHair);
    }
    public void PlaySoundEffect_Success()
    {
        audioSourceSoundEffect.PlayOneShot(audioClipSuccess);
    }
    public void PlaySoundEffect_Fail()
    {
        audioSourceSoundEffect.PlayOneShot(audioClipFail);
    }

}