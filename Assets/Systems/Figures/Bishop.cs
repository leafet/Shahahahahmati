using Systems.GameField;
using UnityEngine;

namespace Systems.Figures
{
    public class Bishop : BaseFigure
    {
        protected override bool ValidateMove(int x, int y)
        {
            bool allowMove = true;

            int x_mod = Mathf.Abs(Current_cell.Grid_Coordinates.x - x);
            int y_mod = Mathf.Abs(Current_cell.Grid_Coordinates.y - y);
            
            if(x_mod != y_mod)
                allowMove = false;

            if (!base.ValidateMove(x, y))
                allowMove = false;
            

            return allowMove;
        }
    }
}