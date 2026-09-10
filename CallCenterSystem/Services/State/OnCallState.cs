using CallCenterSystem.Interfaces;

namespace CallCenterSystem.Services.State
{
    // ConcreteState — the caller is actively speaking; talk time is recorded
    public class OnCallState : ICallState
    {
        public string Name => "On Call";

        public void Connect(CallSession session)
        {
            // Already on the call — nothing to do
        }

        public void Hold(CallSession session) => session.SetState(new OnHoldState());

        public void HangUp(CallSession session)
        {
            session.SetState(new HungUpState());
            session.SaveFinalDuration();
        }

        public void Tick(CallSession session) => session.AddTalkSecond();
    }
}
