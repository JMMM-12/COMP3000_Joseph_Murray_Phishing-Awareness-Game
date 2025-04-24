using UnityEngine;

public class GameManager : MonoBehaviour //Clones the GameStateManager Asset as a runtime object
{
    public static GameManager Instance;
    public GameStateManager gameStateManager;
    public SelectionData selectionData;
    public EncounterResults encounterResults;
    public GameData gameData;


    [SerializeField] private GameStateManager orgGameStateManagerAsset;
    [SerializeField] private SelectionData orgSelectionDataAsset;
    [SerializeField] private EncounterResults orgEncounterResultsAsset;
    [SerializeField] private GameData orgGameDataAsset;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            gameStateManager = Instantiate(orgGameStateManagerAsset);
            selectionData = Instantiate(orgSelectionDataAsset);
            encounterResults = Instantiate(orgEncounterResultsAsset);
            gameData = Instantiate(orgGameDataAsset);
        }

        else
        {
            Destroy(gameObject);
        }
    }
}
