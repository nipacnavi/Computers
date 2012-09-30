using System;
namespace Tools.Synchronisation
{
    public interface ILock : IDisposable
    {
        void Enter();
        void Leave();
    }
}
