using System;
using TMPro;
using UnityEngine;

public class InfoPanelScript : MonoBehaviour
{
   [Header("Main")] 
   public Panel panel;
   public ObjectBottomSpawner objectBottomSpawner;
   public TextMeshProUGUI textDescription;
   
   private void OnEnable()
   {
      ObjectBottomSpawner.OnRandomAddedObject += panel.OpenPanel;
      ObjectBottomSpawner.OnRandomAddedObject += TextAddedObject;
   }
   private void OnDisable()
   {
      ObjectBottomSpawner.OnRandomAddedObject -= panel.OpenPanel;
      ObjectBottomSpawner.OnRandomAddedObject -= TextAddedObject;
   }

   private void TextAddedObject()
   {
      textDescription.text = "Добавлен объект " + objectBottomSpawner.objectPrefubsCurrent[objectBottomSpawner.objectPrefubsCurrent.Count-1].GetComponent<ObjectScript>().objName;
   }
}
