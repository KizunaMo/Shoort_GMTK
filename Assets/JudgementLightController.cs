using UnityEngine;

public class JudgementLightController : MonoBehaviour
{
    public Animator JudgementLight;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void JudgementLight_Idle()
    {
        JudgementLight.Play("JudgementLight_Idle");
    }
    public void SuccessLight()
    {
        JudgementLight.Play("SuccessLight");
    }
    public void FailLight()
    {
        JudgementLight.Play("FailLight");
    }
}
