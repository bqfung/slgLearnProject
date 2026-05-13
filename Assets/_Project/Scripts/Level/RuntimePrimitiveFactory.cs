using UnityEngine;

namespace SLGLearn.Level
{
    public static class RuntimePrimitiveFactory
    {
        public static Material CreateMaterial(Color color)
        {
            var shader = Shader.Find("Universal Render Pipeline/Lit");
            if (shader == null)
            {
                shader = Shader.Find("Standard");
            }

            var material = new Material(shader);
            material.color = color;
            return material;
        }
    }
}
