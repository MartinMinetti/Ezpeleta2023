using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ezpeleta2023.Data;
using Ezpeleta2023.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Ezpeleta2023.Controllers;
[Authorize]
public class SubcategoriasController : Controller 
{
    private readonly ILogger<SubcategoriasController>? _logger;
    private Ezpeleta2023DbContext _contexto;

    public SubcategoriasController(ILogger<SubcategoriasController>? logger, Ezpeleta2023DbContext contexto)
    {
        _logger = logger;
        _contexto = contexto;
    }

    public IActionResult Index()
    {
        //Combo para llamar las categorias que estan activas
        var categorias = _contexto.Categorias.Where(p => p.Eliminada == false).ToList();
        ViewBag.CategoriaID = new SelectList(categorias.OrderBy(p => p.Descripcion), "CategoriaID", "Descripcion");
        
        return View();
    }

    public JsonResult BuscarSubcategorias(int SubcategoriaID = 0)
    {
        
        List<ListadoSubcategorias> mostrarSubcategoria = new List<ListadoSubcategorias>();

        var subcategorias = _contexto.Subcategorias.Include(s => s.Categoria).ToList();
        
        if (SubcategoriaID > 0)
        {
           subcategorias = subcategorias.Where(s => s.SubcategoriaID == SubcategoriaID).OrderBy(s => s.Descripcion).ToList();
        }   

            foreach (var subcategoria in subcategorias)
            {
                var subCategoriaMostrar = new ListadoSubcategorias
                {
                    SubcategoriaID = subcategoria.SubcategoriaID,
                    Descripcion = subcategoria.Descripcion,
                    CategoriaID = subcategoria.CategoriaID,
                    CategoriaNombre = subcategoria.Categoria.Descripcion,
                    Eliminada = subcategoria.Eliminada
                };
                mostrarSubcategoria.Add(subCategoriaMostrar);
            }    

        return Json(mostrarSubcategoria);

    }



    public JsonResult GuardarSubcategoria(int SubcategoriaID, string Descripcion, int CategoriaID){
        int resultado = 0;

            //SI ES 0 - ES CORRECTO
            //SI ES 1 - CAMPO DESCRIPCION ESTÁ VACIO
            //SI ES 2 - EL REGISTRO YA EXISTE CON LA MISMA DESCRIPCION        

        if(!string.IsNullOrEmpty(Descripcion))
        {
            Descripcion = Descripcion.ToUpper();
            if (SubcategoriaID == 0)
            {
                //ANTES DE CREAR EL REGISTRO DEBEMOS PREGUNTAR SI EXISTE UNO CON LA MISMA DESCRIPCION
                if(_contexto.Subcategorias.Any(e => e.Descripcion == Descripcion && e.CategoriaID == CategoriaID))
                {
                    resultado = 2;
                }
                else
                {
                    //CREA EL REGISTRO DE RUBRO
                    //PARA ESO CREAMOS UN OBJETO DE TIPO RUBRO CON LOS DATOS NECESARIOS
                    var subcategoria = new Subcategoria
                    {
                        Descripcion = Descripcion,
                        CategoriaID = CategoriaID
                    };
                    _contexto.Add(subcategoria);
                    _contexto.SaveChanges();
                }
            }
            else
            {
                //ANTES DE EDITAR EL REGISTRO DEBEMOS PREGUNTAR SI EXISTE UNO CON LA MISMA DESCRIPCION
                if(_contexto.Subcategorias.Any(e => e.Descripcion == Descripcion && e.CategoriaID == CategoriaID && e.SubcategoriaID != SubcategoriaID))
                {
                    resultado = 2;
                }
                else
                {
                    //EDITA EL REGISTRO
                    //BUSCAMOS EL REGISTRO EN LA BASE DE DATOS
                    var subcategoria = _contexto.Subcategorias.Single(m => m.SubcategoriaID == SubcategoriaID);
                    //CAMBIAMOS LA DESCRIPCIÓN POR LA QUE INGRESÓ EL USUARIO EN LA VISTA
                    subcategoria.Descripcion = Descripcion;
                    subcategoria.CategoriaID = CategoriaID;
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


    public JsonResult EliminarSubcategoria(int SubcategoriaID, int Elimina)
    {
        int resultado = 0;

        var subCategoria = _contexto.Subcategorias.Find(SubcategoriaID);
        if (subCategoria != null)
        {

            if (subCategoria.Eliminada == true)
            {
                subCategoria.Eliminada = false;
                _contexto.SaveChanges();
            }
            else{
                //NO PUEDE ELIMINAR RUBRO SI TIENE SUBRUBROS ACTIVOS
                var cantidadServicios = (from o in _contexto.Servicios where o.SubcategoriaID == SubcategoriaID && o.Eliminado == false select o).Count();
                if(cantidadServicios == 0){
                    subCategoria.Eliminada = true;
                    _contexto.SaveChanges();
                }
                else
                {
                      resultado = 1;
                }  
            }
              
        }

        return Json(resultado);
    }



}  