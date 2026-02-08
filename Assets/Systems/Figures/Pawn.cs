using Systems.GameField;
using UnityEngine;
using static Systems.Globals.Constants;

namespace Systems.Figures
{
    public class Pawn : BaseFigure
    {
        private int ForwardDirection => FigureTeam == FigureTeam.Team1 ? 1 : -1;
        private bool IsFirstMove => 
            (FigureTeam == FigureTeam.Team1 && Current_cell.Grid_Coordinates.y == 1) ||
            (FigureTeam == FigureTeam.Team2 && Current_cell.Grid_Coordinates.y == 6);
        
        protected override bool ValidateMove(int x, int y)
        {
            if (!base.ValidateMove(x, y))
                return false;

            Vector2Int from = Current_cell.Grid_Coordinates;
            int dx = x - from.x;
            int dy = y - from.y;

            int dir = ForwardDirection;
            
            if (dx == 0)
            {
                if (dy == dir && IsCellEmpty(x, y))
                    return true;
                
                if (dy == 2 * dir && IsFirstMove)
                {
                    int middleY = from.y + dir;
                    if (IsCellEmpty(x, y) && IsCellEmpty(x, middleY))
                        return true;
                }
            }
            
            if (Mathf.Abs(dx) == 1 && dy == dir && IsEnemyAt(x, y))
                return true;

            return false;
        }
    }
}