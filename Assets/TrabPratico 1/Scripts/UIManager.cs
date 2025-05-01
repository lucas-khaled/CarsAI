using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CarSelectionPanel carSelectionPanel;

    private Panel activePanel;

    private void Awake()
    {
        carSelectionPanel.OnContinueClicked += OnContinuedFromSelection;
    }

    private void OnContinuedFromSelection(List<Car> cars)
    {
        HideActive();
        Race.instance.StartRace(cars);
    }

    private void Start()
    {
        StartSelectionPanel();
    }

    public void StartSelectionPanel() 
    {
        if (activePanel != null)
            activePanel.Hide();

        activePanel = carSelectionPanel;
        activePanel.Show();
    }

    public void HideActive() 
    {
        activePanel.Hide();
    }
}
