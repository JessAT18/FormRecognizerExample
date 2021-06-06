using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using functionFormRecognizer.Models;

//Libraries for form recognizer
using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using System.Collections.Generic;

namespace functionFormRecognizer
{
    public static class Function1
    {
        private static readonly string endpoint = "https://jessformrecognizer.cognitiveservices.azure.com/";
        private static readonly string apiKey = "0bd31f6a8ad74efa95376576886179e4";
        private static readonly AzureKeyCredential credential = new AzureKeyCredential(apiKey);
        private static readonly string modelId = "ed259ecf-61b1-4a81-ad51-be352615ad3c";

        private static async Task<string> AnalyzeForm(String modelId, string filePath)
        {
            Dictionary<string, string> factura = new Dictionary<string, string>();
            FormRecognizerClient client = new FormRecognizerClient(new Uri(endpoint), credential);

            var stream = new FileStream(filePath, FileMode.Open);

            var options = new RecognizeCustomFormsOptions() { IncludeFieldElements = true };

            RecognizeCustomFormsOperation operation = await client.StartRecognizeCustomFormsAsync(modelId, stream, options);
            Response<RecognizedFormCollection> operationResponse = await operation.WaitForCompletionAsync();
            RecognizedFormCollection forms = operationResponse.Value;
            foreach (RecognizedForm form in forms)
            {
                //Console.WriteLine($"Form of type: {form.FormType}");
                //Console.WriteLine($"Form was analyzed with model with ID: {modelId}");
                foreach (FormField field in form.Fields.Values)
                {
                    factura.Add(field.Name, field.ValueData.Text);
                }
            }
            
            Factura f = new Factura();
            if (factura.ContainsKey("Nombre Estacion de Servicio"))
            {
                f.NombreEESS = factura["Nombre Estacion de Servicio"];
            }
            if (factura.ContainsKey("NIT/CI"))
            {
                f.Ci = factura["NIT/CI"];
            }
            if (factura.ContainsKey("Nombre"))
            {
                f.Nombre = factura["Nombre"];
            }
            if (factura.ContainsKey("Total Bs"))
            {
                f.TotalBs = System.Convert.ToDouble(factura["Total Bs"].Replace(",", "."));
            }
            if (factura.ContainsKey("Fecha"))
            {
                f.Fecha = factura["Fecha"];
            }
            if (factura.ContainsKey("Hora"))
            {
                f.Hora = factura["Hora"];
            }
            if (factura.ContainsKey("Placa Vehiculo"))
            {
                f.Placa = factura["Placa Vehiculo"];
            }
            if (factura.ContainsKey("Numero de Factura"))
            {
                f.IDFactura = System.Convert.ToInt32(factura["Numero de Factura"]);
            }
            //return "OK";
            return JsonConvert.SerializeObject(f);
        }


        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];
            string urlImagen = req.Query["imagen"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            urlImagen = urlImagen ?? data?.imagen;

            var informacion = AnalyzeForm(modelId, urlImagen);

            string responseMessage = string.IsNullOrEmpty(urlImagen)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : informacion.Result;

            return new OkObjectResult(responseMessage);
        }
    }
}
