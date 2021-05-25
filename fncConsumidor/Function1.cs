using System;
using System.Threading.Tasks;
using fncConsumidor.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace fncConsumidor
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task RunAsync(

            [ServiceBusTrigger(
                    "formrecognizersbq",
                    Connection = "MyConn"
            )] string myQueueItem,

            [CosmosDB(
                    databaseName:"dbfacturas",
                    collectionName:"Facturas",
                    ConnectionStringSetting = "strCosmos"
             )] IAsyncCollector<object> datos,

            ILogger log)
        {
            try
            {
                log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
                var data = JsonConvert.DeserializeObject<Factura>(myQueueItem);
                await datos.AddAsync(data);
            }
            catch (Exception ex)
            {
                log.LogError($"No es posible insertar datos: {ex.Message}");
            }
        }
    }
}
