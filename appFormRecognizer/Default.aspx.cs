using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Azure;
using Azure.AI.FormRecognizer;
using Azure.AI.FormRecognizer.Models;
using Azure.AI.FormRecognizer.Training;

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace appFormRecognizer
{
    public partial class _Default : Page
    {
        private static readonly string endpoint = "https://jessformrecognizer.cognitiveservices.azure.com/";
        private static readonly string apiKey = "0bd31f6a8ad74efa95376576886179e4";
        private static readonly AzureKeyCredential credential = new AzureKeyCredential(apiKey);
        protected void Page_Load(object sender, EventArgs e)
        {
            txtIDFactura.Text = "";
            txtCI.Text = "";
            txtFecha.Text = "";
            txtHora.Text = "";
            txtNombre.Text = "";
            txtNombreEESS.Text = "";
            txtNumFactura.Text = "";
            txtPlaca.Text = "";
            txtTotal.Text = "";
        }

        protected void btnCargar_Click(object sender, EventArgs e)
        {
            string modelId = "f2be2fe5-6f54-40d1-a259-67dd5320eaf9";
            string filePath = txtIDPath.Text; //Server.MapPath(fuFoto.FileName);
            IDictionary<string, string> factura = new Dictionary<string, string>();
            var analyzerPDFFormData = AnalyzeForm(modelId, filePath, factura);
            Task.WaitAll(analyzerPDFFormData);
            if (factura.ContainsKey("Nombre Estacion de Servicio"))
            {
                txtNombreEESS.Text = factura["Nombre Estacion de Servicio"];
            }
            if (factura.ContainsKey("NIT/CI"))
            {
                txtCI.Text = factura["NIT/CI"];
            }
            if (factura.ContainsKey("Nombre"))
            {
                txtNombre.Text = factura["Nombre"];
            }
            if (factura.ContainsKey("Total Bs"))
            {
                txtTotal.Text = factura["Total Bs"];
            }
            if (factura.ContainsKey("Fecha"))
            {
                txtFecha.Text = factura["Fecha"];
            }
            if (factura.ContainsKey("Hora"))
            {
                txtHora.Text = factura["Hora"];
            }
            if (factura.ContainsKey("Placa Vehiculo"))
            {
                txtPlaca.Text = factura["Placa Vehiculo"];
            }
            if (factura.ContainsKey("Numero de factura"))
            {
                txtNumFactura.Text = factura["Numero de factura"];
            }
        }

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
                foreach (FormField field in form.Fields.Values)
                {
                    factura.Add(field.Name, field.ValueData.Text);
                }
            }
        }

        protected void btnGuardar_Click(object sender, EventArgs e)
        {

        }
    }
}