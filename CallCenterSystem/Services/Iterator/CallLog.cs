using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;

namespace CallCenterSystem.Services.Iterator
{
    // ConcreteAggregate
    public class CallLog : ICallCollection
    {
        private readonly List<Call> _calls = new();

        public void AddCall(Call call) => _calls.Add(call);

        public ICallIterator CreateIterator() => new CallLogIterator(_calls);
    }
}
