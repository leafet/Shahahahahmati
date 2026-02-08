using System;
using Systems.Figures;
using Systems.GameField;
using Systems.Input;
using Systems.Interface;
using Systems.Movement;
using Unity.VisualScripting;
using UnityEngine;
using static Systems.Globals.Constants;

public class G : MonoBehaviour
{
    public static G Instance {get; private set;}
    
    public Field GameField;
    public OnGameUI OnGameUI;
    public InputService InputService;
    public FigureInteractionService FigureInteractionService;
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        initializeGameField();

        moveCameraToFieldCenter();

        initializeDebugFeatures();

        initializeInputService();

        initializeFigureMovementService();
        
        initializeOnGameUI();
    }

    private void initializeFigureMovementService()
    {
        FigureInteractionService figureInteractionService = gameObject.AddComponent<FigureInteractionService>();
        figureInteractionService.Initialize();
        
        FigureInteractionService = figureInteractionService;
    }

    private void initializeInputService()
    {
        InputService inputService = gameObject.AddComponent<InputService>();
        inputService.Initialize();
        
        InputService = inputService;
    }

    private void initializeOnGameUI()
    {
        OnGameUI onGameUI = gameObject.AddComponent<OnGameUI>();
        onGameUI.Initialize();
        
        OnGameUI = onGameUI;
    }

    private void moveCameraToFieldCenter()
    {
        if (Camera.main == null) return;

        Camera.main.transform.position = new Vector3(FIELD_SIZE / 2, FIELD_SIZE / 2, -2);
        Camera.main.orthographicSize = FIELD_SIZE / 2;
    }

    private void initializeGameField()
    {
        GameObject gameField = new GameObject("Game Field");
        Field game_field = gameField.AddComponent<Field>();
        game_field.Initialize();
        GameField = game_field;
    }

    private void initializeDebugFeatures()
    {
        TEMPORARY_create_piece(0, 0, FigureType.Pawn, FigureTeam.Team1);
        TEMPORARY_create_piece(1, 0, FigureType.Pawn, FigureTeam.Team1);
        TEMPORARY_create_piece(2, 0, FigureType.Pawn, FigureTeam.Team1);
        
        TEMPORARY_create_piece(0, GRID_SIZE - 1, FigureType.Pawn, FigureTeam.Team2);
        TEMPORARY_create_piece(1, GRID_SIZE - 1, FigureType.Pawn, FigureTeam.Team2);
        TEMPORARY_create_piece(2, GRID_SIZE - 1, FigureType.Pawn, FigureTeam.Team2);
        
        TEMPORARY_create_piece(4, 4, FigureType.Bishop, FigureTeam.Team1);
    }

    private void TEMPORARY_create_piece(int x, int y, FigureType type, FigureTeam team)
    {
        GameObject FigureGO = new GameObject($"{type.ToString()} {team.ToString()}");
        
        switch (type)
        {
            case FigureType.Pawn:
                Pawn pawn = FigureGO.AddComponent<Pawn>();
                pawn.Initialize(Instance.GameField.CellsGrid[x][y], type, team);
                break;
            case FigureType.Bishop:
                Bishop bishop = FigureGO.AddComponent<Bishop>();
                bishop.Initialize(Instance.GameField.CellsGrid[x][y], type, team);
                break;
        }
    }
}
