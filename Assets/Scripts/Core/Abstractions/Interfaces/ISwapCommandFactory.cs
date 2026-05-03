public interface ISwapCommandFactory
{
    ISwapCommand Create(SwapRequest request);
}
