using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class Transaccion 
    {
        public int Id { get; set; }

        public int UsuarioId { get; set; }

        [Display(Name = "Fecha Transaccion")]
        [DataType(DataType.Date)]

        public DateTime FechaTransaccion { get; set; } = DateTime.Today;
            /*Parse(DateTime.Now.ToString("yyyy-MM-dd hh:MM tt"))*/

        public decimal Monto { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Seleccione una categoria para continuar")]
        [Display(Name = "Categoría")]

        public int CategoriaId { get; set; }

        [StringLength(maximumLength: 1000, ErrorMessage = "La nota no puede superar {1} caracteres")]

        public string Nota { get; set; }

        [Range(1, maximum: int.MaxValue, ErrorMessage = "Tiene que seleccionar una Cuenta pues")]
        [Display(Name = "Cuenta")]
          
        public int CuentaId { get; set; }

        [Display(Name = "Tipo Operacion")]
        public TipoOperacion TipoOperacionId { get; set; } = TipoOperacion.Ingreso;

        public string Cuenta { get; set; }

        public string Categoria { get; set; }
    }
}
