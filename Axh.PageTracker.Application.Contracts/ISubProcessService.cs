namespace Axh.PageTracker.Application.Contracts
{
    using System;

    public interface ISubProcessService : IDisposable
    {
        int Initialize(string[] args);
    }
}