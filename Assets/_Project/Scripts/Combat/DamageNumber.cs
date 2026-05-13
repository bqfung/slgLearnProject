using UnityEngine;

namespace SLGLearn.Combat
{
    public sealed class DamageNumber : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] private float lifetime = 0.65f;
        [SerializeField, Min(0f)] private float riseSpeed = 1.2f;

        private DamageNumberPool owner;
        private TextMesh label;
        private float remainingTime;

        private void Awake()
        {
            label = GetComponent<TextMesh>();
        }

        private void Update()
        {
            remainingTime -= Time.deltaTime;
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            if (remainingTime <= 0f)
            {
                ReturnToPool();
            }
        }

        public void Show(DamageNumberPool pool, Vector3 position, float damage)
        {
            owner = pool;
            remainingTime = lifetime;
            transform.position = position;
            transform.rotation = Quaternion.Euler(45f, 180f, 0f);

            if (label == null)
            {
                label = GetComponent<TextMesh>();
            }

            label.text = Mathf.CeilToInt(damage).ToString();
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
