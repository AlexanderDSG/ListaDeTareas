using ListaDeTareas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Data;
using ListaDeTareas.Models;

namespace ListaDeTareas.Controllers
{
    public class HomeController : Controller
    {
        private readonly string _conexion;

        public HomeController(IConfiguration config)
        {
            _conexion = config.GetConnectionString("DefaultConnection");
        }

        // Lista todas las tareas
        public IActionResult Index()
        {
            var tareas = new List<Tarea>();
            using (var conn = new MySqlConnection(_conexion))
            {
                conn.Open();
                var query = "SELECT * FROM tareas";
                var cmd = new MySqlCommand(query, conn);
                var rd = cmd.ExecuteReader();

                while (rd.Read())
                    tareas.Add(new Tarea
                    {
                        Id = rd.GetInt32("id"),
                        Titulo = rd.GetString("titulo"),
                        Descripcion = rd.IsDBNull("descripcion") ? "" : rd.GetString("descripcion"),
                        Estado = rd.GetString("estado")
                    });
            }
            return View(tareas);
        }

        // Muestra el formulario de creación
        [HttpGet]
        public IActionResult Crear() => View();

        // Guarda la nueva tarea
        [HttpPost]
        public IActionResult Crear(Tarea tarea)
        {
            using (var conn = new MySqlConnection(_conexion))
            {
                conn.Open();
                var sql = "INSERT INTO tareas (titulo, descripcion, estado) VALUES (@t, @d, @e)";
                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@t", tarea.Titulo);
                cmd.Parameters.AddWithValue("@d", tarea.Descripcion ?? "");
                cmd.Parameters.AddWithValue("@e", tarea.Estado ?? "pendiente");
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // Cambia el estado de una tarea (pendiente <-> completada)
        [HttpGet]
        public IActionResult CambiarEstado(int id)
        {
            using (var conn = new MySqlConnection(_conexion))
            {
                conn.Open();
                // Obtenemos el estado actual
                var sql1 = "SELECT estado FROM tareas WHERE id = @id";
                var cmd1 = new MySqlCommand(sql1, conn);
                cmd1.Parameters.AddWithValue("@id", id);
                var actual = cmd1.ExecuteScalar()?.ToString();

                // Calculamos el nuevo
                var nuevo = actual == "pendiente" ? "completada" : "pendiente";

                // Actualizamos
                var sql2 = "UPDATE tareas SET estado = @e WHERE id = @id";
                var cmd2 = new MySqlCommand(sql2, conn);
                cmd2.Parameters.AddWithValue("@e", nuevo);
                cmd2.Parameters.AddWithValue("@id", id);
                cmd2.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }

        // Elimina una tarea
        [HttpGet]
        public IActionResult Eliminar(int id)
        {
            using (var conn = new MySqlConnection(_conexion))
            {
                conn.Open();
                var sql = "DELETE FROM tareas WHERE id = @id";
                var cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }
            return RedirectToAction("Index");
        }
    }
}
