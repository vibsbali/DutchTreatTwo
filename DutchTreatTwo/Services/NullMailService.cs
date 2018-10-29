using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DutchTreatTwo.Services
{
    public class NullMailService : IMailService
    {
       private readonly ILogger<NullMailService> _logger;

       public NullMailService(ILogger<NullMailService> logger)
       {
          _logger = logger;
       }
       public void SendMail(string email, string subject, string body)
       {
          _logger.LogInformation($"To: {email}, Subject: {subject}, Body: {body}");
       }
    }

   public interface IMailService
   {
      void SendMail(string email, string subject, string body);
   }
}
