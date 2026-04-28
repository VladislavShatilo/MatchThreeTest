public struct ResolutionPlanCreatedEvent
{
    public ResolutionStepPlan Plan;

    public ResolutionPlanCreatedEvent(ResolutionStepPlan plan)
    {
        Plan = plan;
    }
}
