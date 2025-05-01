using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VictoryPanel : Panel
{
    [SerializeField] private TMP_Text winnerText;

    public void SetWinner(Car car) 
    {
        winnerText.text = car.carName + " Won!";
    }
}
