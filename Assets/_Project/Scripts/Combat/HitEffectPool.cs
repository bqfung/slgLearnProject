using SLGLearn.Core;
using SLGLearn.Level;
using UnityEngine;

namespace SLGLearn.Combat
{
    public sealed class HitEffectPool : ComponentPool<HitEffect>
    {
        public static HitEffectPool Shared { get; private set; }

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

        public static HitEffectPool GetOrCreate()
        {
            if (Shared != null)
            {
                return Shared;
            }

            var poolObject = new GameObject("HitEffectPool");
            return poolObject.AddComponent<HitEffectPool>();
        }

        public void Spawn(Vector3 position)
        {
            var item = Get();
            item.Play(this, position);
        }

        protected override HitEffect CreateItem()
        {
            var effectObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            effectObject.name = "HitEffect";
            effectObject.transform.SetParent(transform);
            effectObject.GetComponent<Renderer>().sharedMaterial =
                RuntimePrimitiveFactory.CreateMaterial(new Color(1f, 0.45f, 0.1f));

            var collider = effectObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
                Destroy(collider);
            }

            return effectObject.AddComponent<HitEffect>();
        }
    }
}
