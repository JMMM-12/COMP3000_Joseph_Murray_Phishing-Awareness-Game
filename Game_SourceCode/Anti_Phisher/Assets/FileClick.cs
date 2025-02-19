using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FileClick : MonoBehaviour, IPointerClickHandler //Tracks when the file is clicked on and changes the indicator or response selection value accordingly
{
    public GameStateManager gameStateManager;
    public SelectionData selectionData;
    public GameData gameData;

    public Image fileImage;
    public Text fileText;
    private bool noFile;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameStateManager.encounterActive == true) //Checks that the encounter is currently active
        {
            if (gameStateManager.encounterState == EncounterState.Indicators) //Checks if the encounter is in the indicators state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true && gameStateManager.highlightersReady == true) //Checks that the email UI elements & contents have been displayed
                {
                    noFile = CheckFileExistence(); //Checks if the email contains a file
                    if (noFile == false) //If the email does contain a file
                    {
                        if (selectionData.indicatorSelection.fileSelected == false) //Checks if the file indicator is not already selected
                        {
                            fileImage.color = gameData.selectedColor;
                            selectionData.indicatorSelection.fileSelected = true; //Marks the file indicator as selected
                            Debug.Log("File indicator was selected");
                        }
                        else
                        {
                            fileImage.color = gameData.transparentColor;
                            selectionData.indicatorSelection.fileSelected = false; //Marks the file indicator as unselected
                            Debug.Log("File indicator was deselected");
                        }
                    }

                    else //If the email does not contain a file
                    {
                        Debug.Log("File area was clicked on, but this email does not contain any files");
                    }
                    

                }
            }

            else if (gameStateManager.encounterState == EncounterState.Response) //Checks if the encounter is in the response state
            {
                if (gameStateManager.emailDisplayed == true && gameStateManager.emailContentsDisplayed == true) //Checks that the email UI elements & contents have been displayed
                {
                    if (noFile == false) //If the email does contain a file
                    {
                        selectionData.responseSelection.fileDownloaded = true;
                        Debug.Log("Response - File was downloaded");
                        gameStateManager.encounterState = EncounterState.RFeedback;
                        gameStateManager.emailDisplayed = false;
                        gameStateManager.answerCheckRequired = true;
                    }

                    else //If the email does not contain a file
                    {
                        Debug.Log("File area was clicked on, but this email does not contain any files");
                    }
                }

                
            }
        }

    }

    private bool CheckFileExistence() //Checks if the email has not attached files
    {
        fileText = GetComponent<Text>();
        if (fileText != null)
        {
            if (fileText.text.ToString() == "No attached files")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            Debug.LogError("Email file text could not be read");
            return false;
        }
    }
}
