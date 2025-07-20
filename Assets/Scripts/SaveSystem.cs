using UnityEngine;

public static class SaveSystem
{
    private const string UnlockedLevelKey = "UnlockedLevelIndex";

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

    public static void ResetProgress()
    {
        PlayerPrefs.DeleteKey(UnlockedLevelKey);
    }
}