using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static Module.RecycleListViewDomain.Adapters.CustomerListAdapter;

namespace Module.RecycleListViewDomain.Items
{
    public class CustomerListItem : ListItem
    {
        private TMP_Text text;
        private SpriteRenderer[] spriteRenderers;
        private Animator animator;
        
        public override void Initialize()
        {
            Amo.Instance.Log($"Initialize: ");
            text ??= GetComponentInChildren<TMP_Text>(true);
            Assert.IsNotNull(text);
            spriteRenderers = GetComponentsInChildren<SpriteRenderer>(true);
            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                Amo.Instance.Log($"{i + 1}: {spriteRenderers[i].name}");
            }
            
            animator = GetComponentInChildren<Animator>(true);
            Assert.IsNotNull(animator);
        }

        public override void UpdateContent(object data)
        {
            Amo.Instance.Log($"UPdate CustomerListItem: ");
            if (data is CustomerValueObject value)
            {
                SetText(value.Message);
                SetColor(value.Color);
            }
        }


        public void SetText(string text)
        {
            this.text.text = text;
        }
        
        public void SetColor(Color color)
        {
            //image.color = color;
        }

        public async UniTask PlayAnimationAsync(string animationName,float duration)
        {
            animator.CrossFade(animationName, duration);
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            float clipLength = stateInfo.length;

            await UniTask.Delay(TimeSpan.FromSeconds(clipLength));
        }
    }
}