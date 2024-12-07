using UnityEngine;

[CreateAssetMenu(fileName = "GameStateManager", menuName = "ScriptableObjects/GameStateManager")] //Creates a new Asset menu for easier creation of this GameStateManager Asset
public class GameStateManager : ScriptableObject //Manages the state of the game and its features
{
    //Variables that can be retrieved and modified from other scripts
    public GameState gameState;
    public DialogueStage dialogueStage;
    public EncounterState encounterState;
    public int EncounterNum;
    public bool dialogueActive;
    public bool encounterActive;
    public bool emailDisplayed;
    public bool emailContentsDisplayed;
    public bool highlightersReady;
}



public enum GameState
{
    Start,
    Playing,
    Paused,
    Closed,
    Unknown
}



public enum DialogueStage
{
    Beginning,
    Middle,
    Feedback,
    Unknown
}



public enum EncounterState
{
    Indicators,
    Response,
    Feedback,
    Unknown
}