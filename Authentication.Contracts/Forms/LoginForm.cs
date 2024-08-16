namespace Authentication.Contracts.Forms
{
    public record LoginForm
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
