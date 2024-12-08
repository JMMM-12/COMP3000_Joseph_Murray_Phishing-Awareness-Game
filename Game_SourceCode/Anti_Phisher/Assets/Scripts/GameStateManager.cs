using UnityEngine;

[CreateAssetMenu(fileName = "GameStateManager", menuName = "ScriptableObjects/GameStateManager")] //Creates a new Asset menu for easier creation of this GameStateManager Asset
public class GameStateManager : ScriptableObject //Manages the state of the game and its specific phases
{
    //Variables that can be retrieved and modified from other scripts
    public GameState gameState; //High-level game state of the entire game
    public DialogueStage dialogueStage; //State of the current encounter's dialogue
    public EncounterState encounterState; //State of the current encounter
    public FeedbackState feedbackState; //state of the current encounter's feedback phase
    public int EncounterNum; //Indicates the encounter the game is currently on
    public bool dialogueActive; //Determines whether the dialogue is active or inactive
    public bool encounterActive; //Determines whether the encounter is active or inactive
    public bool emailDisplayed; //Determines whether the email UI elements are active and displayed
    public bool emailContentsDisplayed; //Determines whether the email contents are displayed
    public bool highlightersReady; //Determined whether the indicator highlighter images are ready
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



public enum FeedbackState
{
    AnswersCheck,
    FeedbackDetermine,
    FeedbackDisplay,
    Inactive
}