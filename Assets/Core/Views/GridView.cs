using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using VContainer;

public class GridView : MonoBehaviour
{
    private IGridService gridService;
    private ITileFactory tileFactory;

    [Inject]
    private void Construct(IGridService gridService, ITileFactory tileFactory)
    {
       
        this.gridService = gridService ?? throw new ArgumentNullException(nameof(gridService));
        this.tileFactory = tileFactory ?? throw new ArgumentNullException(nameof(tileFactory));
    }

    private async void Start()
    {
        await Build();
    }

    private async UniTask Build()
    {

        var grid = gridService.Grid;
        

        for (int x = 0; x < grid.Width; x++)
            for (int y = 0; y < grid.Height; y++)
            {
                var cell = grid.Get(x, y);

                var tile = await tileFactory.Create(cell.Type);
                Debug.Log(transform.localScale);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x, y, 0);
            }
    }
}