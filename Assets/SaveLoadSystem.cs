using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public static void SaveProgress(string sceneName, int unlockedLevel)
    {
        PlayerPrefs.SetString("LastScene", sceneName);
        PlayerPrefs.SetInt("UnlockedLevel", unlockedLevel);
        PlayerPrefs.Save();
    }

    public static void SaveLevelStats(
        string levelID,
        int percent,
        int found,
        int total)
    {
        PlayerPrefs.SetInt(levelID + "_Percent", percent);
        PlayerPrefs.SetInt(levelID + "_Found", found);
        PlayerPrefs.SetInt(levelID + "_Total", total);
        PlayerPrefs.Save();
    }

    public static int GetUnlockedLevel()
    {
        return PlayerPrefs.GetInt("UnlockedLevel", 1);
    }

    public static string GetLastScene()
    {
        return PlayerPrefs.GetString(
            "LastScene",
            "Level1- Tutorial Lobby"
        );
    }

    public static int GetStat(string key)
    {
        return PlayerPrefs.GetInt(key, 0);
    }

    public static void ResetSave()
    {
        PlayerPrefs.DeleteAll();
    }
}