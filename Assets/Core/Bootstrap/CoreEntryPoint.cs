using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class CoreEntryPoint : IStartable
{
    private IGridService gridService;
    private IGridFactory gridFactory;

    [Inject]
    private void Construct(IGridService gridService, IGridFactory gridFactory)
    {
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.gridFactory = gridFactory ?? throw new ArgumentNullException(nameof(gridFactory));
    }

    public void Start()
    {
        var gridModel = gridFactory.Create(8, 8);
        gridService.Initialize(gridModel);
    }
}