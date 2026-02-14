using System.Collections.Generic;
using Systems.Figures;
using Systems.GameField;
using Systems.HelpSystems;
using UnityEngine;

namespace Systems.Combat.EnemySpawnSystem
{
    public class EnemySpawnService : MonoBehaviour
    {
        public List<BaseFigure> Enemies = new List<BaseFigure>();
        
        public void Initialize()
        {
            GameManagementActions.create_piece(5, 5, FigureType.Pawn, FigureTeam.Team2);
            GameManagementActions.create_piece(5, 6, FigureType.Bishop, FigureTeam.Team1);
        }
    }
}