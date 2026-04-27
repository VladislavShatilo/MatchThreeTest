using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using System;
public class LoadMetaButtonView : MonoBehaviour
{
    [SerializeField] private Button button;

    private ILoadMetaSceneUseCase useCase;
    private CancellationToken destroyToken;

    [Inject]
    private void Construct(ILoadMetaSceneUseCase useCase)
    {
        this.useCase = useCase ?? throw new ArgumentNullException(nameof(useCase));
    }
    private void Awake()
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
