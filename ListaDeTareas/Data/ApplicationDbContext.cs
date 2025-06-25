using Microsoft.EntityFrameworkCore;
using ListaDeTareas.Models;

namespace ListaDeTareas.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        // Le decimos a EF que existe una tabla de "Tareas" que usará el modelo "Tarea"
        public DbSet<Tarea> Tareas { get; set; }
    }
}