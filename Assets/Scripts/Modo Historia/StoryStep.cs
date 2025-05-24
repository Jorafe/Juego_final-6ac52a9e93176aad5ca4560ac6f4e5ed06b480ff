[System.Serializable]
public class StoryStep
{
    public enum StepType { Cutscene, Level }
    public StepType type;

    public int cutsceneIndex; // Usado si es Cutscene
    public string levelSceneName; // Usado si es Level
}