using System;
using Cysharp.Threading.Tasks;
using Framework;
using UnityEngine;
using UnityEngine.Assertions;

public class Customer : MonoBehaviour
{
    public int HairCount => allHairStyleCount;
    
    private SpriteRenderer[] spriteRenderers;
    private Animator animator;
    private int allHairStyleCount;
        
    public void Initialize()
    {
        allHairStyleCount = 0;
        Amo.Instance.Log($"Initialize: ");
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
        for (int i = 0; i < spriteRenderers.Length; i++)
        {
            Amo.Instance.Log($"{i + 1}: {spriteRenderers[i].name}");
            if (spriteRenderers[i].name.Contains("hair"))
            {
                allHairStyleCount++;
            }
        }
        Amo.Instance.Log($"All hair style count: {allHairStyleCount}");
            
        animator = GetComponentInChildren<Animator>(true);
        Assert.IsNotNull(animator);
        transform.position = Consts.spawnPosition;
    }
    
    public async UniTask PlayAnimationAsync(string animationName, float transitionDuration = 0.1f, bool waitForComplete = true)
    {
        if (!animator.enabled || animator.runtimeAnimatorController == null)
        {
            return;
        }
    
        animator.CrossFade(animationName, transitionDuration);

        if (!waitForComplete)
        {
            return;
        }
    
        await UniTask.WaitUntil(() => 
        {
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName(animationName) && !animator.IsInTransition(0);
        });
    
        await UniTask.WaitUntil(() => 
        {
            var info = animator.GetCurrentAnimatorStateInfo(0);
            return info.normalizedTime >= 0.95f;
        });
    }
}