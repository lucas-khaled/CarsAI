using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CarSelectionPanel : Panel
{
    [SerializeField] private Car[] possibleCars;
    [SerializeField][Min(0)] private int startingCarsCount = 3;
    [SerializeField] private CarConfigItem configItemPrefab;

    [Header("UI")]
    [SerializeField] private Transform itensContent;
    [SerializeField] private Button addNewButton;
    [SerializeField] private Button continueButton;

    private List<CarConfigItem> activeConfigurables = new List<CarConfigItem>();
    private Stack<CarConfigItem> deactiveConfigurables = new Stack<CarConfigItem>();

    public Action<List<Car>> OnContinueClicked;

    private void Awake()
    {
        CreateItems();
        ConfigAddButton();
        ConfigContinueButton();
    }

    private void ConfigContinueButton()
    {
        continueButton.onClick.AddListener(() => OnContinueClicked?.Invoke(
                                                activeConfigurables.Select(x => x.Car).ToList()));
    }

    private void ConfigAddButton()
    {
        addNewButton.onClick.AddListener(OnAddNewCar);
        CheckAllButtonsActivation();
    }

    private void CheckAllButtonsActivation()
    {
        CheckAddNewButtonActivation();
        CheckContinueButtonActivation();
    }

    private void OnAddNewCar()
    {
        var item = deactiveConfigurables.Pop();
        item.gameObject.SetActive(true);
        activeConfigurables.Add(item);
        item.transform.SetSiblingIndex(itensContent.childCount - 2);

        CheckAllButtonsActivation();
    }

    private void CreateItems()
    {
        int index = 0;
        foreach (Car car in possibleCars)
        {
            var item = Instantiate(configItemPrefab, itensContent);
            item.transform.SetSiblingIndex(itensContent.childCount - 2);
            item.SetCar(car);
            item.OnRemoveCicked += OnRemoved;

            if (index < startingCarsCount)
            {
                activeConfigurables.Add(item);
                index++;
                continue;
            }

            item.gameObject.SetActive(false);
            deactiveConfigurables.Push(item);
        }
    }

    private void OnRemoved(CarConfigItem item)
    {
        item.gameObject.SetActive(false);

        activeConfigurables.Remove(item);
        deactiveConfigurables.Push(item);

        CheckAllButtonsActivation();
    }

    private void CheckAddNewButtonActivation() 
    {
        addNewButton.gameObject.SetActive(deactiveConfigurables.Count > 0);
    }

    private void CheckContinueButtonActivation()
    {
        continueButton.gameObject.SetActive(activeConfigurables.Count > 0);
    }
}
