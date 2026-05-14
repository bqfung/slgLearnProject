using SLGLearn.Data;
using UnityEngine;

namespace SLGLearn.Level
{
    public sealed class GateBuilder
    {
        public void BuildGatePair(GateConfig gate)
        {
            CreateGate(new Vector3(-2.2f, 1f, gate.Z), gate.LeftOperation, gate.LeftValue);
            CreateGate(new Vector3(2.2f, 1f, gate.Z), gate.RightOperation, gate.RightValue);
        }

        private static void CreateGate(Vector3 position, GateOperation operation, int value)
        {
            var gate = new GameObject($"Gate_{operation}_{value}_{position.z:00}");
            gate.transform.position = position;

            var body = gate.AddComponent<Rigidbody>();
            body.isKinematic = true;
            body.useGravity = false;

            var trigger = gate.AddComponent<BoxCollider>();
            trigger.isTrigger = true;
            trigger.size = new Vector3(2.4f, 2.4f, 0.5f);

            CreateGatePanel(gate.transform, operation);
            var label = CreateGateLabel(gate.transform, operation, value);
            gate.AddComponent<SquadGate>().Configure(operation, value, label);
        }

        private static void CreateGatePanel(Transform parent, GateOperation operation)
        {
            var panel = GameObject.CreatePrimitive(PrimitiveType.Cube);
            panel.name = "Panel";
            panel.transform.SetParent(parent);
            panel.transform.localPosition = Vector3.zero;
            panel.transform.localRotation = Quaternion.identity;
            panel.transform.localScale = new Vector3(2.2f, 2.2f, 0.15f);
            panel.GetComponent<Renderer>().sharedMaterial = RuntimePrimitiveFactory.CreateMaterial(
                operation == GateOperation.Subtract
                    ? RuntimePrimitiveFactory.NegativeGateColor
                    : RuntimePrimitiveFactory.PositiveGateColor);

            var collider = panel.GetComponent<Collider>();
            collider.enabled = false;
            Object.Destroy(collider);
        }

        private static TextMesh CreateGateLabel(Transform parent, GateOperation operation, int value)
        {
            var labelObject = new GameObject("Label");
            labelObject.transform.SetParent(parent);
            labelObject.transform.localPosition = new Vector3(0f, 0f, -0.11f);
            labelObject.transform.localRotation = Quaternion.Euler(0f, 180f, 0f);

            var label = labelObject.AddComponent<TextMesh>();
            label.text = GetDisplayText(operation, value);
            label.anchor = TextAnchor.MiddleCenter;
            label.alignment = TextAlignment.Center;
            label.characterSize = RuntimePrimitiveFactory.GateLabelCharacterSize;
            label.fontSize = RuntimePrimitiveFactory.GateLabelFontSize;
            label.color = Color.white;
            return label;
        }

        private static string GetDisplayText(GateOperation operation, int value)
        {
            return operation switch
            {
                GateOperation.Add => $"+{value}",
                GateOperation.Subtract => $"-{value}",
                GateOperation.Multiply => $"x{value}",
                _ => value.ToString()
            };
        }
    }
}
