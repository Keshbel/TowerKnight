using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public static bool IsGamePaused;
    
    [Header("Buttons")] 
    [SerializeField] private Button continueButton;
    [SerializeField] private Button mainMenuButton;

    [Header("Tweens")] 
    public GameObject progressPanel;
    public GameObject darkness;
    public bool isOpenPanel;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerScale;
    
    private void Awake()
    {
        continueButton.onClick.AddListener(ClosePanel);
        mainMenuButton.onClick.AddListener(()=>SceneManager.LoadScene(0));
        if (isOpenPanel)
            OpenPanel();
        else
            ClosePanel();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Cancel") > 0)
            OpenPanel();
    }

    private void OpenPanel()
    {
        _tweenerScale?.Kill();
        Panel(true);
        _tweenerScale = progressPanel.transform.DOScale(1f, 0.15f).OnComplete(Pause);
    }

    public void ClosePanel()
    {
        _tweenerScale?.Kill();
        Play();
        _tweenerScale = progressPanel.transform.DOScale(0f, 0.15f).OnComplete(() =>
        {
            Panel(false);
        });
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
    
    private void Panel(bool isOn)
    {
        darkness.SetActive(isOn);
        progressPanel.SetActive(isOn);
        isOpenPanel = isOn;
    }
}
