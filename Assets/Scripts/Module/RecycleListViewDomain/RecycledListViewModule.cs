using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Tool;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Module.RecycleListViewDomain
{
    public enum ScrollAlignment
    {
        Top,
        Center,
        Bottom
    }

    public class RecycledListViewModule : MonoBehaviour
    {
        private RectTransform viewportTransform;
        private RectTransform contentTransform;

        private float itemHeight;
        /// <summary>
        /// [NOTE] 1/Height
        /// </summary>
        private float _1OverItemHeight;
        private float viewportHeight;

        private Dictionary<int, ListItem> items;
        private Stack<ListItem> pooledItems;

        private IListViewAdapter viewAdapter;

        private int currentTopIndex;
        private int currentBottomIndex;

        private Tweener scrollTweener;
        private ScrollRect scrollRect;

        public bool IsScrolling => scrollTweener != null && scrollTweener.IsPlaying();

        public async UniTask InitializeAsync()
        {
            await UniTask.CompletedTask;
            items = new Dictionary<int, ListItem>();
            pooledItems = new Stack<ListItem>();
            viewAdapter = null;
            currentTopIndex = -1;
            currentBottomIndex = -1;

            viewportTransform = GetComponentsInChildren<IdentifyComponent>(true)
                .FirstOrDefault(i => i.VariableKey == nameof(viewportTransform))
                ?.GetTargetComponent<RectTransform>();
            Assert.IsNotNull(viewportTransform);
            contentTransform = GetComponentsInChildren<IdentifyComponent>(true)
                .FirstOrDefault(i => i.VariableKey == nameof(contentTransform))
                ?.GetTargetComponent<RectTransform>();
            Assert.IsNotNull(contentTransform);
            viewportHeight = viewportTransform.rect.height;
            scrollRect = GetComponent<ScrollRect>();
            scrollRect.onValueChanged.AddListener((pos) => UpdateItemsInTheList());
        }

        public void SetAdapter(IListViewAdapter adapter)
        {
            this.viewAdapter = adapter;
            itemHeight = adapter.ItemHeight;
            _1OverItemHeight = 1f / itemHeight;
        }

        /// <summary>
        /// Update view from adapter data.
        /// </summary>
        public void UpdateList()
        {
            float newHeight = Mathf.Max(1f, viewAdapter.Count * itemHeight);
            contentTransform.sizeDelta = new Vector2(0f, newHeight);
            viewportHeight = viewportTransform.rect.height;
            UpdateItemsInTheList(true);
        }

        public void OnViewportDimensionsChanged()
        {
            viewportHeight = viewportTransform.rect.height;
            UpdateItemsInTheList();
        }

        public void RefreshItem(int index)
        {
            if (items.TryGetValue(index, out ListItem item))
            {
                viewAdapter.SetItemContent(item);
            }
        }

        public (int topIndex, int bottomIndex) GetVisibleItemRange()
        {
            return (currentTopIndex, currentBottomIndex);
        }

        public void StopScrolling(bool complete = false)
        {
            if (complete)
                scrollTweener?.Complete();
            else
                KillAnimation();
        }

        private void UpdateItemsInTheList(bool updateAllVisibleItems = false)
        {
            if (viewAdapter == null || viewAdapter.Count == 0)
            {
                ClearAllItems();
                return;
            }

            float contentPos = contentTransform.anchoredPosition.y - 1f;
            int newTopIndex = Mathf.Max(0, Mathf.FloorToInt(contentPos * _1OverItemHeight));
            int newBottomIndex = Mathf.Min(viewAdapter.Count - 1,
                Mathf.CeilToInt((contentPos + viewportHeight + 2f) * _1OverItemHeight));

            if (currentTopIndex == -1)
            {
                CreateItemsBetweenIndices(newTopIndex, newBottomIndex);
            }
            else
            {
                UpdateVisibleItems(newTopIndex, newBottomIndex, updateAllVisibleItems);
            }

            currentTopIndex = newTopIndex;
            currentBottomIndex = newBottomIndex;
        }

        private void CreateItemsBetweenIndices(int topIndex, int bottomIndex)
        {
            for (int i = topIndex; i <= bottomIndex; i++)
            {
                CreateItemAtIndex(i);
            }
        }

        private void CreateItemAtIndex(int index)
        {
            ListItem item = pooledItems.Count > 0 ? pooledItems.Pop() : viewAdapter.CreateItem();
            item.gameObject.SetActive(true);
            item.SetAdapter(viewAdapter);
            item.Position = index;

            RectTransform rectTransform = (RectTransform)item.transform;
            rectTransform.SetParent(contentTransform, false);

            float yPosition = -index * itemHeight - (itemHeight * 0.5f);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.anchoredPosition = new Vector2(0, yPosition);
            rectTransform.sizeDelta = new Vector2(0, itemHeight);

            items[index] = item;
            viewAdapter.SetItemContent(item);
        }

        private void UpdateVisibleItems(int newTopIndex, int newBottomIndex, bool updateAllVisibleItems)
        {
            // Remove items that are no longer visible
            for (int i = currentTopIndex; i < newTopIndex; i++)
                RemoveItemAtIndex(i);
            for (int i = newBottomIndex + 1; i <= currentBottomIndex; i++)
                RemoveItemAtIndex(i);

            // Add newly visible items
            for (int i = newTopIndex; i <= newBottomIndex; i++)
            {
                if (!items.ContainsKey(i))
                    CreateItemAtIndex(i);
            }

            // Update content of visible items if necessary
            if (updateAllVisibleItems)
            {
                for (int i = newTopIndex; i <= newBottomIndex; i++)
                {
                    viewAdapter.SetItemContent(items[i]);
                }
            }
        }

        private void RemoveItemAtIndex(int index)
        {
            if (items.TryGetValue(index, out ListItem item))
            {
                item.gameObject.SetActive(false);
                pooledItems.Push(item);
                items.Remove(index);
            }
        }

        public void ClearAllItems()
        {
            KillAnimation();
            foreach (var item in items.Values)
            {
                item.gameObject.SetActive(false);
                pooledItems.Push(item);
            }

            items.Clear();
            currentTopIndex = currentBottomIndex = -1;
        }

        public void ScrollToIndex(int index, ScrollAlignment alignment = ScrollAlignment.Top, float duration = 0.3f,
            Ease easeType = Ease.OutQuad, Action onComplete = null)
        {
            if (viewAdapter == null || index < 0 || index >= viewAdapter.Count)
                return;

            KillAnimation();

            // Calculate the base target position
            var targetPosition = index * itemHeight;

            // Adjust the target position based on the alignment
            switch (alignment)
            {
                case ScrollAlignment.Center:
                    targetPosition -= (viewportHeight - itemHeight) * 0.5f;
                    break;
                case ScrollAlignment.Bottom:
                    targetPosition -= (viewportHeight - itemHeight);
                    break;
            }

            // Ensure the target position is within a valid range
            targetPosition = Mathf.Max(0, Mathf.Min(targetPosition, contentTransform.sizeDelta.y - viewportHeight));
            float normalizedPosition = targetPosition / (contentTransform.sizeDelta.y - viewportHeight);
            normalizedPosition = Mathf.Clamp01(normalizedPosition);

            float endPos = 1f - normalizedPosition;

            scrollTweener = DOTween.To(
                    () => scrollRect.verticalNormalizedPosition,
                    (x) =>
                    {
                        scrollRect.verticalNormalizedPosition = x;
                        UpdateItemsInTheList();
                    },
                    endPos,
                    duration)
                .SetEase(easeType)
                .OnComplete(() =>
                {
                    UpdateItemsInTheList();
                    onComplete?.Invoke();
                });
        }

        public void ScrollToIndexImmediate(int index, ScrollAlignment alignment = ScrollAlignment.Top)
        {
            ScrollToIndex(index, alignment, 0f, Ease.Linear);
        }

        private void KillAnimation()
        {
            scrollTweener?.Kill();
        }

        private void OnDestroy()
        {
            KillAnimation();
        }
    }
}
