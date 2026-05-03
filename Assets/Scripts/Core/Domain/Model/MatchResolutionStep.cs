using System.Collections.Generic;

public class MatchResolutionStep
{
    public List<MatchGroup> Matches { get; } = new();
    public ResolutionStepPlan Plan { get; } = new();
}
