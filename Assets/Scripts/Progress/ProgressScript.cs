using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.UI;

public class ProgressScript : MonoBehaviour
{
   [Header("ChangesParameters")]
   public Health heroHealth;

   [Header("Buttons")] 
   [SerializeField] private Button heartPlusButton;

   [Header("Tweens")] 
   public GameObject progressPanel;
   public GameObject darkness;
   public bool isOpenPanel;
   private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerScale;

   private void Awake()
   {
      heartPlusButton.onClick.AddListener(AddHeart);
      ProgressData.LevelChanged += OpenPanel;
      if (isOpenPanel)
         OpenPanel();
      else
         ClosePanel();
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.P)) //cheatcode
      {
         ProgressData.AddProgressPoint(5);
      }
   }

   private void OnDestroy()
   {
      _tweenerScale?.Kill();
      ProgressData.LevelChanged -= OpenPanel;
   }

   private void AddHeart()
   {
      ClosePanel();
      heroHealth.maxHealth++;
      heroHealth.ChangeHealth(1);
   }

   private void OpenPanel()
   {
      _tweenerScale?.Kill();
      Panel(true);
      _tweenerScale = progressPanel.transform.DOScale(1f, 0.15f).OnComplete(PauseScript.Pause);
   }

   public void ClosePanel()
   {
      _tweenerScale?.Kill();
      PauseScript.Play();
         _tweenerScale = progressPanel.transform.DOScale(0f, 0.15f).OnComplete(() =>
      {
         Panel(false);
      });
   }

   private void Panel(bool isOn)
   {
      darkness.SetActive(isOn);
      progressPanel.SetActive(isOn);
      isOpenPanel = isOn;
   }
}
