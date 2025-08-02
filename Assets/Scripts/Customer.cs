using System;
using Cysharp.Threading.Tasks;
using Framework;
using UnityEngine;
using UnityEngine.Assertions;

public class Customer : MonoBehaviour
{
    public int HairCount => flavor.GetHairCount();

    //private SpriteRenderer[] spriteRenderers;
    private Animator animator;
    //private int allHairStyleCount;
    private Flavor flavor;

    public void Initialize()
    {
        flavor = GetComponent<Flavor>();
        flavor.Init();
        Assert.IsNotNull(flavor);
        Amo.Instance.Log($"Initialize: ");
        animator = GetComponentInChildren<Animator>(true);
        Assert.IsNotNull(animator);
        transform.position = Consts.spawnPosition;
    }

    public async UniTask PlayAnimationAsync(string animationName, float transitionDuration = 0.1f, bool waitForComplete = true,
        Action<Customer> callBack = null)
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
            if (animator == null || this == null)
                return true; 
            var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            var result = stateInfo.IsName(animationName) && !animator.IsInTransition(0);
            callBack?.Invoke(this);
            return result;
        });

        await UniTask.WaitUntil(() =>
        {
            if (animator == null || this == null)
                return true; 
            var info = animator.GetCurrentAnimatorStateInfo(0);
            var result = info.normalizedTime >= 0.95f;
            return result;
        });
    }

    public void DestorySelf()
    {
        Amo.Instance.Log($"Destory self: {gameObject.name}");
        Destroy(gameObject);
    }

    public void UnregisterFlaverEvents()
    {
        flavor.UnregiserEvents();
    }

    public void EnableAnimator(bool isEnabled)
    {
        animator.enabled = isEnabled;
    }

}