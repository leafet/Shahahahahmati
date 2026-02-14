using System;
using Systems.Combat.EnemySpawnSystem;
using Systems.Figures;
using Systems.GameField;
using Systems.HelpSystems;
using Systems.Input;
using Systems.Interface;
using Systems.Movement;
using Systems.TurnSystem;
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
    public EnemySpawnService EnemySpawnService;
    public TurnService TurnService;
    
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

        initializeTurnManager();
        
        initializeInputService();

        initializeFigureMovementService();

        initializeEnemySpawnService();

        
        
        initializeOnGameUI();
    }

    private void initializeTurnManager()
    {
        TurnService turnService = gameObject.AddComponent<TurnService>();
        turnService.Initialize();
        TurnService = turnService;
    }

    private void initializeEnemySpawnService()
    {
        EnemySpawnService ecs = gameObject.AddComponent<EnemySpawnService>();
        ecs.Initialize();
        EnemySpawnService = ecs;
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
        
    }
}
