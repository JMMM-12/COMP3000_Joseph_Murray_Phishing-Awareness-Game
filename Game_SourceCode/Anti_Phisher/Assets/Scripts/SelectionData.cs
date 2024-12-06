using UnityEngine;

[CreateAssetMenu(fileName = "SelectionData", menuName = "ScriptableObjects/SelectionData")]
public class SelectionData : ScriptableObject //for storing the indicator and response selections made by the player, per encounter
{
    public IndicatorSelection indicatorSelection;
    public ResponseSelection responseSelection;
}



[System.Serializable]
public class IndicatorSelection
{
    public bool subjectSelected;
    public bool senderSelected;
    public bool introductionSelected;
    public bool mainbodySelected;
    public bool linkSelected;
    public bool endSelected;
    public bool fileSelected;
}



[System.Serializable]
public class ResponseSelection
{
    public bool emailReply;
    public bool emailDeleted;
    public bool emailReported;
    public bool linkOpened;
    public bool fileDownloaded;
}