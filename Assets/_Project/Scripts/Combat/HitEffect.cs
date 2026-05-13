using UnityEngine;

namespace SLGLearn.Combat
{
    public sealed class HitEffect : MonoBehaviour
    {
        [SerializeField, Min(0.05f)] private float lifetime = 0.18f;

        private HitEffectPool owner;
        private float remainingTime;

        private void Update()
        {
            remainingTime -= Time.deltaTime;
            transform.localScale += Vector3.one * (Time.deltaTime * 2.5f);

            if (remainingTime <= 0f)
            {
                ReturnToPool();
            }
        }

        public void Play(HitEffectPool pool, Vector3 position)
        {
            owner = pool;
            remainingTime = lifetime;
            transform.position = position;
            transform.localScale = Vector3.one * 0.25f;
            gameObject.SetActive(true);
        }

        private void ReturnToPool()
        {
            if (owner != null)
            {
                owner.Release(this);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}
