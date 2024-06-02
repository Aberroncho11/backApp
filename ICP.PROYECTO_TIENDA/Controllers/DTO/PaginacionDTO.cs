namespace Icp.TiendaApi.Controllers.DTO
{
    public class PaginacionDTO<T>
    {
        public List<T> Items { get; set; }
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }

}
