namespace OnlineStore.Models.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public List<string>? Errors { get; set; }
    }
}
