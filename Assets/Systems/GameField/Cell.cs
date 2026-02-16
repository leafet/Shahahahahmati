using JetBrains.Annotations;
using Systems.Figures;
using UnityEngine;
using static Systems.Globals.Constants;

namespace Systems.GameField
{
    public class Cell : MonoBehaviour
    {
        public BaseFigure Figure;
        public Vector2Int Grid_Coordinates;
        
        private SpriteRenderer _cell_renderer;
        
        public void Initialize(Vector2Int cell_coordinates, bool is_cell_even)
        {
            Grid_Coordinates = cell_coordinates;
            
            transform.position = new Vector3(
                Grid_Coordinates.x * CELL_SIZE + CELL_SIZE / 2
                , Grid_Coordinates.y * CELL_SIZE + CELL_SIZE / 2
                , 0);

            _cell_renderer = gameObject.AddComponent<SpriteRenderer>();
            
            var whiteSprite = Resources.Load<Sprite>("Sprites/FieldSprites/tile4");
            var blackSprite = Resources.Load<Sprite>("Sprites/FieldSprites/tile3");
            
            _cell_renderer.sprite = is_cell_even ? whiteSprite : blackSprite;
        }
    }
}