using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Keeps track of the player's progress throughout the whole game, counting deaths, collectibles picked up, etc
/// </summary>
public class ProgressController : MonoBehaviour
{
    private Progress progress;
    public GameplayUIManager gameplayUIManager;

    // Start is called before the first frame update
    void Start()
    {
        progress = new Progress();
    }

    public void incrementDeathCount()
    {
        progress.deathCount++;
    }

    public int getDeathCount()
    {
        return progress.deathCount;
    }

    public void incrementCollectiblePickupCount()
    {
        progress.collectiblesPickedUp++;
        gameplayUIManager.SetPickupCount(progress.collectiblesPickedUp);
    }

    public int getCollectiblesPickedUp()
    {
        return progress.collectiblesPickedUp;
    }

    public void SaveProgress(Progress progress)
    {
        String jsonSave = JsonUtility.ToJson(progress);
        String path = "saveFiles/saveFile.txt";
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(jsonSave);
        writer.Close();
    }

    private Progress LoadProgress()
    {
        StreamReader reader = new StreamReader("saveFiles/saveFile.txt");
        String loadJson = reader.ReadToEnd();
        return JsonUtility.FromJson<Progress>(loadJson);
    }

    [Serializable]
    public class Progress
    {
        public int deathCount;
        public int collectiblesPickedUp;

        public Progress()
        {
            deathCount = 0;
            collectiblesPickedUp = 0;
        }
    }
}