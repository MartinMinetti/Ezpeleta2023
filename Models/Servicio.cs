using System.ComponentModel.DataAnnotations;

namespace Ezpeleta2023.Models
{
    public class Servicio{

        [Key]
        public int ServicioID { get; set; }



        [Required(ErrorMessage = "Este valor es obligatorio.")]
        public string? Descripcion { get; set; }




        [Required(ErrorMessage = "Este valor es obligatorio.")]
        public string? Direccion { get; set; }





        [Required(ErrorMessage = "Este valor es obligatorio.")]
        public string? Telefono { get; set; }



        public bool Eliminado { get; set; }
        

        public int SubcategoriaID { get; set; }

    

        public virtual Subcategoria? Subcategoria { get; set; }
    }


    public class ListadoServicios{
        public int ServicioID { get; set; }

        public string? Descripcion { get; set; }
        
        public string? Direccion { get; set; }

        public string? Telefono { get; set; }

        public int SubcategoriaID { get; set; }

        public string? SubcategoriaNombre { get; set; }

        public int CategoriaID { get; set; }

        public string? CategoriaNombre { get; set; }

        public bool Eliminado { get; set; }

    }

}