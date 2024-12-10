using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FeedbackDialogues", menuName = "ScriptableObjects/FeedbackDialogues")]
public class FeedbackDialogues : ScriptableObject
{
    public List<FDialogues> feedbackDialogues;
}


[System.Serializable]
public class FDialogues
{
    public string ChipModel;
    public string FeedbackText;

    public FDialogues(string newModel, string newText)
    {
        ChipModel = newModel;
        FeedbackText = newText;
    }
}
