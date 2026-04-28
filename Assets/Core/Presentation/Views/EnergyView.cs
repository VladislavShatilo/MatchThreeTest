using R3;
using System;
using TMPro;
using UnityEngine;
using VContainer;

public class EnergyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;

    private IGameStateRepository energyService;
    private IDisposable subscription;

    [Inject]
    public void Construct(IGameStateRepository energyService)
    {
        this.energyService = energyService;
    }

    private void Start()
    {
        subscription = energyService.NatureEnergy.Subscribe(UpdateView);
    }

    private void UpdateView(int value)
    {
        energyText.text = value.ToString();
    }

    private void OnDestroy()
    {
        subscription.Dispose();
    }
}