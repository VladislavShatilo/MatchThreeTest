using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class CoreEntryPoint : IStartable
{
    private IGridService gridService;

    [Inject]
    private void Construct(IGridService gridService)
    {
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
    }

    public void Start()
    {
        gridService.Initialize(8, 8);
    }
}