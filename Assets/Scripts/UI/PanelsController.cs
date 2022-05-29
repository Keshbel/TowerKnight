using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PanelsController : MonoBehaviour
{
    public List<Panel> panels;
    // Start is called before the first frame update
    private void OnEnable()
    {
        foreach (var panel in panels)
        {
            panel.OnPanelStateChange += CheckPanelState;
        }
    }
    private void OnDisable()
    {
        foreach (var panel in panels)
        {
            panel.OnPanelStateChange -= CheckPanelState;
        }
    }

    private void CheckPanelState()
    {
        //проверяем на открытость панели для паузы
        var isOpen = panels.Any(p => p.isOpen);
        if (isOpen)
        {
            PauseScript.Pause();
        }
        else
        {
            PauseScript.Play();
        }
    }
}
