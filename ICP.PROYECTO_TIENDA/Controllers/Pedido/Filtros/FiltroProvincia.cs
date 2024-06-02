using System.ComponentModel.DataAnnotations;

namespace Icp.TiendaApi.Controllers.Pedido.Filtros
{
    public class FiltroProvincia : ValidationAttribute
    {
        private static readonly HashSet<string> Provincias = new HashSet<string>
    {
        "Álava", "Albacete", "Alicante", "Almería", "Asturias", "Ávila", "Badajoz", "Baleares",
        "Barcelona", "Burgos", "Cáceres", "Cádiz", "Cantabria", "Castellón", "Ciudad Real",
        "Córdoba", "Cuenca", "Girona", "Granada", "Guadalajara", "Gipuzkoa", "Huelva", "Huesca",
        "Jaén", "La Rioja", "Las Palmas", "León", "Lleida", "Lugo", "Madrid", "Málaga", "Murcia",
        "Navarra", "Ourense", "Palencia", "Pontevedra", "Salamanca", "Segovia", "Sevilla", "Soria",
        "Tarragona", "Santa Cruz de Tenerife", "Teruel", "Toledo", "Valencia", "Valladolid",
        "Bizkaia", "Zamora", "Zaragoza", "Ceuta", "Melilla"
    };

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var provincia = value as string;
            if (provincia != null && Provincias.Contains(provincia))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Provincia no válida. Debe ser una provincia de España.");
        }
    }
}
