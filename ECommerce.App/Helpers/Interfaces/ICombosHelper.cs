using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.App.Helpers.Interfaces
{
    public interface ICombosHelper
    {
        IEnumerable<SelectListItem> GetComboDepartamentos();
        IEnumerable<SelectListItem> GetComboMedidums();
        IEnumerable<SelectListItem> GetComboIvas();
        IEnumerable<SelectListItem> GetComboProveedors();
        IEnumerable<SelectListItem> GetComboTipoDocuemtnos();
    }
}
