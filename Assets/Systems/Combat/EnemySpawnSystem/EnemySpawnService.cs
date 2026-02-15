using System.Collections.Generic;
using Systems.Figures;
using Systems.GameField;
using Systems.Globals;
using Systems.HelpSystems;
using UnityEngine;
using UnityEngine.Serialization;

namespace Systems.Combat.EnemySpawnSystem
{
    public class EnemySpawnService : MonoBehaviour
    {
        public List<BaseFigure> ActiveEnemies;

        public BaseFigure Player;
        
        public void Initialize()
        {
            ActiveEnemies = new List<BaseFigure>();
            
            Player = GameManagementActions.create_piece(5, 5, FigureType.Bishop, FigureTeam.Team1);
            for (int i = 0; i < 7; i++)
            {
                int pos_x = Random.Range(0, Constants.GRID_SIZE);
                int pos_y = Random.Range(0, Constants.GRID_SIZE);
                
                BaseFigure enemyToAdd = GameManagementActions.create_piece(pos_x, pos_y, FigureType.Pawn, FigureTeam.Team2);

                if (enemyToAdd is not null && !ActiveEnemies.Contains(enemyToAdd))
                {
                    ActiveEnemies.Add(enemyToAdd);
                    enemyToAdd.LivingComponent.OnDeath += (go) => HandleEnemyDeath(enemyToAdd);
                }
                    
            }
        }

        private void HandleEnemyDeath(BaseFigure enemy)
        {
            if (ActiveEnemies.Contains(enemy))
            {
                ActiveEnemies.Remove(enemy);
            }
        }
    }
}