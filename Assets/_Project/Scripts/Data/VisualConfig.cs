using UnityEngine;

namespace SLGLearn.Data
{
    [CreateAssetMenu(menuName = "SLG Learn/Visual Config", fileName = "VisualConfig")]
    public sealed class VisualConfig : ScriptableObject
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private GameObject hitEffectPrefab;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private GameObject soldierPrefab;

        [Header("Combat")]
        [SerializeField] private Color bulletColor = new(1f, 0.85f, 0.2f);
        [SerializeField] private Color damageNumberColor = new(1f, 0.25f, 0.15f);
        [SerializeField] private Color hitEffectColor = new(1f, 0.45f, 0.1f);
        [SerializeField, Min(0.01f)] private float bulletSize = 0.22f;
        [SerializeField, Min(0.01f)] private float hitEffectStartSize = 0.25f;
        [SerializeField, Min(0.01f)] private float damageNumberCharacterSize = 0.28f;
        [SerializeField, Min(1)] private int damageNumberFontSize = 72;

        [Header("Characters")]
        [SerializeField] private Color enemyColor = new(0.85f, 0.25f, 0.2f);
        [SerializeField] private Color bossColor = new(0.45f, 0.15f, 0.85f);
        [SerializeField] private Color soldierColor = new(0.2f, 0.65f, 0.95f);

        [Header("Gates")]
        [SerializeField] private Color positiveGateColor = new(0.1f, 0.55f, 0.95f);
        [SerializeField] private Color negativeGateColor = new(0.85f, 0.2f, 0.2f);
        [SerializeField, Min(0.01f)] private float gateLabelCharacterSize = 0.5f;
        [SerializeField, Min(1)] private int gateLabelFontSize = 96;

        [Header("UI")]
        [SerializeField] private Color resultPanelColor = new(0f, 0f, 0f, 0.72f);
        [SerializeField] private Color restartButtonColor = new(0.15f, 0.45f, 0.95f);
        [SerializeField] private Color hudTextColor = Color.white;

        public GameObject BulletPrefab => bulletPrefab;
        public GameObject HitEffectPrefab => hitEffectPrefab;
        public GameObject EnemyPrefab => enemyPrefab;
        public GameObject BossPrefab => bossPrefab;
        public GameObject SoldierPrefab => soldierPrefab;
        public Color BulletColor => bulletColor;
        public Color DamageNumberColor => damageNumberColor;
        public Color HitEffectColor => hitEffectColor;
        public float BulletSize => bulletSize;
        public float HitEffectStartSize => hitEffectStartSize;
        public float DamageNumberCharacterSize => damageNumberCharacterSize;
        public int DamageNumberFontSize => damageNumberFontSize;
        public Color EnemyColor => enemyColor;
        public Color BossColor => bossColor;
        public Color SoldierColor => soldierColor;
        public Color PositiveGateColor => positiveGateColor;
        public Color NegativeGateColor => negativeGateColor;
        public float GateLabelCharacterSize => gateLabelCharacterSize;
        public int GateLabelFontSize => gateLabelFontSize;
        public Color ResultPanelColor => resultPanelColor;
        public Color RestartButtonColor => restartButtonColor;
        public Color HudTextColor => hudTextColor;
    }
}
