using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Segundo_Momento_Evaluativo.Data;
using Segundo_Momento_Evaluativo.Models;

namespace Segundo_Momento_Evaluativo.Controllers
{
    public class InscripcionesController : Controller
    {
        private readonly AppDbContext _context;

        public InscripcionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Inscripciones
        public async Task<IActionResult> Index()
        {
            var inscripciones = await _context.Inscripciones
                .Include(i => i.Curso)
                .OrderByDescending(i => i.FechaInscripcion)
                .ToListAsync();

            return View(inscripciones);
        }

        // GET: Inscripciones/Create
        public IActionResult Create()
        {
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo");
            return View();
        }

        // POST: Inscripciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CursoId,NombreEstudiante,CorreoEstudiante")] Inscripcion inscripcion)
        {
            if (ModelState.IsValid)
            {
                bool yaInscrito = await _context.Inscripciones
                    .AnyAsync(i => i.CursoId == inscripcion.CursoId && i.CorreoEstudiante == inscripcion.CorreoEstudiante);

                if (yaInscrito)
                {
                    ModelState.AddModelError("CorreoEstudiante", "Este estudiante ya se encuentra inscrito en este curso.");
                    ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", inscripcion.CursoId);
                    return View(inscripcion);
                }

                inscripcion.FechaInscripcion = DateTime.UtcNow;
                _context.Add(inscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", inscripcion.CursoId);
            return View(inscripcion);
        }

        // GET: Inscripciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var inscripcion = await _context.Inscripciones
                .Include(i => i.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (inscripcion == null)
            {
                return NotFound();
            }

            return View(inscripcion);
        }

        // POST: Inscripciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var inscripcion = await _context.Inscripciones.FindAsync(id);
            if (inscripcion != null)
            {
                _context.Inscripciones.Remove(inscripcion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
