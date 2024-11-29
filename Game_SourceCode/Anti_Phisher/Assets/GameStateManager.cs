using UnityEngine;

public class GameStateManager : MonoBehaviour //Manages the state of the game and its features
{
    //Properties that can be retrieved and modified
    public GameState gameState {get; set;}
    public EncounterState encounterState {get; set;}
    public int EncounterNum {get; set;}

    void Start()
    {
        //Initializes the game states once the game has started
        gameState = GameState.Start;
        encounterState = EncounterState.Inactive;
    }
}



public enum GameState
{
    Start,
    Playing,
    Paused,
    Closed,
    Unknown
}



public enum EncounterState
{
    Beginning,
    Middle,
    Feedback,
    Inactive,
    Unknown
}