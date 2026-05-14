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
            var effectObject = RuntimePrimitiveFactory.InstantiatePrefabOrPrimitive(
                RuntimePrimitiveFactory.HitEffectPrefab,
                PrimitiveType.Sphere,
                "HitEffect",
                transform);

            var renderer = effectObject.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = RuntimePrimitiveFactory.CreateMaterial(RuntimePrimitiveFactory.HitEffectColor);
            }

            RuntimePrimitiveFactory.DisableAndDestroyCollider(effectObject);
            return RuntimePrimitiveFactory.GetOrAdd<HitEffect>(effectObject);
        }
    }
}
