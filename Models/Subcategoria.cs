using System.ComponentModel.DataAnnotations;

namespace Ezpeleta2023.Models
{
    public class Subcategoria{

        [Key]
        public int SubcategoriaID { get; set; }

        public string? Descripcion { get; set; }
        
        public int CategoriaID { get; set; }

        public bool Eliminada { get; set; }


        public virtual Categoria? Categoria { get; set; }


        public virtual ICollection<Servicio> Servicios { get; set; }
    }
  

  //Se crea esta vista, se usa para ejecucion no se guarda en la base! Porque la anterior era muy grande. CLASE DE VISTA
    public class ListadoSubcategorias{
        public int SubcategoriaID { get; set; }

        public string? Descripcion { get; set; }
        
        public int CategoriaID { get; set; }

        public string? CategoriaNombre { get; set; }

        public bool Eliminada { get; set; }

    }

   
}