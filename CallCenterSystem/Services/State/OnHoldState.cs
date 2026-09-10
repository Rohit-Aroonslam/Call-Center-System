using CallCenterSystem.Interfaces;

namespace CallCenterSystem.Services.State
{
    // ConcreteState: the caller cannot speak, hold time is recorded instead of talk time
    public class OnHoldState : ICallState
    {
        public string Name => "On Hold";

        public void Connect(CallSession session) => session.SetState(new OnCallState()); // resume the call

        public void Hold(CallSession session)
        {
            // Already on hold, nothing to do
        }

        public void HangUp(CallSession session)
        {
            session.SetState(new HungUpState());
            session.SaveFinalDuration();
        }

        public void Tick(CallSession session) => session.AddHoldSecond();
    }
}