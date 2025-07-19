using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct LevelDataStruct
{
    public string levelName;
    public int columns;
    public float cardWidth;
    public float cardSpacing;
    public List<Sprite> cardImages;

}


[CreateAssetMenu(menuName = "CardGame/LevelData")]
public class LevelData : ScriptableObject
{
  public List<LevelDataStruct> levels;
}