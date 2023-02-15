namespace AsyncTypeAPI.Model
{
    public class Request
    {
        public int Id { get; set; }
        public string? RequestBody { get; set; }
        public string? EstimatedTime { get; set; }
        public string? RequestId { get; set; }=Guid.NewGuid().ToString();



    }
}
