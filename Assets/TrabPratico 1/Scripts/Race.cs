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
    [SerializeField] private List<Transform> racePoints;

    private List<Car> _cars = new List<Car>();
    private List<Car> finishedCars = new List<Car>();

    public void PassThroughCheckPoint(RaceCheckPoint check, Car car)
    {
        int index = checkPoints.IndexOf(check);
        int newIndex = (index + 1) % checkPoints.Count;

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
        UIManager.instance.ShowVictoryPanel(car);
    }

    public void StartRace(List<Car> carsPrefabs)
    {
        for(int i = 0; i < carsPrefabs.Count; i++) 
        {
            var car = Instantiate(carsPrefabs[i], racePoints[i].transform.position, racePoints[i].transform.rotation);
            _cars.Add(car);
        }

        InitializeChecks();
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

    private void InitializeChecks()
    {
        bool first = true;
        foreach (var check in checkPoints)
        {
            check.InitializeCheckPoint(_cars, first);
            first = false;
        }
    }
    
}