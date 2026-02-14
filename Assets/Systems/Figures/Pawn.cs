using Systems.GameField;
using UnityEngine;
using static Systems.Globals.Constants;

namespace Systems.Figures
{
    public class Pawn : BaseFigure
    {
        protected override bool ValidateMove(int x, int y)
        {
            if (!base.ValidateMove(x, y))
                return false;

            Vector2Int from = Current_cell.Grid_Coordinates;
            int dx = Mathf.Abs(x - from.x);
            int dy = Mathf.Abs(y - from.y);
            
            
            if(dx == 1 && dy == 0 && !IsEnemyAt(x, y))
                return true;
            
            if(dx == 0 && dy == 1 && !IsEnemyAt(x, y))
                return true;
            
            if (dx == 1 && dy == 1 && IsEnemyAt(x, y))
                return true;

            return false;
        }
    }
}