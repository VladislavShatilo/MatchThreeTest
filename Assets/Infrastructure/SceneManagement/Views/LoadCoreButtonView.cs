using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class LoadCoreButtonView : MonoBehaviour
{
    [SerializeField] private Button button;

    private ILoadCoreSceneUseCase useCase;
    private CancellationToken destroyToken;

    [Inject]
    private void Construct(ILoadCoreSceneUseCase useCase)
    {
        this.useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
    }

    private void Start()
    {
        destroyToken = this.GetCancellationTokenOnDestroy();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {

        Execute().Forget();
    }

    private async UniTaskVoid Execute()
    {
        await useCase.Execute(destroyToken);
    }
}