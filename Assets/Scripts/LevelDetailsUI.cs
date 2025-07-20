using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelDetailsUI : MonoBehaviour
{
    public Transform levelContentTransform;
    public GameObject levelButtonPrefab;

    public void LoadLevelButttons()
    {
        foreach (Transform child in levelContentTransform)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < GameManager.Instance.levelsData.levels.Count; i++)
        {
            var levelData = GameManager.Instance.levelsData.levels[i];
            var levelButtonObj = Instantiate(levelButtonPrefab, levelContentTransform);
            var levelButton = levelButtonObj.GetComponent<LevelButton>();
            if (levelButton != null)
            {
                levelButton.SetLevelIndex(i, levelData.levelName);
                levelButton.SetLocked(SaveSystem.GetUnlockedLevelIndex() < i);
            }
        }
    }
}
