#load "../Share/Order.csx"
#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json;


public static async Task<object> Run(HttpRequestMessage req, TraceWriter log,
    IAsyncCollector<Order> outputQueueItem, IAsyncCollector<Order> outputTable)
{
    log.Info($"Order received!");

    string jsonContent = await req.Content.ReadAsStringAsync();
    var order = JsonConvert.DeserializeObject<Order>(jsonContent);

    log.Info($"Order {order.OrderId} received from {order.Email} for product {order.ProductId}");

    order.PartitionKey = "Orders";
    order.RowKey = order.OrderId;
    order.Paid = true;

    await outputTable.AddAsync(order);
    await outputQueueItem.AddAsync(order);

    return req.CreateResponse(HttpStatusCode.OK, new{
        message = "Thank you for your order! It's a pleasure to serve you."
    });
}
