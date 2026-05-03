using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;

public class GridRaycaster : IGridRaycaster
{
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;
    private GridView gridView;

    [Inject]
    private void Construct(GraphicRaycaster raycaster, EventSystem eventSystem, GridView gridView)
    {
        this.raycaster = raycaster ?? throw new ArgumentNullException(nameof(raycaster));
        this.eventSystem = eventSystem ?? throw new ArgumentNullException(nameof(eventSystem));
        this.gridView = gridView ?? throw new ArgumentNullException(nameof(gridView));
    }

    public bool TryGetGridPosition(ScreenPoint screenPoint, out GridPosition position)
    {
        var eventData = new PointerEventData(eventSystem)
        {
            position = new Vector2(screenPoint.X, screenPoint.Y)
        };

        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            var view = result.gameObject.GetComponentInParent<CellView>();
            if (view != null)
                return gridView.TryGetGridPosition(view, out position);
        }

        position = default;
        return false;
    }
}
