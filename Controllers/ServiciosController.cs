using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ezpeleta2023.Data;
using Ezpeleta2023.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Ezpeleta2023.Controllers;

[Authorize]

public class ServiciosController : Controller 
{
    private readonly ILogger<ServiciosController>? _logger;
    private Ezpeleta2023DbContext _contexto;

    public ServiciosController(ILogger<ServiciosController>? logger, Ezpeleta2023DbContext contexto)
    {
        _logger = logger;
        _contexto = contexto;
    }

    public IActionResult Index()
    {
        //Combo para llamar las categorias que estan activas
        var subcategorias = _contexto.Subcategorias.Where(p => p.Eliminada == false).ToList();
        ViewBag.SubcategoriaID = new SelectList(subcategorias.OrderBy(p => p.Descripcion), "SubcategoriaID", "Descripcion");
        
        return View();
    }

    public JsonResult BuscarServicios( int ServicioID = 0) 
    {
        
            List<ListadoServicios> mostrarServicio = new List<ListadoServicios>();
            
            var servicios = _contexto.Servicios.Include(s => s.Subcategoria).Include(s => s.Subcategoria.Categoria).ToList();
            
            if (ServicioID > 0)
            {
                servicios = servicios.Where(s => s.ServicioID == ServicioID).OrderBy(s => s.Descripcion).ToList();
            }

            foreach (var servicio in servicios)
            {
                var servicioMostrar = new ListadoServicios
                {
                    ServicioID = servicio.ServicioID,
                    Descripcion = servicio.Descripcion,
                    Direccion = servicio.Direccion,
                    Telefono = servicio.Telefono,
                    SubcategoriaID = servicio.SubcategoriaID,
                    SubcategoriaNombre = servicio.Subcategoria.Descripcion,
                    CategoriaID = servicio.Subcategoria.CategoriaID,
                    CategoriaNombre = servicio.Subcategoria.Categoria.Descripcion,
                    Eliminado = servicio.Eliminado
                };
                mostrarServicio.Add(servicioMostrar);
            }    

            
        
        return Json(mostrarServicio);
    }

        public JsonResult GuardarServicio(int ServicioID, string Descripcion, string Direccion, string Telefono, int SubcategoriaID){
        
        int resultado = 0;

            //SI ES 0 - ES CORRECTO
            //SI ES 1 - CAMPO DESCRIPCION ESTÁ VACIO
            //SI ES 2 - EL REGISTRO YA EXISTE CON LA MISMA DESCRIPCION        

        if(!string.IsNullOrEmpty(Descripcion))
        {
            Descripcion = Descripcion.ToUpper();
            if (ServicioID == 0)
            {
                //ANTES DE CREAR EL REGISTRO DEBEMOS PREGUNTAR SI EXISTE UNO CON LA MISMA DESCRIPCION
                if(_contexto.Servicios.Any(e => e.Descripcion == Descripcion && e.SubcategoriaID == SubcategoriaID))
                {
                    resultado = 2;
                }
                else
                {
                    //CREA EL REGISTRO DEL SERVICIO
                    //PARA ESO CREAMOS UN OBJETO DE TIPO SERVICIO CON LOS DATOS NECESARIOS
                    var servicio = new Servicio
                    {
                        Descripcion = Descripcion,
                        Telefono =  Telefono,
                        Direccion = Direccion,
                        SubcategoriaID = SubcategoriaID
                    };
                    _contexto.Add(servicio);
                    _contexto.SaveChanges();
                }
            }
            else
            {
                // ANTES DE EDITAR EL SERVICIO DEBEMOS PREGUNTAR SI EXISTE UNO CON LA MISMA DESCRIPCION
                if(_contexto.Servicios.Any(e => e.Descripcion == Descripcion && e.SubcategoriaID == SubcategoriaID && e.ServicioID != ServicioID))
                {
                    resultado = 2;
                }
                else
                {
                    //EDITA EL SERVICIO
                    //BUSCAMOS EL SERVICIO EN LA BASE DE DATOS
                    var servicio = _contexto.Servicios.Single(m => m.ServicioID == ServicioID);
                    //CAMBIAMOS LA DESCRIPCIÓN POR LA QUE INGRESÓ EL USUARIO EN LA VISTA
                    servicio.Descripcion = Descripcion;
                    servicio.Telefono =  Telefono;
                    servicio.Direccion = Direccion;
                    servicio.SubcategoriaID = SubcategoriaID;
                    _contexto.SaveChanges();
                }
            }
        }
        else
        {
            resultado = 1;
        }

        return Json(resultado);
    }



    public JsonResult EliminarServicio(int ServicioID, bool Habilitar)
    {
        int resultado = 0;
        
        

        var servicio = _contexto.Servicios.Find(ServicioID);
        if (servicio != null)
        {

            // if(Habilitar == false){

            //     servicio.Eliminado = true;
                

            //     var subCategoria = _contexto.Subcategorias.Where(s => s.Eliminada == servicio.Eliminado).Single();
            //     subCategoria.Eliminada = true;

            //     var categoria = _contexto.Categorias.Where(s => s.Eliminada == servicio.Eliminado).Single();
            //     categoria.Eliminada = true;

            //     _contexto.SaveChanges();
            // }
            // else
            // {
            //     servicio.Eliminado = false;
            //     _contexto.SaveChanges();


            // }

            if (servicio.Eliminado == false)
            {
                servicio.Eliminado = true;
                _contexto.SaveChanges();

                
            }
            else{
                servicio.Eliminado = false;
                _contexto.SaveChanges();
            }
            resultado = 1;
        }

        return Json(resultado);
    }

}