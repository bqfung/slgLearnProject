using SLGLearn.Core;
using UnityEngine;

namespace SLGLearn.Combat
{
    public sealed class DamageNumberPool : ComponentPool<DamageNumber>
    {
        public static DamageNumberPool Shared { get; private set; }

        protected override void Awake()
        {
            if (Shared != null && Shared != this)
            {
                Destroy(gameObject);
                return;
            }

            Shared = this;
            base.Awake();
        }

        public static DamageNumberPool GetOrCreate()
        {
            if (Shared != null)
            {
                return Shared;
            }

            var poolObject = new GameObject("DamageNumberPool");
            return poolObject.AddComponent<DamageNumberPool>();
        }

        public void Spawn(Vector3 position, float damage)
        {
            var item = Get();
            item.Show(this, position, damage);
        }

        protected override DamageNumber CreateItem()
        {
            var itemObject = new GameObject("DamageNumber");
            itemObject.transform.SetParent(transform);

            var text = itemObject.AddComponent<TextMesh>();
            text.anchor = TextAnchor.MiddleCenter;
            text.alignment = TextAlignment.Center;
            text.characterSize = 0.28f;
            text.fontSize = 72;
            text.color = new Color(1f, 0.25f, 0.15f);

            return itemObject.AddComponent<DamageNumber>();
        }
    }
}
