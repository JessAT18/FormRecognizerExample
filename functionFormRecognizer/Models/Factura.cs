using System.ComponentModel.DataAnnotations;

namespace functionFormRecognizer.Models
{
    public class Factura
    {
        [Key]
        public int IDFactura { get; set; }
        [Required]
        public string NombreEESS { get; set; }
        [Required]
        public string Fecha { get; set; }
        [Required]
        public string Hora { get; set; }
        [Required]
        public string Ci { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        public string Placa { get; set; }
        [Required]
        public double TotalBs { get; set; }
    }
}
