using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Race : MonoBehaviour
{
    public static Race instance { get; private set; }

    [SerializeField] private int totalLaps = 3;
    [SerializeField] private List<RaceCheckPoint> checkPoints;
    [SerializeField] private GameObject finishedPanel;
    [SerializeField] private Text finhsedCarName;

    private List<Car> _cars = new List<Car>();
    private List<Car> finishedCars = new List<Car>();

    public void PassThroughCheckPoint(RaceCheckPoint check, Car car)
    {
        int index = checkPoints.IndexOf(check);
        int newIndex = (index == checkPoints.Count-1) ? 0 : index + 1;

        if(checkPoints[index].timesPassed[car] <= car.Laps)
            checkPoints[index].PassCar(car);

        else if (index == 0 && checkPoints[checkPoints.Count-1].timesPassed[car] == checkPoints[0].timesPassed[car])
        {
            checkPoints[index].PassCar(car);
            car.Laps++;
            if (car.Laps >= totalLaps)
            {
                if(finishedCars.Count == 0)
                    CarWon(car);
                
                finishedCars.Add(car);
            }
        }

        car.CheckPoint = checkPoints[newIndex];

    }

    private void CarWon(Car car)
    {
        finishedPanel.SetActive(true);
        finhsedCarName.text = car.carName+" Won!";
    }

    private void Start()
    {
        InitializeCarsAndChecks();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
            return;
        }

        instance = this;
    }

    private void InitializeCarsAndChecks()
    {
        _cars.AddRange(GameObject.FindObjectsOfType<Car>());

        bool first = true;
        foreach (var check in checkPoints)
        {
            check.InitializeCheckPoint(_cars, first);
            first = false;
        }
    }
    
}