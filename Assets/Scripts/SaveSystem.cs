using UnityEngine;

public static class SaveSystem
{
    private const string UnlockedLevelKey = "UnlockedLevelIndex";
    private const string scoreKey = "Score";
    private const string levelTurn = "LevelTurn";
    private const string levelCombo = "LevelCombo";

    public static void SaveScore(int score)
    {
        PlayerPrefs.SetInt(scoreKey, score);
        PlayerPrefs.Save();
    }

    public static int GetSavedScore()
    {
        return PlayerPrefs.GetInt(scoreKey, 0); // Default score is 0
    }

    public static int GetUnlockedLevelIndex()
    {
        return PlayerPrefs.GetInt(UnlockedLevelKey, 0); // 0 = only level 0 unlocked
    }

    public static void UnlockNextLevel(int currentLevel)
    {
        int highest = GetUnlockedLevelIndex();
        if (currentLevel + 1 > highest)
        {
            PlayerPrefs.SetInt(UnlockedLevelKey, currentLevel + 1);
            PlayerPrefs.Save();
        }
    }

    public static void SaveLevelTurn(int levelID, int turn)
    {
        PlayerPrefs.SetInt(levelTurn + levelID, turn);
        PlayerPrefs.Save();
    }

    public static int GetSavedLevelTurn(int levelID)
    {
        return PlayerPrefs.GetInt(levelTurn + levelID, 0);
    }
    public static void SaveLevelCombo(int levelID, int combo)
    {
        PlayerPrefs.SetInt(levelCombo + levelID, combo);
        PlayerPrefs.Save();
    }
    public static int GetSavedLevelCombo(int levelID)
    {
        return PlayerPrefs.GetInt(levelCombo + levelID, 0);
    }

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    }
}