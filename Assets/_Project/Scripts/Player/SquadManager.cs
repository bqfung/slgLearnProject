using System.Collections.Generic;
using SLGLearn.Combat;
using UnityEngine;

namespace SLGLearn.Player
{
    public sealed class SquadManager : MonoBehaviour
    {
        [Header("Members")]
        [SerializeField] private Transform soldierPrefab;
        [SerializeField, Min(1)] private int initialCount = 5;
        [SerializeField, Min(1)] private int maxCount = 100;
        [SerializeField] private bool addDefaultWeapon = true;

        [Header("Formation")]
        [SerializeField, Min(1)] private int membersPerRow = 5;
        [SerializeField, Min(0.1f)] private float spacing = 0.75f;
        [SerializeField, Min(0f)] private float followSpeed = 12f;

        private readonly List<Transform> members = new();
        private readonly List<Vector3> targetPositions = new();

        public int Count => members.Count;

        private void Start()
        {
            SetCount(initialCount);
        }

        private void Update()
        {
            UpdateFormationMovement();
        }

        public void AddMembers(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            SetCount(Count + amount);
        }

        public void RemoveMembers(int amount)
        {
            if (amount <= 0)
            {
                return;
            }

            SetCount(Count - amount);
        }

        public void SetCount(int count)
        {
            var targetCount = Mathf.Clamp(count, 0, maxCount);

            while (members.Count < targetCount)
            {
                members.Add(CreateMember(members.Count));
            }

            while (members.Count > targetCount)
            {
                var lastIndex = members.Count - 1;
                var member = members[lastIndex];
                members.RemoveAt(lastIndex);

                if (member != null)
                {
                    Destroy(member.gameObject);
                }
            }

            RebuildTargetPositions();
        }

        private Transform CreateMember(int index)
        {
            Transform member;
            if (soldierPrefab != null)
            {
                member = Instantiate(soldierPrefab, transform);
            }
            else
            {
                var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                capsule.name = $"Soldier_{index + 1:00}";
                capsule.transform.SetParent(transform);
                capsule.transform.localScale = new Vector3(0.45f, 0.9f, 0.45f);
                member = capsule.transform;
            }

            member.name = $"Soldier_{index + 1:00}";
            member.localPosition = Vector3.zero;
            member.localRotation = Quaternion.identity;

            if (addDefaultWeapon && member.GetComponent<SoldierWeapon>() == null)
            {
                member.gameObject.AddComponent<SoldierWeapon>();
            }

            return member;
        }

        private void RebuildTargetPositions()
        {
            targetPositions.Clear();

            for (var i = 0; i < members.Count; i++)
            {
                var row = i / membersPerRow;
                var column = i % membersPerRow;
                var rowCount = Mathf.Min(membersPerRow, members.Count - row * membersPerRow);
                var rowWidth = (rowCount - 1) * spacing;
                var x = column * spacing - rowWidth * 0.5f;
                var z = -row * spacing;

                targetPositions.Add(new Vector3(x, 0f, z));
            }
        }

        private void UpdateFormationMovement()
        {
            for (var i = 0; i < members.Count; i++)
            {
                var member = members[i];
                if (member == null)
                {
                    continue;
                }

                var target = targetPositions[i];
                member.localPosition = Vector3.Lerp(
                    member.localPosition,
                    target,
                    1f - Mathf.Exp(-followSpeed * Time.deltaTime));
            }
        }

        private void OnValidate()
        {
            initialCount = Mathf.Clamp(initialCount, 1, maxCount);
            membersPerRow = Mathf.Max(1, membersPerRow);
        }
    }
}
