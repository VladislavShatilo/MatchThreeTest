using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SwapAcceptedEvent
{
    public readonly SwapRequest Request;

    public SwapAcceptedEvent(SwapRequest request)
    {
        Request = request;
    }
}