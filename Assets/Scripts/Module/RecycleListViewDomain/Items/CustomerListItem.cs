using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using static Module.RecycleListViewDomain.Adapters.CustomerListAdapter;

namespace Module.RecycleListViewDomain.Items
{
    public class CustomerListItem : ListItem
    {
        public TMP_Text text;
        
        public SpriteRenderer[] spriteRenderers;
        //public Image image;
        
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
            //image ??= GetComponentInChildren<Image>(true);
            //Assert.IsNotNull(image);
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
    }
}