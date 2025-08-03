using UnityEngine;

public class RulerController : MonoBehaviour
{
    public Animator RulerAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void RulerIdle()
    {
        RulerAnimator.Play("RulerIdle");
    }
    public void RulerPlay()
    {
        RulerAnimator.Play("RulerPlay");
    }

}
