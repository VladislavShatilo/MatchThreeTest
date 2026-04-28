public interface IInputLockService
{
    bool IsLocked { get; }
    void Lock();
    void Unlock();
}