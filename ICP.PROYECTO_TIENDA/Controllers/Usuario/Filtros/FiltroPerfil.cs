namespace Icp.TiendaApi.Controllers.Usuario.Filtros
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Icp.TiendaApi.Controllers.DTO.Usuario;

    public class FiltroPerfil : ValidationAttribute
    {
        public enum UserRole
        {
            Administrador = 1,
            Gestor = 2,
            Operador = 3
        }
 
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int intValue && Enum.IsDefined(typeof(UserRole), intValue))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Perfil debe ser Administrador, Gestor o Operador.");
        }
    }
}
