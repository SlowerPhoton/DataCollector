namespace DTOs
{
    public class SpecifiedElementDTO
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public bool Required { get; set; }
        public static string[] properties = { "state", "need of investment", "amount of investment", "notes" };
    }
}