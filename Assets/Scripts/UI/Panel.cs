using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.EventSystems;

public class Panel : MonoBehaviour, IPointerClickHandler
{
    [Header("Tweens")] 
    public GameObject progressPanel;
    public bool isOpenPanel;
    private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerScale;

    private void Awake()
    {
        progressPanel = gameObject;
        if (isOpenPanel)
            OpenPanel();
        else
            ClosePanel();
    }

    private void OpenPanel()
    {
        _tweenerScale?.Kill();
        PanelState(true);
        _tweenerScale = progressPanel.transform.DOScale(1f, 0.15f).OnComplete(PauseScript.Pause);
    }

    public void ClosePanel()
    {
        _tweenerScale?.Kill();
        PauseScript.Play();
        _tweenerScale = progressPanel.transform.DOScale(0f, 0.15f).OnComplete(() =>
        {
            PanelState(false);
        });
    }

    private void PanelState(bool isOn)
    {
        progressPanel.SetActive(isOn);
        isOpenPanel = isOn;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClosePanel();
    }
}
