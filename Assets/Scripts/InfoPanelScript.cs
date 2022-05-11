using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

public class InfoPanelScript : MonoBehaviour
{
   public ObjectBottomSpawner objectBottomSpawner;

   [Header("Texts")] 
   public TextMeshProUGUI textDescription;
   
   [Header("Tweens")] public GameObject progressPanel;
   public GameObject darkness;
   public bool isOpenPanel;
   private TweenerCore<Vector3, Vector3, VectorOptions> _tweenerScale;

   private void Start()
   {
      ProgressData.LevelChanged += OpenPanel;
   }

   private void OnDestroy()
   {
      _tweenerScale?.Kill();
      ProgressData.LevelChanged -= OpenPanel;
   }

   private void OpenPanel()
   {
      _tweenerScale?.Kill();
      Panel(true);
      _tweenerScale = progressPanel.transform.DOScale(1f, 0.15f).OnComplete(PauseScript.Pause);
      textDescription.text = "Добавлен объект " + objectBottomSpawner.objectPrefubsCurrent[objectBottomSpawner.objectPrefubsCurrent.Count-1].name;
   }

   public void ClosePanel()
   {
      _tweenerScale?.Kill();
      PauseScript.Play();
      _tweenerScale = progressPanel.transform.DOScale(0f, 0.15f).OnComplete(() => { Panel(false); });
   }

   private void Panel(bool isOn)
   {
      darkness.SetActive(isOn);
      progressPanel.SetActive(isOn);
      isOpenPanel = isOn;
   }
}
