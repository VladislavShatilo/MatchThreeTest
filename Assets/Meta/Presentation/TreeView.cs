using R3;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;

public class TreeView : MonoBehaviour
{
    [SerializeField] private Renderer targetRenderer;
    [SerializeField] private int maxEnergy = 10000;

    private IGameStateRepository gameStateRepository;
    private MaterialPropertyBlock mpb;

    private readonly int EnergyID = Shader.PropertyToID("_Energy");

    [Inject]
    public void Construct(IGameStateRepository gameStateRepository)
    {
        this.gameStateRepository = gameStateRepository ?? throw new ArgumentNullException(nameof(gameStateRepository));

        
    }
    private void Start()
    {
        mpb = new MaterialPropertyBlock();

        gameStateRepository.NatureEnergy.Subscribe(OnEnergyChanged).AddTo(this);
    }


    private void OnEnergyChanged(int energy)
    {
        float normalized = Mathf.Clamp01((float)energy / maxEnergy);

        targetRenderer.GetPropertyBlock(mpb);
        mpb.SetFloat(EnergyID, normalized);
        targetRenderer.SetPropertyBlock(mpb);
    }
}
