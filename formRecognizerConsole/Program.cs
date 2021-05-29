using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;
using formRecognizerConsole.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace formRecognizerConsole
{
    class Program
    {
        private static readonly string endpoint = "https://jessformrecognizer.cognitiveservices.azure.com/";
        private static readonly string apiKey = "0bd31f6a8ad74efa95376576886179e4";
        private static readonly AzureKeyCredential credential = new AzureKeyCredential(apiKey);
        static void Main(string[] args)
        {
            string modelId = "ed259ecf-61b1-4a81-ad51-be352615ad3c";
            Console.WriteLine("¡Bienvenido! A continuación escribe la ruta completa donde se encuentra tu imagen");
            Console.WriteLine("Ejemplo: D:/Ejemplos/ejemplo2.jpg");
            string filePath = Console.ReadLine();
            IDictionary<string, string> factura = new Dictionary<string, string>();
            var analyzeFormTask = AnalyzeForm(modelId, filePath, factura);
            Task.WaitAll(analyzeFormTask);

        }

        // Analyze PDF form data
        private static async Task AnalyzeForm(String modelId, string filePath, IDictionary<string, string> factura)
        {
            FormRecognizerClient client = new FormRecognizerClient(new Uri(endpoint), credential);

            var stream = new FileStream(filePath, FileMode.Open);

            var options = new RecognizeCustomFormsOptions() { IncludeFieldElements = true };

            RecognizeCustomFormsOperation operation = await client.StartRecognizeCustomFormsAsync(modelId, stream, options);
            Response<RecognizedFormCollection> operationResponse = await operation.WaitForCompletionAsync();
            RecognizedFormCollection forms = operationResponse.Value;

            foreach (RecognizedForm form in forms)
            {
                Console.WriteLine($"Form of type: {form.FormType}");
                Console.WriteLine($"Form was analyzed with model with ID: {modelId}");
                foreach (FormField field in form.Fields.Values)
                {
                    factura.Add(field.Name, field.ValueData.Text);
                }
            }

            foreach (KeyValuePair<string, string> dato in factura)
            {
                Console.WriteLine("{0}: {1}",
                dato.Key, dato.Value);
            }
            Console.WriteLine("¿Deseas guardar los datos? Si/No");
            string guardar = Console.ReadLine();
            if (guardar == "Si")
            {
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
                else
                {
                    Console.WriteLine("Escribe el ID de la factura");
                    f.IDFactura = Convert.ToInt32(Console.ReadLine());
                }

                var httpWebRequest = (HttpWebRequest)WebRequest.Create("https://apiproductorai.azurewebsites.net/api/factura");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = JsonConvert.SerializeObject(f);
                    streamWriter.Write(json);
                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var result = streamReader.ReadToEnd();
                }
            }
        }
    }
}
