using CallCenterSystem.Models;

namespace CallCenterSystem.Interfaces
{
    // Aggregate
    public interface ICallCollection
    {
        ICallIterator CreateIterator();
        void AddCall(Call call);
    }
}
