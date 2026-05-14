using System.Collections.Generic;
using UnityEngine;

namespace SLGLearn.Core
{
    public abstract class ComponentPool<T> : MonoBehaviour where T : Component
    {
        [SerializeField, Min(0)] private int initialSize = 16;
        [SerializeField, Min(1)] private int maxSize = 128;

        private readonly Queue<T> available = new();
        private int totalCreated;
        private int peakActiveCount;
        private int expansionCount;

        public int TotalCreated => totalCreated;
        public int AvailableCount => available.Count;
        public int ActiveCount => totalCreated - available.Count;
        public int PeakActiveCount => peakActiveCount;
        public int ExpansionCount => expansionCount;
        public int MaxSize => maxSize;

        protected virtual void Awake()
        {
            Prewarm(initialSize);
        }

        public void Release(T item)
        {
            if (item == null)
            {
                return;
            }

            item.gameObject.SetActive(false);
            item.transform.SetParent(transform);
            available.Enqueue(item);
        }

        protected T Get()
        {
            if (available.Count > 0)
            {
                var item = available.Dequeue();
                UpdatePeakActiveCount();
                return item;
            }

            if (totalCreated >= maxSize)
            {
                expansionCount++;
                Debug.LogWarning($"{name} exceeded configured max size {maxSize}. Expanding pool.");
            }

            var created = CreateAndRegister();
            UpdatePeakActiveCount();
            return created;
        }

        protected abstract T CreateItem();

        private void Prewarm(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Release(CreateAndRegister());
            }
        }

        private T CreateAndRegister()
        {
            var item = CreateItem();
            totalCreated++;
            item.gameObject.SetActive(false);
            return item;
        }

        private void UpdatePeakActiveCount()
        {
            peakActiveCount = Mathf.Max(peakActiveCount, ActiveCount);
        }
    }
}
