using UnityEngine;

public class OpeningController : MonoBehaviour
{
    public Animator OpeningAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void PlayOpening()
    {
        OpeningAnimator.Play("MainMenuOpening");
    }

    public void ResetOpening()
    {
        OpeningAnimator.Play("MainMenuOpeningIdle");
    }

}
