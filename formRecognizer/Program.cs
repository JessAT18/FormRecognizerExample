using Azure;
using Azure.AI.FormRecognizer;  
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace formRecognizer
{
    class Program
    {
        private static readonly string endpoint = "https://jessformrecognizer.cognitiveservices.azure.com/";
        private static readonly string apiKey = "0bd31f6a8ad74efa95376576886179e4";
        private static readonly AzureKeyCredential credential = new AzureKeyCredential(apiKey);
        static void Main(string[] args)
        {
            string modelId = "ed259ecf-61b1-4a81-ad51-be352615ad3c";
            string filePath = "D:/Ejemplos/ejemplo2.jpg";

            var analyzerPDFFormData = AnalyzeForm(modelId, filePath);
            Task.WaitAll(analyzerPDFFormData);
        }
        static private FormRecognizerClient AuthenticateClient()
        {
            var credential = new AzureKeyCredential(apiKey);
            var client = new FormRecognizerClient(new Uri(endpoint), credential);
            return client;
        }
        static private FormTrainingClient AuthenticateTrainingClient()
        {
            var credential = new AzureKeyCredential(apiKey);
            var client = new FormTrainingClient(new Uri(endpoint), credential);
            return client;
        }
        
        // Analyze PDF form data
        private static async Task AnalyzeForm(String modelId, string filePath)
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
                    Console.WriteLine($"Field '{field.Name}: ");

                    if (field.LabelData != null)
                    {
                        Console.WriteLine($"    Label: '{field.LabelData.Text}");
                    }

                    Console.WriteLine($"    Value: '{field.ValueData.Text}");
                    Console.WriteLine($"    Confidence: '{field.Confidence}");
                }
                Console.WriteLine("Table data:");
                foreach (FormPage page in form.Pages)
                {
                    for (int i = 0; i < page.Tables.Count; i++)
                    {
                        FormTable table = page.Tables[i];
                        Console.WriteLine($"Table {i} has {table.RowCount} rows and {table.ColumnCount} columns.");
                        foreach (FormTableCell cell in table.Cells)
                        {
                            Console.WriteLine($"    Cell ({cell.RowIndex}, {cell.ColumnIndex}) contains {(cell.IsHeader ? "header" : "text")}: '{cell.Text}'");
                        }
                    }
                }
            }
        }

    }
}
