public sealed class InputLockService : IInputLockService
{
    public bool IsLocked { get; private set; }

    public void Lock() => IsLocked = true;
    public void Unlock() => IsLocked = false;
}