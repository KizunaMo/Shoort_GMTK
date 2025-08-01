using UnityEngine;
using UnityEngine.EventSystems;

namespace Module.RecycleListViewDomain
{
    public abstract class ListItem : MonoBehaviour, IPointerClickHandler
    {
        public object Tag { get; set; }
        public int Position { get; set; }
        private IListViewAdapter adapter;
        internal void SetAdapter(IListViewAdapter adapter)
        {
            this.adapter = adapter;
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            adapter?.OnItemClicked?.Invoke(this);
        }
        public abstract void Initialize();
        public abstract void UpdateContent(object data);
    }
}