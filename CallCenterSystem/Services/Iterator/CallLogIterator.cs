using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;

namespace CallCenterSystem.Services.Iterator
{
    // ConcreteIterator
    public class CallLogIterator : ICallIterator
    {
        private readonly List<Call> _calls;
        private int _position;

        public CallLogIterator(List<Call> calls)
        {
            _calls = calls;
            _position = 0;
        }

        public void First() => _position = 0;

        public void Next() => _position++;

        public bool IsDone() => _position >= _calls.Count;

        public Call? CurrentItem() =>
            IsDone() ? null : _calls[_position];

        // Supports the "search for a call" requirement without exposing the list itself
        public Call? Find(int callId)
        {
            for (First(); !IsDone(); Next())
            {
                if (CurrentItem()?.Id == callId)
                    return CurrentItem();
            }
            return null;
        }
    }
}
