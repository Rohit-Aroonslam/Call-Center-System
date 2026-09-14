namespace CallCenterSystem.Models
{
    // Item being iterated over
    public class Call
    {
        public int Id { get; }
        public string CallerName { get; }      // student or technician
        public string PhoneNumber { get; }
        public DateTime LoggedAt { get; }       // when this call entry was created
        public TimeSpan Duration { get; set; }
        public TimeSpan TalkTime { get; set; }  // set alongside Duration once the call ends
        public TimeSpan HoldTime { get; set; }  // time spent on hold during the call, if any
        public string Status { get; set; }     // set/read by the State pattern classes

        public Call(int id, string callerName, string phoneNumber)
        {
            Id = id;
            CallerName = callerName;
            PhoneNumber = phoneNumber;
            LoggedAt = DateTime.Now;
            Duration = TimeSpan.Zero;
            TalkTime = TimeSpan.Zero;
            HoldTime = TimeSpan.Zero;
            Status = "Ended";
        }

        public override string ToString() =>
            $"[{Id}] {CallerName} ({PhoneNumber}) - {Status} - {Duration:mm\\:ss}";
    }
}