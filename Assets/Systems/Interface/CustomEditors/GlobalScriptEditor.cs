using Systems.Figures;
using UnityEditor;
using UnityEngine;

namespace Systems.Interface.CustomEditors
{
    [CustomEditor(typeof(G))]
    public class GlobalScriptEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Move piece right"))
            {
                BaseFigure current_figure = G.Instance.GameField.CellsGrid[0][0].Figure;
                
                if (current_figure is not null)
                {
                    int x = current_figure.GetGridCoordinates().x;
                    int y = current_figure.GetGridCoordinates().y;
                    current_figure.MoveOnGrid(x + 1, y);
                }
                    
            }
            if (GUILayout.Button("Move piece left"))
            {
                BaseFigure current_figure = G.Instance.GameField.CellsGrid[0][0].Figure;
                
                if (current_figure is not null)
                {
                    int x = current_figure.GetGridCoordinates().x;
                    int y = current_figure.GetGridCoordinates().y;
                    current_figure.MoveOnGrid(x - 1, y);
                }
                    
            }
            if (GUILayout.Button("Move piece up"))
            {
                BaseFigure current_figure = G.Instance.GameField.CellsGrid[0][0].Figure;
                
                if (current_figure is not null)
                {
                    int x = current_figure.GetGridCoordinates().x;
                    int y = current_figure.GetGridCoordinates().y;
                    current_figure.MoveOnGrid(x, y + 1);
                }
                    
            }
            if (GUILayout.Button("Move piece down"))
            {
                BaseFigure current_figure = G.Instance.GameField.CellsGrid[0][0].Figure;
                
                if (current_figure is not null)
                {
                    int x = current_figure.GetGridCoordinates().x;
                    int y = current_figure.GetGridCoordinates().y;
                    current_figure.MoveOnGrid(x, y - 1);
                }
                    
            }
        }
    }
}