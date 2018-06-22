#load "../Share/License.csx"
#load "../Share/Order.csx"
#r "Newtonsoft.Json"
#r "SendGrid"

using SendGrid.Helpers.Mail;
using System;
using Newtonsoft.Json;


public static void Run(string myBlob, string filename, Order ordersRow, TraceWriter log, out Mail message)
{    
    //var license = JsonConvert.DeserializeObject<License>(myBlob);

    log.Info($"Got order from {ordersRow.Email}\n License filename: {filename}");

    message = new Mail();
    
    var personalization = new Personalization();
    personalization.AddTo(new Email(ordersRow.Email));
    message.AddPersonalization(personalization);

    var attachment = new Attachment();
    var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(myBlob);
    attachment.Content = System.Convert.ToBase64String(plainTextBytes);
    attachment.Type = "text/plain";
    attachment.Filename = "license.lic";
    attachment.Disposition = "attachment";
    attachment.ContentId = "License File";
    message.AddAttachment(attachment);

    var messageContent = new Content("text/html", "Your license file is attached");
    message.AddContent(messageContent);
    message.Subject = "Thanks for your order";
    message.From = new Email("henry.seng@starticket.ch");


}
