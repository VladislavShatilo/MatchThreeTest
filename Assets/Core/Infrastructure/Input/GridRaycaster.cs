using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using System;
public class GridRaycaster : IGridRaycaster
{
    private GraphicRaycaster raycaster;
    private EventSystem eventSystem;

    [Inject]
    private void Construct(GraphicRaycaster raycaster, EventSystem eventSystem)
    {
        this.raycaster = raycaster ?? throw new ArgumentNullException(nameof(raycaster));
        this.eventSystem = eventSystem ?? throw new ArgumentNullException(nameof(eventSystem));
    }

    public bool TryGetCellView(Vector3 screenPosition, out CellView cellView)
    {
        var eventData = new PointerEventData(eventSystem)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        raycaster.Raycast(eventData, results);

        foreach (var result in results)
        {
            var view = result.gameObject.GetComponentInParent<CellView>();
            if (view != null)
            {
                cellView = view;
                return true;
            }
        }

        cellView = null;
        return false;
    }
}
