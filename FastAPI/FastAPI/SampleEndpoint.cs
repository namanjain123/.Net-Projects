using FastEndpoints;

namespace FastAPI
{
    public class SampleEndpoint :EndpointWithoutRequest
    {
        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("test");
            //to allow everyone
            AllowAnonymous();
        }
        
    }
}
