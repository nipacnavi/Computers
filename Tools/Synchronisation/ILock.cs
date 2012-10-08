using System;
namespace Tools.Synchronisation
{
    public interface ILock : IDisposable
    {
        void Enter();
        void Leave();
        // Permforms Enter and returns itself. To be used with using
        ILock EnterAndReturnLock();
    }
}
