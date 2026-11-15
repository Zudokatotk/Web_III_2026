using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Segundo_Momento_Evaluativo.Data;
using Segundo_Momento_Evaluativo.Models;

namespace Segundo_Momento_Evaluativo.Controllers
{
    [Authorize]
    public class InscripcionesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public InscripcionesController(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            var query = _context.Inscripciones.Include(i => i.Curso).AsQueryable();

            if (!User.IsInRole("Administrador") && user != null)
            {
                query = query.Where(i => i.UsuarioId == user.Id || i.CorreoEstudiante == user.Email);
            }

            var inscripciones = await query.OrderByDescending(i => i.FechaInscripcion).ToListAsync();
            return View(inscripciones);
        }

        public async Task<IActionResult> Create(int? cursoId)
        {
            ViewData["CursoId"] = new SelectList(_context.Cursos.Where(c => c.EstaPublicado), "Id", "Titulo", cursoId);
            
            var user = await _userManager.GetUserAsync(User);
            var inscripcion = new Inscripcion();
            if (user != null)
            {
                inscripcion.CorreoEstudiante = user.Email ?? "";
                inscripcion.NombreEstudiante = user.UserName?.Split('@')[0] ?? "Estudiante";
            }
            if (cursoId.HasValue)
            {
                inscripcion.CursoId = cursoId.Value;
            }

            return View(inscripcion);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CursoId,NombreEstudiante,CorreoEstudiante")] Inscripcion inscripcion)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                inscripcion.UsuarioId = user.Id;
            }

            if (ModelState.IsValid)
            {
                bool yaInscrito = await _context.Inscripciones
                    .AnyAsync(i => i.CursoId == inscripcion.CursoId && (i.CorreoEstudiante == inscripcion.CorreoEstudiante || (user != null && i.UsuarioId == user.Id)));

                if (yaInscrito)
                {
                    ModelState.AddModelError("CursoId", "Ya te encuentras inscrito en este curso.");
                    ViewData["CursoId"] = new SelectList(_context.Cursos.Where(c => c.EstaPublicado), "Id", "Titulo", inscripcion.CursoId);
                    return View(inscripcion);
                }

                inscripcion.FechaInscripcion = DateTime.UtcNow;
                _context.Add(inscripcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["CursoId"] = new SelectList(_context.Cursos.Where(c => c.EstaPublicado), "Id", "Titulo", inscripcion.CursoId);
            return View(inscripcion);
        }

        [Authorize(Roles = "Administrador")]
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

        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrador")]
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
