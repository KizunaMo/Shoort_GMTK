using UnityEngine;
using System.Collections;

public class CutHairEffectController : MonoBehaviour
{
    public GameObject[] CutHairEffectObject;

    void Start()
    {
        if (CutHairEffectObject != null && CutHairEffectObject.Length > 0)
        {
            int randomIndex = Random.Range(0, CutHairEffectObject.Length);
            GameObject selected = CutHairEffectObject[randomIndex];
            selected.SetActive(true);

            SpriteRenderer sr = selected.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                StartCoroutine(FadeOutAndDestroy(sr, 0.2f));
            }
        }
        else
        {
            Debug.LogWarning("CutHairEffectObject �}�C���]�w�ά��šI");
        }
    }

    IEnumerator FadeOutAndDestroy(SpriteRenderer sr, float duration)
    {
        Color originalColor = sr.color;
        float startAlpha = originalColor.a;
        float time = 0f;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, 0f, time / duration);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            time += Time.deltaTime;
            yield return null;
        }

        sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

        // �H�X��R���o�Ӹ}���Ҧb�� GameObject
        Destroy(gameObject);
    }
}
