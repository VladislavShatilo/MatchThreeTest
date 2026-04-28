public struct GridAnimationCompletedEvent
{
    public GridAnimationPhase Phase;

    public GridAnimationCompletedEvent(GridAnimationPhase phase)
    {
        Phase = phase;
    }
}