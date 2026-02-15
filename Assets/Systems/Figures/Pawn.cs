using Systems.GameField;
using UnityEngine;
using static Systems.Globals.Constants;

namespace Systems.Figures
{
    public class Pawn : BaseFigure
    {
        protected override bool CanMoveToEmptyCell(int x, int y)
        {
            Vector2Int from = Current_cell.Grid_Coordinates;
            
            int dx = Mathf.Abs(x - from.x);
            int dy = Mathf.Abs(y - from.y);
            
            return (dx == 1 && dy == 0) ||  (dx == 0 && dy == 1);
        }

        protected override bool CanAttackAtCell(int x, int y)
        {
            Vector2Int from = Current_cell.Grid_Coordinates;
            
            int dx = Mathf.Abs(x - from.x);
            int dy = Mathf.Abs(y - from.y);
            
            return dx == 1 && dy == 0;
        }
    }
}