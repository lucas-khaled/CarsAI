using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VictoryPanel : Panel
{
    [SerializeField] private TMP_Text winnerText;
    [SerializeField] private Button continueButton;

    public Action OnContinueClicked;

    private void Awake()
    {
        continueButton.onClick.AddListener(() => OnContinueClicked?.Invoke());
    }

    public void SetWinner(Car car) 
    {
        winnerText.text = car.carName + " Won!";
    }
}
