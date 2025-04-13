namespace Souq.Api.Helper.Errors
{
    public class ApiValiditionError:ApiResponse
    {
        public ApiValiditionError()
            :base(400)
        {
            Errors=new List<string>();
        }
        public IEnumerable<string> Errors { get; set; }
    }
}
