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
            if (bulletPrefab != null)
            {
                return Instantiate(bulletPrefab, transform);
            }

            var bulletObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            bulletObject.name = "Bullet";
            bulletObject.transform.SetParent(transform);
            bulletObject.transform.localScale = Vector3.one * 0.22f;

            var collider = bulletObject.GetComponent<Collider>();
            if (collider != null)
            {
                collider.enabled = false;
                Destroy(collider);
            }

            bulletObject.GetComponent<Renderer>().sharedMaterial =
                RuntimePrimitiveFactory.CreateMaterial(new Color(1f, 0.85f, 0.2f));
            return bulletObject.AddComponent<Bullet>();
        }
    }
}
