using UnityEngine;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    public static StoryManager Instance;

    public StoryFlow storyFlow;
    private int currentStepIndex = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartNewStory()
    {
        currentStepIndex = 0;
        SaveProgress();
        LoadCurrentStep();
    }

    public void ContinueStory()
    {
        LoadProgress();
        LoadCurrentStep();
    }

    public void LoadNextStep()
    {
        currentStepIndex++;
        if (currentStepIndex < storyFlow.steps.Count)
        {
            SaveProgress();
            LoadCurrentStep();
        }
        else
        {
            Debug.Log("Modo historia completo");
            PlayerPrefs.DeleteKey("StoryStep");
        }
    }

    private void LoadCurrentStep()
    {
        var step = storyFlow.steps[currentStepIndex];
        if (step.type == StoryStep.StepType.Cutscene)
        {
            SceneManager.LoadScene("CutsceneScene");
        }
        else if (step.type == StoryStep.StepType.Level)
        {
            SceneManager.LoadScene(step.levelSceneName);
        }
    }

    public int GetCurrentCutsceneIndex()
    {
        return storyFlow.steps[currentStepIndex].cutsceneIndex;
    }

    private void SaveProgress()
    {
        PlayerPrefs.SetInt("StoryStep", currentStepIndex);
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        currentStepIndex = PlayerPrefs.GetInt("StoryStep", 0);
    }
}
