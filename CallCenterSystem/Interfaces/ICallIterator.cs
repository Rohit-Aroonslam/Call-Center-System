using CallCenterSystem.Models;

namespace CallCenterSystem.Interfaces
{
    // Iterator
    public interface ICallIterator
    {
        void First();
        void Next();
        bool IsDone();
        Call? CurrentItem();
    }
}
