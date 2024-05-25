using JAPAN.Data;
using JAPAN.Data.Entities;
using JAPAN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JAPAN.Controllers
{
    public class PitanjeController : Controller
    {
        readonly private JapanContext _context;

        public PitanjeController(JapanContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Moderator")]
        [Route("Pitanja")]
        public async Task<IActionResult> ModeratorPitanja()
        {
            var pitanja = await _context.Pitanja.Include(p => p.Odgovori)
                                              .ToListAsync();

            var viewModel = new ModeratorPitanjaViewModel
            {
                Pitanja = pitanja
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Moderator")]
        [HttpDelete]
        [Route("Pitanja/Obrisi/{id}")]
        public async Task<IActionResult> ModeratorObrisiPitanje(int id)
        {
            var pitanje = await _context.Pitanja.FindAsync(id);
            if (pitanje == null)
            {
                return NotFound();
            }

            _context.Pitanja.Remove(pitanje);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Moderator")]
        [Route("Pitanja/{id}")]
        public async Task<IActionResult> ModeratorPitanje(int id)
        {
            var pitanje = await _context.Pitanja.Include(p => p.Odgovori)
                                              .FirstOrDefaultAsync(t => t.Id == id);

            if (pitanje == null)
            {
                return NotFound();
            }

            var viewModel = new ModeratorPitanjeViewModel
            {
                Pitanje = pitanje
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Moderator")]
        [Route("Pitanja/Novi")]
        public IActionResult ModeratorPitanjeNovi()
        {
            var viewModel = new NovoPitanjeViewModel();

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        [Route("Pitanja/Novo")]
        public async Task<IActionResult> ModeratorNoviPitanje(NovoPitanjeViewModel viewModel)
        {
            foreach (var odgovor in viewModel.Odgovori)
            {
                odgovor.Pitanje = viewModel.Pitanje;
            }

            foreach (var key in ModelState.Keys)
            {
                if (key.StartsWith("Odgovori[") && key.EndsWith("].Pitanje"))
                {
                    ModelState[key].ValidationState = ModelValidationState.Valid;
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pitanje = viewModel.Pitanje;
            var odgovori = viewModel.Odgovori;

            _context.Pitanja.Add(pitanje);
            await _context.SaveChangesAsync();

            foreach (var odgovor in odgovori)
            {
                odgovor.Idpitanje = pitanje.Id;
                _context.Odgovori.Add(odgovor);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ModeratorPitanja");
        }

        [Authorize(Roles = "Moderator")]
        [Route("Pitanja/Uredi/{id}")]
        public async Task<IActionResult> ModeratorPitanjeUredi(int id)
        {
            var pitanje = await _context.Pitanja.Include(p => p.Odgovori).FirstOrDefaultAsync(p => p.Id == id);

            if (pitanje == null)
            {
                return NotFound();
            }

            var viewModel = new NovoPitanjeViewModel {
                Pitanje = pitanje,
                Odgovori = [.. pitanje.Odgovori]
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        [Route("Pitanja/Uredeno")]
        public async Task<IActionResult> ModeratorUredeno(NovoPitanjeViewModel viewModel)
        {
            foreach (var odgovor in viewModel.Odgovori)
            {
                if (odgovor.Pitanje == null)
                {
                    odgovor.Pitanje = viewModel.Pitanje;
                }
            }

            foreach (var key in ModelState.Keys)
            {
                if (key.StartsWith("Odgovori[") && key.EndsWith("].Pitanje"))
                {
                    ModelState[key].ValidationState = ModelValidationState.Valid;
                }
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pitanje = await _context.Pitanja.Include(p => p.Odgovori).FirstOrDefaultAsync(p => p.Id == viewModel.Pitanje.Id);
            var odgovori = viewModel.Odgovori;
            var odgovori_to_remove = pitanje.Odgovori.Where(o => !odgovori.Any(od => od.Id == o.Id));
            _context.Odgovori.RemoveRange(odgovori_to_remove);
            await _context.SaveChangesAsync();

            pitanje.Odgovori = odgovori;
            _context.Pitanja.Update(pitanje);
            await _context.SaveChangesAsync();

            foreach (var odgovor in odgovori)
            {
                odgovor.Idpitanje = pitanje.Id;
                _context.Odgovori.Update(odgovor);
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("ModeratorPitanja");
        }
    }
}
