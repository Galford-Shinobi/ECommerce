﻿using AutoMapper;
using ECommerce.Common.Application.Implementacion;
using ECommerce.Common.Entities;
using ECommerce.Common.Models.Dtos;
using System.Drawing.Imaging;
using System.Drawing;
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
            CreateMap<AspNetRole, RolDto>().ReverseMap();
            #region Proveedor
            CreateMap<Proveedor, ProveedorDto>()
                .ForMember(destiny => destiny.IsActive,
                opt => opt.MapFrom(origin => origin.IsActive==1?1: 0))
                .ForMember(destiny => destiny.DocumentoTipo,
                opt => opt.MapFrom(origin => origin.TipoDocumento.Descripcion));
            CreateMap<ProveedorDto, Proveedor>()
               .ForMember(destino =>
                   destino.TipoDocumento,
                   opt => opt.Ignore()
               );
            #endregion
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

            #region Barra

            CreateMap<Barra, VMBarraProducto>()
                .ForMember(destino =>
                    destino.IsActive,
                    opt => opt.MapFrom(origen => origen.IdproductoNavigation.IsActive == 1 ? 1 : 0)
                )
                .ForMember(destino =>
                    destino.NombreMedida,
                    opt => opt.MapFrom(origen => $"{origen.IdproductoNavigation.MedidaNavigation.Descripcion}{" - "}{origen.IdproductoNavigation.MedidaNavigation.Escala}")
                )
                .ForMember(destino =>
                    destino.NombreIva,
                    opt => opt.MapFrom(origen => $"{origen.IdproductoNavigation.Iva.Descripcion}{" - "}{origen.IdproductoNavigation.Iva.Tarifa}")
                )
                .ForMember(destino =>
                    destino.NombreDepartamento,
                    opt => opt.MapFrom(origen => $"{origen.IdproductoNavigation.Departamento.Descripcion}")
                )
                .ForMember(destino =>
                    destino.Nombre,
                    opt => opt.MapFrom(origen => $"{origen.IdproductoNavigation.Nombre}")
                )
                .ForMember(destino =>
                    destino.Barcode,
                    opt => opt.MapFrom(origen => $"{origen.Barcode}")
                )
                .ForMember(destino =>
                    destino.Descripcion,
                    opt => opt.MapFrom(origen => $"{origen.IdproductoNavigation.Descripcion}")
                )
                .ForMember(destino =>
                    destino.PathImagen,
                    opt => opt.MapFrom(origen => $"{origen.IdproductoNavigation.PathImagen}")
                )
                .ForMember(destino =>
                    destino.Imagen,
                    opt => opt.MapFrom(origen => origen.IdproductoNavigation.Imagen)
                )
                .ForMember(destino =>
                    destino.Precio,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdproductoNavigation.Precio.Value, new CultureInfo("es-Mex")))
                )
                .ForMember(destino =>
                    destino.Medida,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdproductoNavigation.Medida, new CultureInfo("es-Mex")))
                )
                .ForMember(destino =>
                    destino.Pieza,
                    opt => opt.MapFrom(origen => Convert.ToString(origen.IdproductoNavigation.Pieza.Value, new CultureInfo("es-Mex")))
                )
                 .ForMember(destiny => destiny.BarCodeImage,
               opt => opt.MapFrom(origin => BarCodeIndex(origin.Barcode)));


            CreateMap<VMBarraProducto, Barra>()
                .ForMember(destino =>
                    destino.IdproductoNavigation,
                    opt => opt.Ignore()
                );

            #endregion



        }

        #region MyBarCode
        private string BarCodeIndex(string barcode)
        {
            string ImageBarCode;
            using (MemoryStream ms = new MemoryStream())
            {
                //The Image is drawn based on length of Barcode text.
                using (Bitmap bitMap = new Bitmap(barcode.Length * 40, 80))
                {
                    //The Graphics library object is generated for the Image.
                    using (Graphics graphics = Graphics.FromImage(bitMap))
                    {
                        //The installed Barcode font.
                        Font oFont = new Font("IDAutomationHC39M Free Version", 16);
                        PointF point = new PointF(2f, 2f);

                        //White Brush is used to fill the Image with white color.
                        SolidBrush whiteBrush = new SolidBrush(Color.White);
                        graphics.FillRectangle(whiteBrush, 0, 0, bitMap.Width, bitMap.Height);

                        //Black Brush is used to draw the Barcode over the Image.
                        SolidBrush blackBrush = new SolidBrush(Color.Black);
                        graphics.DrawString("*" + barcode + "*", oFont, blackBrush, point);
                    }

                    //The Bitmap is saved to Memory Stream.
                    bitMap.Save(ms, ImageFormat.Png);

                    //The Image is finally converted to Base64 string.
                    ImageBarCode = "data:image/png;base64," + Convert.ToBase64String(ms.ToArray());
                }
            }

            return ImageBarCode;
        }
        #endregion
    }
}
