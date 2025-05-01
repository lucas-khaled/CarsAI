using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarConfigItem : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private TMP_Text carNameText;
    [SerializeField] private Image carImage;

    [Header("Inputs")]
    [SerializeField] private TMP_InputField maxVelocityText;
    [SerializeField] private TMP_InputField breakDistanceText;
    [SerializeField] private TMP_InputField carSteeringForceText;
    [SerializeField] private TMP_InputField breakForceText;

    [Header("Buttons")]
    [SerializeField] private Button removeButton;

    public Action<CarConfigItem> OnRemoveCicked;
    public Car Car { get; private set; }
    private CarAI carAI;

    private void Awake()
    {
        InitializeInputs();

        removeButton.onClick.AddListener(() => OnRemoveCicked?.Invoke(this));
    }

    private void InitializeInputs()
    {
        maxVelocityText.contentType = TMP_InputField.ContentType.DecimalNumber;
        maxVelocityText.onEndEdit.AddListener(OnMaxVelocityChanged);

        breakDistanceText.contentType = TMP_InputField.ContentType.DecimalNumber;
        breakDistanceText.onEndEdit.AddListener(OnBreakDistanceTextChanged);

        carSteeringForceText.contentType = TMP_InputField.ContentType.DecimalNumber;
        carSteeringForceText.onEndEdit.AddListener(OnCarSteeringForceTextChanged);

        breakForceText.contentType = TMP_InputField.ContentType.DecimalNumber;
        breakForceText.onEndEdit.AddListener(OnBreakForceTextChanged);
    }

    private void OnBreakForceTextChanged(string value)
    {
        carAI.breakForce = Convert.ToSingle(value);
    }

    private void OnCarSteeringForceTextChanged(string value)
    {
        carAI.carSteeringForce = Convert.ToSingle(value);
    }

    private void OnBreakDistanceTextChanged(string value)
    {
        carAI.breakDistance = Convert.ToSingle(value);
    }

    private void OnMaxVelocityChanged(string value)
    {
        carAI.maxVelocity = Convert.ToSingle(value);
    }

    public void SetCar(Car car) 
    {
        this.carAI = car.GetComponent<CarAI>();
        this.Car = car;

        maxVelocityText.text = carAI.maxVelocity.ToString();
        breakDistanceText.text = carAI.breakDistance.ToString();
        carSteeringForceText.text = carAI.carSteeringForce.ToString();
        breakForceText.text = carAI.breakForce.ToString();

        carNameText.text = car.carName;
        carImage.sprite = car.carImage;
    }


}
