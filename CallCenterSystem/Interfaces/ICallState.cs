using CallCenterSystem.Services.State;

namespace CallCenterSystem.Interfaces
{
    // State
    // Each concrete state decides what happens for a given action, and whether/how
    // the CallSession transitions to a different state. This keeps CallSession itself
    // free of if/else chains about "what state am I in".
    public interface ICallState
    {
        // Display name shown in the UI and stored back onto Call.Status
        string Name { get; }

        // Pick up / resume the call
        void Connect(CallSession session);

        // Put the call on hold
        void Hold(CallSession session);

        // End the call
        void HangUp(CallSession session);

        // Called once per second while a session is open, so each state can decide
        // whether talk time or hold time accrues (or neither, once ended)
        void Tick(CallSession session);
    }
}
