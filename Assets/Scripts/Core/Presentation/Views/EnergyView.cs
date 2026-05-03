using R3;
using System;
using TMPro;
using UnityEngine;
using VContainer;

public class EnergyView : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI energyText;

    private IEnergyStateReader state;
    private IDisposable subscription;

    private void Awake()
    {
        if (energyText == null)
            energyText = GetComponentInChildren<TextMeshProUGUI>();
    }

    [Inject]
    public void Construct(IEnergyStateReader state)
    {
        this.state = state ?? throw new ArgumentNullException(nameof(state));
    }

    protected virtual void Start()
    {
        if (state == null)
        {
            Debug.LogError($"{nameof(EnergyView)} on {name} was not constructed with {nameof(IEnergyStateReader)}.", this);
            enabled = false;
            return;
        }

        if (energyText == null)
        {
            Debug.LogError($"{nameof(EnergyView)} on {name} could not find a {nameof(TextMeshProUGUI)} reference.", this);
            enabled = false;
            return;
        }

        subscription = state.Observe.Subscribe(UpdateView);
        UpdateView(state.Current);

    }


    protected virtual void UpdateView(int value)
    {
        energyText.text = value.ToString();
    }

    private void OnDestroy()
    {
        subscription?.Dispose();
    }
}
