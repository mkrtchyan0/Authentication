namespace Authentication.Contracts.Responses
{
    public class AppResposne
    {
        public int StatusCode { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
