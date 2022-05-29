using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [Header("Main")] 
    public Panel panel;
    public static bool IsGamePaused;
    
    [Header("Buttons")] 
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;

    private void OnEnable()
    {
        continueButton.onClick.AddListener(panel.ClosePanel);
        mainMenuButton.onClick.AddListener(()=>SceneManager.LoadScene(0));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cancel") > 0)
            panel.OpenPanel();
    }

    public static void Pause()
    {
        Time.timeScale = 0;
        IsGamePaused = true;
    }
    public static void Play()
    {
        Time.timeScale = 1;
        IsGamePaused = false;
    }
}
