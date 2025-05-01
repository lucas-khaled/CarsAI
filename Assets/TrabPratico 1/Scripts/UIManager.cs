using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CarSelectionPanel carSelectionPanel;
    [SerializeField] private VictoryPanel victoryPanel;

    private Panel activePanel;

    public static UIManager instance;

    private void Awake()
    {
        if(instance != null) 
        {
            Destroy(this);
            return;
        }

        instance = this;
        carSelectionPanel.OnContinueClicked += OnContinuedFromSelection;
    }

    private void OnContinuedFromSelection(List<Car> cars)
    {
        HideActive();
        Race.instance.StartRace(cars);
    }

    private void Start()
    {
        ShowSelectionPanel();
    }

    public void ShowSelectionPanel() 
    {
        SetActive(carSelectionPanel);   
    }

    public void ShowVictoryPanel(Car winner) 
    {
        victoryPanel.SetWinner(winner);
        SetActive(victoryPanel);
    }

    private void SetActive(Panel panel)
    {
        if (activePanel != null)
            activePanel.Hide();

        activePanel = panel;
        activePanel.Show();
    }

    public void HideActive() 
    {
        activePanel.Hide();
    }
}
