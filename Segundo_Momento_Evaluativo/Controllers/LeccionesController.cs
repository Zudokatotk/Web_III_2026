using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Segundo_Momento_Evaluativo.Data;
using Segundo_Momento_Evaluativo.Models;

namespace Segundo_Momento_Evaluativo.Controllers
{
    public class LeccionesController : Controller
    {
        private readonly AppDbContext _context;

        public LeccionesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Lecciones
        public async Task<IActionResult> Index(int? cursoId)
        {
            var query = _context.Lecciones.Include(l => l.Curso).AsQueryable();

            if (cursoId.HasValue)
            {
                query = query.Where(l => l.CursoId == cursoId.Value);
                ViewData["CursoSeleccionado"] = await _context.Cursos.FindAsync(cursoId.Value);
            }

            return View(await query.OrderBy(l => l.Orden).ToListAsync());
        }

        // GET: Lecciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccion = await _context.Lecciones
                .Include(l => l.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (leccion == null)
            {
                return NotFound();
            }

            return View(leccion);
        }

        // GET: Lecciones/Create
        public IActionResult Create(int? cursoId)
        {
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", cursoId);
            var model = new Leccion();
            if (cursoId.HasValue)
            {
                model.CursoId = cursoId.Value;
            }
            return View(model);
        }

        // POST: Lecciones/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CursoId,Titulo,Contenido,Orden")] Leccion leccion)
        {
            if (ModelState.IsValid)
            {
                leccion.FechaCreacion = DateTime.UtcNow;
                _context.Add(leccion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { cursoId = leccion.CursoId });
            }
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", leccion.CursoId);
            return View(leccion);
        }

        // GET: Lecciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccion = await _context.Lecciones.FindAsync(id);
            if (leccion == null)
            {
                return NotFound();
            }
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", leccion.CursoId);
            return View(leccion);
        }

        // POST: Lecciones/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CursoId,Titulo,Contenido,Orden,FechaCreacion")] Leccion leccion)
        {
            if (id != leccion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leccion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeccionExists(leccion.Id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index), new { cursoId = leccion.CursoId });
            }
            ViewData["CursoId"] = new SelectList(_context.Cursos, "Id", "Titulo", leccion.CursoId);
            return View(leccion);
        }

        // GET: Lecciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leccion = await _context.Lecciones
                .Include(l => l.Curso)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (leccion == null)
            {
                return NotFound();
            }

            return View(leccion);
        }

        // POST: Lecciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leccion = await _context.Lecciones.FindAsync(id);
            int cursoId = leccion?.CursoId ?? 0;
            if (leccion != null)
            {
                _context.Lecciones.Remove(leccion);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { cursoId });
        }

        private bool LeccionExists(int id)
        {
            return _context.Lecciones.Any(e => e.Id == id);
        }
    }
}
