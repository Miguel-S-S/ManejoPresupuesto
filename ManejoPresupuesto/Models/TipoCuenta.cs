using ManejoPresupuesto.Validaciones;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Models
{
    public class TipoCuenta
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Escribi tu {0} antes de enviar el formulario")]


        //a continuacion validaciones por atributo
        //[StringLength(maximumLength:50, MinimumLength = 3, ErrorMessage = "Tu {0} necesita al menos {2} y como maximo {1} ¡GATITO!")]
        //[Display(Name = "Nombre del tipo cuenta: ")]
        [Remote(action: "VerificarExisteTipoCuenta", controller: "TiposCuentas")]

        [PrimeraLetraMayuscula]
        public String Nombre { get; set; }

        public int UsuarioId { get; set; }

        public int Orden { get; set; }

        //pruebas de otras validaciones por defecto
        //[Required(ErrorMessage = "El campo {0} es importante que lo complete")]
        //[EmailAddress(ErrorMessage = "El {0} no tiene formato de correo electronico por favor verifiquelo ")]
        //public String Email { get; set; }

        //public int Edad { get; set; }

        //[Url(ErrorMessage = "No es una URL ingreso invalido")]
        //public String URL { get; set; }

        //[CreditCard(ErrorMessage = "Tu tarjeta no tiene saldo ")]
        //[Display(Name ="Tarjeta de Credito")]
        //public String TarjetaDeCredito { get; set; }
    }
}
