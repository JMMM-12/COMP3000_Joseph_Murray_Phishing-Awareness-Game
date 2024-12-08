using UnityEngine;

[CreateAssetMenu(fileName = "EncounterResults", menuName = "ScriptableObjects/EncounterResults")]
public class EncounterResults : ScriptableObject //Stores the calculated & simplified results for an encounter - for easier feedback determining
{
    public IndicatorResults indicatorResults;
    public ResponseResults responseResults;
}





[System.Serializable]
public class IndicatorResults
{
    public Result subjectResult; //Result Enums which hold the result of the particular indicator
    public Result senderResult;
    public Result introductionResult;
    public Result mainbodyResult;
    public Result linkResult;
    public Result endResult;
    public Result fileResult;
    public int totalIndicators; //Total number of phishing indicators present in the email
    public int correctIndicators; //Total number of phishing indicators correctly selected by the player
    public int incorrectIndicators; //Total number of indicators incorrectly identifed by the player
    public int missedIndicators; //Total number of indicators missed by the player
    public Grade indicatorGrade; //Grade value that summarizes the indicator results
    public bool indicatorsIncorrect; //Determines whether there are any indicators incorrectly identified by the player
}




[System.Serializable]
public class ResponseResults
{
    public Result linkResult; //Result Enums which hold the result of the particular response type
    public Result fileResult;
    public Result replyResult;
    public Result deleteResult;
    public Result reportResult;
    public bool responseCorrect; //Determies whether the response was correct
}




public enum Result //Result metrics to simplify and contain the results for each indicator and response type
{
    TruePositive,
    TrueNegative,
    FalsePositive,
    FalseNegative
}




public enum Grade
{
    All,
    Some,
    None,
    Unrequired,
}
