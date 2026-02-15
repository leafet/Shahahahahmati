using System;
using System.Collections.Generic;
using static Systems.Globals.Constants;
using UnityEngine;

namespace Systems.GameField
{
    public class Field : MonoBehaviour
    {
        public Cell[,] CellsGrid{get; private set;}
        
        public void Initialize()
        {
            CellsGrid = new Cell[GRID_SIZE, GRID_SIZE];
            
            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    CreateCell(x, y);
                }
            }
        }

        private void CreateCell(int x, int y)
        {
            GameObject go = new GameObject($"Cell {x}_{y}");
            
            go.transform.SetParent(transform);
            
            Cell cell = go.AddComponent<Cell>();
            
            cell.Initialize(new Vector2Int(x, y), (x + y) % 2 == 0);
            
            CellsGrid[x, y] = cell;
        }

        // private void generateCellsAsOneImage()
        // {
        //     Texture2D texture = new Texture2D(FIELD_SIZE, FIELD_SIZE);
        //     Color[] texture_colors = new Color[FIELD_SIZE * FIELD_SIZE];
        //     
        //     for (int i = 0; i < GRID_SIZE; i++)
        //     {
        //         List<Cell> cells_row = new List<Cell>();
        //         
        //         for (int j = 0; j < GRID_SIZE; j++)
        //         {
        //             Vector3 cell_pos = new Vector3(i * CELL_SIZE, 0, j * CELL_SIZE);
        //             
        //             Cell cell_logic = new Cell();
        //             
        //             cell_logic.Initialize(new Vector2Int(i, j) ,cell_pos, null);
        //             
        //             cells_row.Add(cell_logic);
        //         }
        //         
        //         CellsGrid.Add(cells_row);
        //     }
        //     
        //     for (int k = 0; k < texture_colors.Length; k++)
        //     {
        //         if (k % (FIELD_SIZE * CELL_SIZE * 2) < FIELD_SIZE * CELL_SIZE)
        //             texture_colors[k] = (k % (CELL_SIZE * 2) < CELL_SIZE) ? Color.white : Color.darkGray;
        //         else
        //             texture_colors[k] = (k % (CELL_SIZE * 2) < CELL_SIZE) ? Color.darkGray : Color.white;
        //     }
        //     
        //     texture.SetPixels(texture_colors);
        //     texture.Apply();
        //     
        //     Sprite grid_sprite = 
        //         Sprite.Create(texture, 
        //             new Rect(0, 0, FIELD_SIZE, FIELD_SIZE), 
        //             new Vector2(0, 0), 1.0f);
        //     
        //     gameObject.AddComponent<SpriteRenderer>().sprite = grid_sprite;
        // }
    }
}