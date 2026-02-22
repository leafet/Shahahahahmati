using UnityEngine;
using static Systems.Globals.Constants;

namespace Systems.Combat
{
    public class BattleEffect : MonoBehaviour
    {
        [SerializeField] private float lifetime = 0.5f;
        [SerializeField] private float effectWidthPercent = 0.2f;

        private float distance = CELL_SIZE;
        private float hueOffset = 0f;

        public void SetDistance(float dist)
        {
            distance = Mathf.Max(dist, CELL_SIZE * 0.5f);
        }

        private void Awake()
        {
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.material = new Material(Shader.Find("Sprites/Default"));
                spriteRenderer.color = Color.white;

                int textureSize = 32;
                Texture2D texture = new Texture2D(textureSize, textureSize);
                Color[] pixels = new Color[textureSize * textureSize];

                for (int y = 0; y < textureSize; y++)
                {
                    for (int x = 0; x < textureSize; x++)
                    {
                        float dx = x - textureSize / 2f;
                        float dy = y - textureSize / 2f;
                        float dist = Mathf.Sqrt(dx * dx + dy * dy);
                        float maxDist = textureSize / 2f;

                        if (dist <= maxDist)
                        {
                            float alpha = 1f - Mathf.Pow(dist / maxDist, 2);
                            Color rainbowColor = Color.HSVToRGB((x / (float)textureSize + hueOffset) % 1f, 1f, 1f);
                            pixels[y * textureSize + x] = new Color(rainbowColor.r, rainbowColor.g, rainbowColor.b, alpha);
                        }
                        else
                        {
                            pixels[y * textureSize + x] = Color.clear;
                        }
                    }
                }

                texture.SetPixels(pixels);
                texture.Apply();
                texture.filterMode = FilterMode.Bilinear;

                Sprite sprite = Sprite.Create(texture, new Rect(0, 0, textureSize, textureSize), new Vector2(0.5f, 0.5f), textureSize);
                spriteRenderer.sprite = sprite;
            }

            float effectWidth = CELL_SIZE * effectWidthPercent;
            transform.localScale = new Vector3(distance, effectWidth, 1);
        }

        private void Update()
        {
            hueOffset += Time.deltaTime * 2f;
        }

        private void Start()
        {
            Destroy(gameObject, lifetime);
        }
    }
}
