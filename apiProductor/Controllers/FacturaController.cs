using apiProductor.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Azure.Messaging.ServiceBus;

namespace apiProductor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        [HttpPost]
        public async Task<bool> EnviarAsync([FromBody] Factura f)
        {
            string connectionString = "Endpoint=sb://jessformrecognizersbq.servicebus.windows.net/;SharedAccessKeyName=EnviarEscuchar;SharedAccessKey=sVIj7qnnEzpLqIh/MV03yIK8xsm0+hAv3tT+dfUnJSw=;EntityPath=formrecognizersbq";
            string queueName = "formrecognizersbq";
            string mensaje = JsonConvert.SerializeObject(f);
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a sender for the queue 
                ServiceBusSender sender = client.CreateSender(queueName);

                // create a message that we can send
                ServiceBusMessage message = new ServiceBusMessage(mensaje);

                // send the message
                await sender.SendMessageAsync(message);
                Console.WriteLine($"Sent a single message to the queue: {queueName}");
            }
            return true;
        }
    }
}
