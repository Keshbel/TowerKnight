using UnityEngine;
using UnityEngine.UI;

public class ProgressScript : MonoBehaviour
{
   [Header("Main")] 
   public Panel panel;
   
   [Header("Health")] 
   public Health heroHealth;
   [SerializeField] private Button heartPlusButton;

   private void OnEnable()
   {
      heartPlusButton.onClick.AddListener(AddHeart);
      ProgressData.LevelChanged += panel.OpenPanel;
   }
   private void OnDisable()
   {
      ProgressData.LevelChanged -= panel.OpenPanel;
   }

   private void Update()
   {
      if (Input.GetKeyDown(KeyCode.P)) //cheatcode
      {
         ProgressData.AddProgressPoint(5);
      }
   }

   private void AddHeart()
   {
      heroHealth.maxHealth++;
      heroHealth.ChangeHealth(1);
      
      panel.ClosePanel();
   }
}
