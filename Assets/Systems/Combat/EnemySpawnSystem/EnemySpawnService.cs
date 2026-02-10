using System.Collections.Generic;
using Systems.Figures;
using Systems.GameField;
using Systems.HelpSystems;
using UnityEngine;

namespace Systems.Combat.EnemySpawnSystem
{
    public class EnemySpawnService : MonoBehaviour
    {
        public void Initialize()
        {
            foreach(List<Cell> cells in G.Instance.GameField.CellsGrid) 
            {
                foreach (Cell cell in cells)
                {
                    GameManagementActions.create_piece(cell.Grid_Coordinates.x, cell.Grid_Coordinates.y, FigureType.Bishop, FigureTeam.Team1);
                }
            }
            
            GameManagementActions.remove_piece_at_grid_by_coords(5, 5);
            GameManagementActions.create_piece(5, 5, FigureType.Bishop, FigureTeam.Team2);
        }
    }
}