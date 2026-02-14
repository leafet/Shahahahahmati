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

        public BaseFigure Player;
        
        public void Initialize()
        {
            Player = GameManagementActions.create_piece(5, 5, FigureType.Pawn, FigureTeam.Team1);
            Enemies.Add(GameManagementActions.create_piece(5, 6, FigureType.Pawn, FigureTeam.Team2)); 
            Enemies.Add(GameManagementActions.create_piece(1, 1, FigureType.Pawn, FigureTeam.Team2)); 
            Enemies.Add(GameManagementActions.create_piece(2, 2, FigureType.Pawn, FigureTeam.Team2)); 
            Enemies.Add(GameManagementActions.create_piece(3, 1, FigureType.Pawn, FigureTeam.Team2)); 
            Enemies.Add(GameManagementActions.create_piece(5, 1, FigureType.Pawn, FigureTeam.Team2)); 
            
        }
    }
}