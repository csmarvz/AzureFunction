#r "Newtonsoft.Json"

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Mail;

public static void Run(JObject eventGridEvent, TraceWriter log)
{
    log.Info(eventGridEvent.ToString(Formatting.Indented));

    
    // do some data validation... skipped this for demo purpose only.
    // if validation failes -> HttpStatusCode.BadRequest should be returned as HTTP Status
    dynamic data = ((dynamic)eventGridEvent).data;

    bool isImportantEmail = bool.Parse(data.isImportant.ToString());
    string fromEmail = data.fromEmail;
    string toEmail = data.toEmail;
    int smtpPort = 25;
    bool smtpEnableSsl = true;
    string smtpHost = "smtp.mailtrap.io"; // your smtp host
    string smtpUser = "20d56f423be2ee"; // your smtp user
    string smtpPass = "322ab4778ccb33"; // your smtp password
    string subject = data.subject;
    string message = data.message;
    
    MailMessage mail = new MailMessage(fromEmail, toEmail);
    SmtpClient client = new SmtpClient();
    client.Port = smtpPort;
    client.EnableSsl = smtpEnableSsl;
    client.DeliveryMethod = SmtpDeliveryMethod.Network;
    client.UseDefaultCredentials = false;
    client.Host = smtpHost;
    client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
    mail.Subject = message;
    
    if (isImportantEmail) {
      mail.Priority = MailPriority.High;
    }
    
    mail.Body = message;
    try {
      client.Send(mail);
      log.Info("Email sent.");
    }catch (Exception ex) {
      log.Info(ex.ToString());
    }
}
