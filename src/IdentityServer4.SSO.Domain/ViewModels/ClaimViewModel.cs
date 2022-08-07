namespace ByLearning.Domain.Core.ViewModels
{
    public class ClaimViewModel
    {
        public ClaimViewModel() { }
        public ClaimViewModel(string type, string value)
        {
            Type = type;
            Value = value;
        }

        public string Type { get; set; }
        public string Value { get; set; }
    }
}
