using System.Net;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info("C# HTTP trigger function processed a request.");
    
    Sleep(log);

    return req.CreateResponse(HttpStatusCode.OK, "OK");
}

public static async Task Sleep(TraceWriter log)
{
    System.Threading.Thread.Sleep(20000);
    log.Info("Done Sleeping");
}
