using System.Collections.Generic;
using UnityEngine;

public class GameQuit_Logic : MonoBehaviour //Performs game state and selection data resets upon game quit
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;
    public EncounterResults encounterResults;

    private void OnApplicationQuit()
    {
        Debug.Log("Game is Quitting. Resetting the Game state");
        gameStateManager.gameState = GameState.Closed;
        gameStateManager.encounterActive = false;
        gameStateManager.dialogueActive = false;
        gameStateManager.encounterState = EncounterState.Unknown;
        gameStateManager.dialogueStage = DialogueStage.Unknown;
        gameStateManager.EncounterNum = 0;
        gameStateManager.emailDisplayed = false;
        gameStateManager.emailContentsDisplayed = false;
        gameStateManager.highlightersReady = false;
        gameStateManager.instructionsDisplayed = false;
        gameStateManager.helpButtonActive = false;
        gameStateManager.instructionsTextRequired = false;
        gameStateManager.highlightersTransparent = false;
        gameStateManager.answerCheckRequired = false;
        selectionData.indicatorSelection = new IndicatorSelection();
        selectionData.responseSelection = new ResponseSelection();
        encounterResults.indicatorResults = new IndicatorResults();
        encounterResults.responseResults = new ResponseResults();
    }
}
