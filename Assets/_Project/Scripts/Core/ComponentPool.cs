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
                return available.Dequeue();
            }

            if (totalCreated >= maxSize)
            {
                Debug.LogWarning($"{name} exceeded configured max size {maxSize}. Expanding pool.");
            }

            return CreateAndRegister();
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
    }
}
