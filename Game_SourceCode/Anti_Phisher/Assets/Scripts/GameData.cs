using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData")]
public class GameData: ScriptableObject //Stores General Game data values for use in multiple scripts
{
    //Color values for indicator selection and deselection
    public Color transparentColor = new Color(0f, 0f, 0f, 0f);
    public Color selectedColor = new Color(0f, 0.75f, 0f, 0.75f);
}
