using JetBrains.Annotations;
using Systems.Figures;
using UnityEngine;
using static UnityEngine.Object;

namespace Systems.HelpSystems
{
    public static class GameManagementActions
    {
        public static BaseFigure create_piece(int x, int y, FigureType type, FigureTeam team)
        {
            if(G.Instance.GameField.CellsGrid[x, y].Figure != null) return null;
            
            GameObject FigureGO = new GameObject($"{type.ToString()} {team.ToString()}");
        
            switch (type)
            {
                case FigureType.Pawn:
                    Pawn pawn = FigureGO.AddComponent<Pawn>();
                    pawn.Initialize(G.Instance.GameField.CellsGrid[x, y], type, team);
                    break;
                case FigureType.Bishop:
                    Bishop bishop = FigureGO.AddComponent<Bishop>();
                    bishop.Initialize(G.Instance.GameField.CellsGrid[x, y], type, team);
                    break;
            }
            
            return FigureGO.GetComponent<BaseFigure>();
        }
        
        public static void remove_piece_at_grid_by_coords(int x, int y)
        {
            if(G.Instance.GameField.CellsGrid[x, y].Figure is null) return;
            
            Destroy(G.Instance.GameField.CellsGrid[x, y].Figure);
            
            G.Instance.GameField.CellsGrid[x, y].Figure = null;
            
        }
    }
}