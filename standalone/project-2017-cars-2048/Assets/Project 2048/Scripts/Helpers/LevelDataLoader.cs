using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[Serializable]
public class StoryData
{
    public ActData[] acts;
}

[Serializable]
public class ActData
{
    public int id;
    public string name;
    public ChapterData[] chapters;
}

[Serializable]
public class ChapterData
{
    public int id;
    public int require;
    public int objective;
    public bool unlimited;
    public string path;
}

class LevelDataLoader
{
    public string JSONPath = "levels";

    public StoryData Load() {
        TextAsset txt = Resources.Load<TextAsset>(JSONPath);
        StoryData data = JsonUtility.FromJson<StoryData>(txt.text);
        return data;
    }
}

