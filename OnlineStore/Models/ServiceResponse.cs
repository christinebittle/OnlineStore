namespace OnlineStore.Models
{
    public class ServiceResponse
    {
        //Not to be confused with HTTP response!
        //The type of response to give on direct data manipulations (Create, Delete, Update)

        public enum ServiceStatus { NotFound, Created, Updated, Deleted, Error }

        public ServiceStatus Status { get; set; }

        public int CreatedId { get; set; }

        // ServiceResponse package allows for more information, such as logic / validation errors
        public List<string> Messages { get; set; } = new List<string>();

    }
}
