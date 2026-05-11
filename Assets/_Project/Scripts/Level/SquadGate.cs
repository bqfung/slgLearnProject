using SLGLearn.Player;
using UnityEngine;

namespace SLGLearn.Level
{
    [RequireComponent(typeof(BoxCollider))]
    public sealed class SquadGate : MonoBehaviour
    {
        [SerializeField] private GateOperation operation = GateOperation.Add;
        [SerializeField] private int value = 5;
        [SerializeField] private TextMesh label;

        private bool hasTriggered;

        private void Awake()
        {
            EnsureTriggerCollider();
            RefreshLabel();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (hasTriggered)
            {
                return;
            }

            var squad = other.GetComponentInParent<SquadManager>();
            if (squad == null)
            {
                return;
            }

            ApplyTo(squad);
            hasTriggered = true;
            SetVisualActive(false);
        }

        public void Configure(GateOperation gateOperation, int gateValue, TextMesh gateLabel = null)
        {
            operation = gateOperation;
            value = Mathf.Max(1, gateValue);

            if (gateLabel != null)
            {
                label = gateLabel;
            }

            RefreshLabel();
        }

        private void ApplyTo(SquadManager squad)
        {
            switch (operation)
            {
                case GateOperation.Add:
                    squad.AddMembers(value);
                    break;
                case GateOperation.Subtract:
                    squad.RemoveMembers(value);
                    break;
                case GateOperation.Multiply:
                    squad.SetCount(squad.Count * value);
                    break;
            }
        }

        private void EnsureTriggerCollider()
        {
            var boxCollider = GetComponent<BoxCollider>();
            boxCollider.isTrigger = true;

            var body = GetComponent<Rigidbody>();
            if (body == null)
            {
                body = gameObject.AddComponent<Rigidbody>();
            }

            body.isKinematic = true;
            body.useGravity = false;
        }

        private void RefreshLabel()
        {
            if (label == null)
            {
                label = GetComponentInChildren<TextMesh>();
            }

            if (label != null)
            {
                label.text = GetDisplayText();
            }
        }

        private string GetDisplayText()
        {
            return operation switch
            {
                GateOperation.Add => $"+{value}",
                GateOperation.Subtract => $"-{value}",
                GateOperation.Multiply => $"x{value}",
                _ => value.ToString()
            };
        }

        private void SetVisualActive(bool isActive)
        {
            foreach (var renderer in GetComponentsInChildren<Renderer>())
            {
                renderer.enabled = isActive;
            }

            foreach (var collider in GetComponents<Collider>())
            {
                collider.enabled = isActive;
            }
        }

        private void OnValidate()
        {
            value = Mathf.Max(1, value);
            RefreshLabel();
        }
    }
}
