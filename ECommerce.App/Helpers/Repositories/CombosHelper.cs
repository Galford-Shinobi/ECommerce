using ECommerce.App.Helpers.Interfaces;
using ECommerce.Common.DataBase;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ECommerce.App.Helpers.Repositories
{
    public class CombosHelper : ICombosHelper
    {
        private readonly ECommerceDbContext _dataContext;

        public CombosHelper(ECommerceDbContext dbContext)
        {
            _dataContext = dbContext;
        }

        public IEnumerable<SelectListItem> GetComboDepartamentos()
        {
            List<SelectListItem> list = _dataContext.Departamentos
               .Where(t => t.IsActive == 1)
               .OrderBy(pt => pt.DepartamentoId)
                .Select(pt => new SelectListItem
                {
                    Text = $"{pt.Descripcion.ToString()}",
                    Value = pt.DepartamentoId.ToString(),
                })
                 .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Departamento...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboGeneros()
        {
            List<SelectListItem> list = _dataContext.Generos
             .Where(t => t.IsActive == 1)
             .OrderBy(pt => pt.GenderId)
              .Select(pt => new SelectListItem
              {
                  Text = $"{pt.GeneroName.ToString()}",
                  Value = pt.GenderId.ToString(),
              })
               .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona tipo de Genero...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboIvas()
        {
            List<SelectListItem> list = _dataContext.Ivas
              .Where(t => t.IsActive == 1)
              .OrderBy(pt => pt.Ivaid)
               .Select(pt => new SelectListItem
               {
                   Text = $"{pt.Descripcion.ToString()} {pt.Tarifa.ToString()}",
                   Value = pt.Ivaid.ToString(),
               })
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un impuesto...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboMedidums()
        {
            List<SelectListItem> list = _dataContext.Medida
              .Where(t => t.IsActive == 1)
              .OrderBy(pt => pt.MedidaId)
               .Select(pt => new SelectListItem
               {
                   Text = $"{pt.Descripcion.ToString()} {pt.Escala.ToString()}",
                   Value = pt.MedidaId.ToString(),
               })
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Medida...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboProveedors()
        {
            List<SelectListItem> list = _dataContext.Proveedors
              .Where(t => t.IsActive == 1)
              .OrderBy(pt => pt.IDProveedor)
               .Select(pt => new SelectListItem
               {
                   Text = $"{pt.Nombre.ToString()} {pt.NombresContacto.ToString()}",
                   Value = pt.IDProveedor.ToString(),
               })
                .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Proveedor...)",
                Value = "0"
            });

            return list;
        }

        public IEnumerable<SelectListItem> GetComboTipoDocuemtnos()
        {
            List<SelectListItem> list = _dataContext.TipoDocumentos
             .Where(t => t.IsActive == 1)
             .OrderBy(pt => pt.TipoDocumentoId)
              .Select(pt => new SelectListItem
              {
                  Text = $"{pt.Descripcion.ToString()}",
                  Value = pt.TipoDocumentoId.ToString(),
              })
               .ToList();

            list.Insert(0, new SelectListItem
            {
                Text = "(Selecciona un Tipo de Documento...)",
                Value = "0"
            });

            return list;
        }
    }
}
