using CallCenterSystem.Interfaces;
using CallCenterSystem.Models;

namespace CallCenterSystem.Services.State
{
    // Context
    // Wraps a single Call from the shared call log and manages its live status
    // (On Call / On Hold / Ended) by delegating to the current ICallState.
    // Deliberately does NOT touch CallLog/CallLogIterator directly — it only
    // reads and writes the Call object it was handed, since Call is already a
    // shared reference sitting inside the log.
    public class CallSession
    {
        public Call Call { get; }
        public ICallState CurrentState { get; private set; }

        // Tracked separately, per the brief: talk time accrues while on a call,
        // hold time accrues while on hold, neither accrues once hung up.
        public TimeSpan TalkTime { get; private set; } = TimeSpan.Zero;
        public TimeSpan HoldTime { get; private set; } = TimeSpan.Zero;

        public CallSession(Call call)
        {
            Call = call;
            CurrentState = new OnCallState();
            Call.Status = CurrentState.Name;
        }

        // Concrete states call this to move the session to a new state
        public void SetState(ICallState state)
        {
            CurrentState = state;
            Call.Status = state.Name;
        }

        public void Connect() => CurrentState.Connect(this);

        public void Hold() => CurrentState.Hold(this);

        public void HangUp() => CurrentState.HangUp(this);

        // Called once a second by the UI timer while the session is open
        public void Tick() => CurrentState.Tick(this);

        public void AddTalkSecond() => TalkTime = TalkTime.Add(TimeSpan.FromSeconds(1));

        public void AddHoldSecond() => HoldTime = HoldTime.Add(TimeSpan.FromSeconds(1));

        // Called once, by HungUpState, when the call ends — pushes the final
        // recorded duration onto the shared Call so the log/iterator reflect it
        public void SaveFinalDuration() => Call.Duration = TalkTime;
    }
}
