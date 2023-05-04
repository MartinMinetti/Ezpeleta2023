using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Ezpeleta2023.Data;
using Ezpeleta2023.Models;
using Microsoft.AspNetCore.Authorization;

namespace Ezpeleta2023.Controllers;
[Authorize]
public class CategoriasController : Controller 
{
    private readonly ILogger<CategoriasController>? _logger;
    private Ezpeleta2023DbContext _contexto;

    public CategoriasController(ILogger<CategoriasController>? logger, Ezpeleta2023DbContext contexto)
    {
        _logger = logger;
        _contexto = contexto;
    }

    public IActionResult Index()
    {
        return View();
    }


    public JsonResult BuscarCategorias(int categoriaID = 0)
    {
       var categorias = _contexto.Categorias.ToList();

       if (categoriaID > 0){
        categorias = categorias.Where(c => c.CategoriaID == categoriaID).OrderBy(c => c.Descripcion).ToList();
       }

       return Json(categorias);
    }

    public JsonResult GuardarCategoria(int categoriaID, string descripcion)
    {
        bool resultado = false;
         if (!string.IsNullOrEmpty(descripcion))
         {
            //SI ES 0 QUIERE DECIR QUE ESTA CREANDO LA CATEGORIA
            if(categoriaID == 0){
                //BUSCAMOS EN LA TABLA SI EXISTE UNA CON LA MISMA DESCRIPCION
                var categoriaOriginal = _contexto.Categorias.Where(c => c.Descripcion == descripcion).FirstOrDefault();
                if (categoriaOriginal == null){
                    //DECLAMOS EL OBJETO DANDO EL VALOR
                    var categoriaGuardar = new Categoria {
                        Descripcion = descripcion
                    };

                    categoriaGuardar.Descripcion = categoriaGuardar.Descripcion.ToUpper();
                    _contexto.Add(categoriaGuardar);
                    _contexto.SaveChanges();
                    resultado = true;
                }
            }
         
            else{
                //BUSCAMOS EN LA TABLA SI EXISTE UNA CON LA MISMA DESCRIPCION Y DISTINTO ID DE REGISTRO AL QUE ESTAMOS EDITANDO
                var categoriaOriginal = _contexto.Categorias.Where(c => c.Descripcion == descripcion && c.CategoriaID != categoriaID).FirstOrDefault();
                if(categoriaOriginal == null){
                    //CREAR VARIABLE QUE GUARDE EL OBJETO SEGUN EL ID DESEADO
                    var categoriaEditar = _contexto.Categorias.Find(categoriaID);
                    if (categoriaEditar != null){
                        categoriaEditar.Descripcion = descripcion;
                        categoriaEditar.Descripcion = categoriaEditar.Descripcion.ToUpper();
                        _contexto.SaveChanges();
                        resultado = true;
                    }
                }
            

            }

         }

        return Json(resultado);
    }


    public JsonResult EliminarCategoria(int categoriaID, int elimina)
    {

        int resultado = 0; 
        

        var categoria = _contexto.Categorias.Find(categoriaID);

        if (categoria != null){      
            if(categoria.Eliminada == true){
            
                categoria.Eliminada = false;
                _contexto.SaveChanges();
                
  
            }
            else{

                //NO PUEDE ELIMINAR RUBRO SI TIENE SUBRUBROS ACTIVOS
                var cantidadSubCategorias = (from o in _contexto.Subcategorias where o.CategoriaID == categoriaID && o.Eliminada == false select o).Count();
                if(cantidadSubCategorias == 0){
                    categoria.Eliminada = true;
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
