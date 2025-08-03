using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimationEvent : MonoBehaviour
{

    public void DeSelect()
    {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
