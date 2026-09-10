using CallCenterSystem.Interfaces;

namespace CallCenterSystem.Services.State
{
    // ConcreteState — terminal state. The caller cannot speak and the final
    // duration has already been written back onto the shared Call/log entry.
    public class HungUpState : ICallState
    {
        public string Name => "Ended";

        public void Connect(CallSession session)
        {
            // A call that has ended cannot be reconnected from here — the
            // manager would place a NEW call from the log (Iterator's job),
            // not resume this session.
        }

        public void Hold(CallSession session)
        {
            // Nothing to hold — the call is already over
        }

        public void HangUp(CallSession session)
        {
            // Already ended — nothing to do
        }

        public void Tick(CallSession session)
        {
            // Not speaking, so neither talk time nor hold time accrues
        }
    }
}
