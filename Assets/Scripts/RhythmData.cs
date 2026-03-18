using System.Collections.Generic;

[System.Serializable]
public class RhythmNote
{
    public float time;
    public int lane;
    public string lane_name;
    public int velocity;
    public float strength;
}

[System.Serializable]
public class RhythmData
{
    public float tempo;
    public List<float> events;
    public string difficulty;
    public int note_count;
    public List<RhythmNote> notes;
}