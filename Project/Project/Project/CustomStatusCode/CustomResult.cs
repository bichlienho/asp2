namespace Project.CustomStatusCode
{
    public class CustomResult
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public object? Data { get; set; }
    }
}
