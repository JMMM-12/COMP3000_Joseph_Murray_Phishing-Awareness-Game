using UnityEngine;

public class GameQuit_Logic : MonoBehaviour //Performs game state and selection data resets upon game quit
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;

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
        selectionData.indicatorSelection = new IndicatorSelection();
        selectionData.responseSelection = new ResponseSelection();
    }
}
