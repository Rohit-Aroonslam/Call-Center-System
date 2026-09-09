namespace CallCenterSystem.Models
{
    // Item being iterated over
    public class Call
    {
        public int Id { get; }
        public string CallerName { get; }      // student or technician
        public string PhoneNumber { get; }
        public TimeSpan Duration { get; set; }
        public string Status { get; set; }     // set/read by the State pattern classes

        public Call(int id, string callerName, string phoneNumber)
        {
            Id = id;
            CallerName = callerName;
            PhoneNumber = phoneNumber;
            Duration = TimeSpan.Zero;
            Status = "Ended";
        }

        public override string ToString() =>
            $"[{Id}] {CallerName} ({PhoneNumber}) - {Status}";
    }
}
