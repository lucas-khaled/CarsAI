using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceCheckPoint : MonoBehaviour
{
    public Dictionary<Car, int> timesPassed { get; private set; }

    public void InitializeCheckPoint(List<Car> cars, bool first = false)
    {
        timesPassed = new Dictionary<Car, int>();
        int initalCount = (first) ? 1 : 0;
        foreach (Car car in cars)
        {
            timesPassed.Add(car, initalCount);
        }
    }

    public void PassCar(Car car)
    {
        timesPassed[car]++;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Carro"))
        {
            Car car = other.GetComponent<Car>();
            Race.instance.PassThroughCheckPoint(this, car);
        }
    }
}
