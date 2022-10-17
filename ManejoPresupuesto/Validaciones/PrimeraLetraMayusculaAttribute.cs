using System.ComponentModel.DataAnnotations;

namespace ManejoPresupuesto.Validaciones
{
    public class PrimeraLetraMayusculaAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //verifica si es valido primero 
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            var primeraLetra = value.ToString()[0].ToString();

            //si la letra es distinta a su mayuscula entonces esta en minusculas
            if (primeraLetra != primeraLetra.ToUpper())
            {
                return new ValidationResult("La primera letra tiene que ser mayúscula");
            }
            return ValidationResult.Success;
        }
    }
}
