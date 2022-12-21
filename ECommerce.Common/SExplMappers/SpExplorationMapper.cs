using AutoMapper;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using System.Globalization;

namespace ECommerce.Common.SExplMappers
{
    public class SpExplorationMapper : Profile
    {
        public SpExplorationMapper()
        {
            CreateMap<Concepto, ConceptoDto>().ReverseMap();
            CreateMap<Bodega, BodegaDto>().ReverseMap();
            CreateMap<Departamento, DepartamentoDto>().ReverseMap();
            CreateMap<Iva, IvaDto>().ReverseMap();
            CreateMap<Medidum, MedidumDto>().ReverseMap();
            CreateMap<Producto, ProductoDto>().ReverseMap();
            #region Producto
            CreateMap<Producto, VMProducto>()
               .ForMember(destiny => destiny.EsActivo,
               opt => opt.MapFrom(origin => origin.IsActive == 1 ? 1 : 0))
               .ForMember(destiny => destiny.NombreMedida,
               opt => opt.MapFrom(origin => $"{origin.MedidaNavigation.Descripcion}{" - "}{origin.MedidaNavigation.Escala}"))
               .ForMember(destiny => destiny.NombreIva,
               opt => opt.MapFrom(origin => $"{origin.Iva.Descripcion}{" - "}{origin.Iva.Tarifa}"))
               .ForMember(destiny => destiny.NombreDepartamento,
               opt => opt.MapFrom(origin => $"{origin.Departamento.Descripcion}"))
               .ForMember(destiny => destiny.Precio,
               opt => opt.MapFrom(origin => String.Format("{0:C2}", Convert.ToString(origin.Precio.Value, new CultureInfo("es-Mex")))))
               .ForMember(destiny => destiny.Medida,
               opt => opt.MapFrom(origin => Convert.ToString(origin.Medida, new CultureInfo("es-Mex"))))
               .ForMember(destiny => destiny.Pieza,
               opt => opt.MapFrom(origin => Convert.ToString(origin.Pieza.Value, new CultureInfo("es-Mex"))));

            CreateMap<VMProducto, Producto>()
              .ForMember(destiny => destiny.IsActive,
              opt => opt.MapFrom(origin => origin.EsActivo == 1 ? true : false))
              .ForMember(destiny =>
              destiny.DepartamentoId,
              opt => opt.Ignore())
              .ForMember(destiny =>
              destiny.MedidaId,
              opt => opt.Ignore())
              .ForMember(destiny =>
              destiny.Ivaid,
              opt => opt.Ignore())
              .ForMember(destiny => destiny.Precio,
              opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Precio, new CultureInfo("es-Mex"))))
              .ForMember(destiny => destiny.Medida,
              opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Medida, new CultureInfo("es-Mex"))))
              .ForMember(destiny => destiny.Pieza,
              opt => opt.MapFrom(origin => Convert.ToDecimal(origin.Pieza, new CultureInfo("es-Mex"))));
            #endregion Producto
        }
    }
}
