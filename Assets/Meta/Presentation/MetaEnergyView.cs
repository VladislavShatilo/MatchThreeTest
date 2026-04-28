using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using VContainer;

public class MetaEnergyView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI energyText;

    private IGameStateRepository energyService;

    [Inject]
    public void Construct(IGameStateRepository energyService)
    {
        this.energyService = energyService;
    }

    private void Start()
    {
        energyText.text = energyService.NatureEnergy.ToString();
    }

}
