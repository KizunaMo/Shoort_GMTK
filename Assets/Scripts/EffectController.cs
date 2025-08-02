using UnityEngine;

public class EffectController : MonoBehaviour
{
    public GameObject CutHairEffectObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    public void ShowCutHairEffect()
    {
        Instantiate(CutHairEffectObject, Vector2.zero, Quaternion.identity);
    }
}
