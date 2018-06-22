#load "../Share/Order.csx"
#load "../Share/License.csx"
#r "Newtonsoft.Json"

using System;
using Newtonsoft.Json;

public static void Run(Order myQueueItem, TraceWriter log, IBinder binder)
{
    log.Info($"Order received: Order {myQueueItem.OrderId}, Product {myQueueItem.ProductId}, Email {myQueueItem.Email}");

    using(var outputBlob = binder.Bind<TextWriter>(new BlobAttribute($"licenses/{myQueueItem.OrderId}.lic")))
    {
        var license = new License()
        {
            OrderId = myQueueItem.OrderId,
            Email = myQueueItem.Email,
            ProductId = myQueueItem.ProductId      
        };
    
        var md5 = System.Security.Cryptography.MD5.Create();
        var hash = md5.ComputeHash(System.Text.Encoding.UTF8.GetBytes(myQueueItem.Email + "secret"));
        
        license.SecretCode = BitConverter.ToString(hash);
        outputBlob.WriteLine(JsonConvert.SerializeObject(license));
    }    
}