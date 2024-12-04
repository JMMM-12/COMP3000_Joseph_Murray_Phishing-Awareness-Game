using UnityEngine;

[CreateAssetMenu(fileName = "GameStateManager", menuName = "ScriptableObjects/GameStateManager")] //Creates a new Asset menu for easier creation of this GameStateManager Asset
public class GameStateManager : ScriptableObject //Manages the state of the game and its features
{
    //Variables that can be retrieved and modified from other scripts
    public GameState gameState;
    public EncounterState encounterState;
    public int EncounterNum;
    public bool dialogueActive;
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
    Indicators,
    Response,
    Feedback,
    Rewards,
    Inactive,
    Unknown
}