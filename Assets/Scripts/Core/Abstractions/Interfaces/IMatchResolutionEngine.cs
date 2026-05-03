public interface IMatchResolutionEngine
{
    bool TryBuildNextStep(GridModel grid, out MatchResolutionStep step);
    bool TryReshuffle(GridModel grid, out GridSnapshot reshuffledGrid);
}
