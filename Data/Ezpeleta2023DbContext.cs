using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Ezpeleta2023.Models;

namespace Ezpeleta2023.Data;

public class Ezpeleta2023DbContext : DbContext
{
    public Ezpeleta2023DbContext(DbContextOptions<Ezpeleta2023DbContext> options)
        : base(options)
    {
    }

    public DbSet<Categoria> Categorias { get; set; }

    public DbSet<Subcategoria> Subcategorias { get; set; }

    public DbSet<Servicio> Servicios { get; set; }
    
}