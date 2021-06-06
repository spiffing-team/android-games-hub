using UnityEngine;

namespace Racer
{
    public class Road : Singleton<Road>
    {
        private float speed;
        private float distance = 0f;
        
        new Renderer renderer;
        MaterialPropertyBlock materialPropertyBlock;
        private static readonly int OffsetY = Shader.PropertyToID("_offsetY");

        protected override void OnAwake()
        {
            materialPropertyBlock = new MaterialPropertyBlock();
            renderer = GetComponent<Renderer>();
            renderer.GetPropertyBlock(materialPropertyBlock);
        }

        private void Update()
        {
            distance = (distance + (speed * Time.deltaTime)) % 1;
            materialPropertyBlock.SetFloat(OffsetY, distance);
            renderer.SetPropertyBlock(materialPropertyBlock);
        }

        public void RecalculateSpeed(float newSpeed)
        {
            speed = newSpeed * 0.1f;
        }
    }
}