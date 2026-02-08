using JetBrains.Annotations;
using Systems.Figures;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.GameField
{
    public class Cell
    {
        [CanBeNull] public BaseFigure Figure;
        
        public Vector2Int Grid_Coordinates;
        private SpriteRenderer _cell_renderer;
        
        public void Initialize(Vector2Int cell_coordinates,Vector3 cell_position, BaseFigure figure)
        {
            Grid_Coordinates = cell_coordinates;
            
            Figure = figure;
        }
    }
}