using SLGLearn.Core;
using SLGLearn.Level;
using UnityEngine;

namespace SLGLearn.Combat
{
    public sealed class BulletPool : ComponentPool<Bullet>
    {
        [SerializeField] private Bullet bulletPrefab;

        public static BulletPool Shared { get; private set; }

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

        public static BulletPool GetOrCreate()
        {
            if (Shared != null)
            {
                return Shared;
            }

            var poolObject = new GameObject("BulletPool");
            return poolObject.AddComponent<BulletPool>();
        }

        public Bullet Spawn(Vector3 position)
        {
            var bullet = Get();
            bullet.transform.position = position;
            bullet.gameObject.SetActive(true);
            return bullet;
        }

        protected override Bullet CreateItem()
        {
            var configuredPrefab = bulletPrefab != null
                ? bulletPrefab.gameObject
                : RuntimePrimitiveFactory.BulletPrefab;

            if (configuredPrefab != null)
            {
                var instance = Instantiate(configuredPrefab, transform);
                instance.name = "Bullet";
                RuntimePrimitiveFactory.DisableAndDestroyCollider(instance);
                return RuntimePrimitiveFactory.GetOrAdd<Bullet>(instance);
            }

            var bulletObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bulletObject.name = "Bullet";
            bulletObject.transform.SetParent(transform);
            bulletObject.transform.localScale = Vector3.one * RuntimePrimitiveFactory.BulletSize;

            RuntimePrimitiveFactory.DisableAndDestroyCollider(bulletObject);

            bulletObject.GetComponent<Renderer>().sharedMaterial =
                RuntimePrimitiveFactory.CreateMaterial(RuntimePrimitiveFactory.BulletColor);
            return bulletObject.AddComponent<Bullet>();
        }
    }
}
