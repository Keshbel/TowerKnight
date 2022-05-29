using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CanvasGroup))]
public class Panel : MonoBehaviour, IPointerClickHandler
{
    [Header("Main")] 
    public GameObject panel;
    public CanvasGroup canvasGroup;

    public float duration = 0.35f;
    public bool isOpen;
    
    private Tweener _tweenerScale;
    private Tweener _tweenerFade;

    public Action OnPanelStateChange;

    private void Awake()
    {
        if (!panel)
            panel = gameObject;

        if (!canvasGroup)
            canvasGroup = GetComponent<CanvasGroup>();
        
        if (isOpen)
            OpenPanel();
        else
            ClosePanel();
    }

    public void OpenPanel()
    {
        _tweenerScale?.Complete();

        _tweenerScale = panel.transform.DOScale(1f, duration).OnComplete(()=>
        {
            isOpen = true;
            OnPanelStateChange?.Invoke();
        }).SetUpdate(true);
        
        TweenFadeDarkness(1);
    }

    public void ClosePanel()
    {
        _tweenerScale?.Complete();

        _tweenerScale = panel.transform.DOScale(0f, duration).OnComplete(() =>
        {
            isOpen = false;
            OnPanelStateChange?.Invoke();
        }).SetUpdate(true);
        
        TweenFadeDarkness(0);
    }

    public void TweenFadeDarkness(float value)
    {
        _tweenerFade?.Kill();
        _tweenerFade = canvasGroup.DOFade(value, duration).SetUpdate(true);
        if (value > 0)
        {
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ClosePanel();
    }
}
