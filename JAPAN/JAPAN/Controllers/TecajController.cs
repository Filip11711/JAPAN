using System.Security.Claims;
using JAPAN.Data;
using JAPAN.Data.Entities;
using JAPAN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace JAPAN.Controllers
{
    public class TecajController : Controller
    {
        private readonly JapanContext _context;

        public TecajController(JapanContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Korisnik")]
        [Route("Tecaj")]
        public async Task<IActionResult> KorisnikTecaji()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Korisnici.Include(u => u.Statistike)
                                               .FirstOrDefaultAsync(u => u.Identifikator == userId);
            var tecaji = await _context.Tecaji.Include(t => t.Tipsadrzaja)
                                              .Include(t => t.Tezina)
                                              .OrderBy(t => t.Pozicija)
                                              .ToListAsync();

            var rezultati = new Dictionary<int, string?>();
            foreach (var tecaj in tecaji)
            {
                var rezultat = user.Statistike.FirstOrDefault(s => s.Idtecaj == tecaj.Id)?.Rezultat;
                rezultati.Add(tecaj.Id, rezultat);
            }

            var viewModel = new KorisnikTecajiViewModel
            {
                Tecaji = tecaji,
                Rezultati = rezultati
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Korisnik")]
        [Route("Tecaj/{id}")]
        public async Task<IActionResult> KorisnikTecaj(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Korisnici.Include(u => u.Statistike)
                                               .FirstOrDefaultAsync(u => u.Identifikator == userId);

            var tecaj = await _context.Tecaji.Include(t => t.Tipsadrzaja)
                                              .Include(t => t.Tezina)
                                              .FirstOrDefaultAsync(t => t.Id == id);

            if (tecaj == null)
            {
                return NotFound();
            }

            var rezultat = user.Statistike.FirstOrDefault(s => s.Idtecaj == tecaj.Id)?.Rezultat;

            var viewModel = new KorisnikTecajViewModel
            {
                Korisnik = user,
                Tecaj = tecaj,
                Rezultat = rezultat
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Korisnik")]
        [Route("Tecaj/Zavrsi")]
        public async Task<IActionResult> KorisnikTecajZavrsi(int userId, int tecajId)
        {
            var user = await _context.Korisnici.FirstOrDefaultAsync(u => u.Id == userId);
            var tecaj = await _context.Tecaji.FirstOrDefaultAsync(t => t.Id == tecajId);

            if (user != null && tecaj != null)
            {
                var statistika = new Statistika
                {
                    Rezultat = "Završeno",
                    Zavrseno = DateOnly.FromDateTime(DateTime.Now),
                    Idkorisnik = userId,
                    Idtecaj = tecajId,
                    Korisnik = user,
                    Tecaj = tecaj
                };
                _context.Add(statistika);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("KorisnikTecaji");
        }

        [Authorize(Roles = "Moderator")]
        [Route("Tecaji")]
        public async Task<IActionResult> ModeratorTecaji()
        {
            var tecaji = await _context.Tecaji.Include(t => t.Tipsadrzaja)
                                              .Include(t => t.Tezina)
                                              .OrderBy(t => t.Pozicija)
                                              .ToListAsync();

            var viewModel = new ModeratorTecajiViewModel
            {
                Tecaji = tecaji
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Moderator")]
        [HttpPost]
        [Route("Moderator/Tecaji/UpdateOrder")]
        public async Task<IActionResult> PromjeniPoredak([FromBody] List<TecajPoredakViewModel> poredak)
        {
            foreach (var item in poredak)
            {
                var tecaj = await _context.Tecaji.FindAsync(item.Id);
                if (tecaj != null)
                {
                    tecaj.Pozicija = item.Pozicija;
                    _context.Tecaji.Update(tecaj);
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize(Roles = "Moderator")]
        [Route("Tecaji/{id}")]
        public async Task<IActionResult> ModeratorTecaj(int id)
        {
            var tecaj = await _context.Tecaji.Include(t => t.Tipsadrzaja)
                                              .Include(t => t.Tezina)
                                              .FirstOrDefaultAsync(t => t.Id == id);

            if (tecaj == null)
            {
                return NotFound();
            }

            var viewModel = new ModeratorTecajViewModel
            {
                Tecaj = tecaj
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Moderator")]
        [Route("Tecaji/Uredi/{id}")]
        public async Task<IActionResult> ModeratorTecajUredi(int id)
        {
            var tecaj = await _context.Tecaji.FindAsync(id);
            if (tecaj == null)
            {
                return NotFound();
            }

            var viewModel = new UrediTecajViewModel
            {
                Id = tecaj.Id,
                Naziv = tecaj.Naziv,
                Opis = tecaj.Opis,
                Sadrzaj = tecaj.Sadrzaj,
                TezinaId = tecaj.Idtezina,
                TipSadrzajaId = tecaj.Idtipsadrzaj
            };

            ViewBag.TipoviSadrzaja = new SelectList(_context.TipoviSadrzaja, "Id", "Naziv", tecaj.Idtipsadrzaj);
            ViewBag.Tezine = new SelectList(_context.Tezine, "Id", "Naziv", tecaj.Idtezina);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        [Route("Tecaj/Uredeno")]
        public async Task<IActionResult> ModeratorUredeno(UrediTecajViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tecaj = await _context.Tecaji.FindAsync(viewModel.Id);
            if (tecaj == null)
            {
                return NotFound();
            }

            tecaj.Naziv = viewModel.Naziv;
            tecaj.Opis = viewModel.Opis;
            tecaj.Sadrzaj = viewModel.Sadrzaj;
            tecaj.Idtezina = viewModel.TezinaId;
            tecaj.Idtipsadrzaj = viewModel.TipSadrzajaId;

            _context.Update(tecaj);
            await _context.SaveChangesAsync();

            return RedirectToAction("ModeratorTecaji");
        }

        [Authorize(Roles = "Moderator")]
        [Route("Tecaji/Novi")]
        public IActionResult ModeratorTecajNovi()
        {
            var viewModel = new NoviTecajViewModel
            {
                TezinaId = 1,
                TipSadrzajaId = 1
            };

            ViewBag.TipoviSadrzaja = new SelectList(_context.TipoviSadrzaja, "Id", "Naziv", 1);
            ViewBag.Tezine = new SelectList(_context.Tezine, "Id", "Naziv", 1);

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Moderator")]
        [Route("Tecaj/Novo")]
        public async Task<IActionResult> ModeratorNovo(NoviTecajViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tecaj = new Tecaj();

            tecaj.Naziv = viewModel.Naziv;
            tecaj.Opis = viewModel.Opis;
            tecaj.Sadrzaj = viewModel.Sadrzaj;
            tecaj.Kreirano = DateOnly.FromDateTime(DateTime.Now);
            tecaj.Pozicija = _context.Tecaji.ToList().Last().Pozicija + 1;
            tecaj.Idtezina = viewModel.TezinaId;
            tecaj.Idtipsadrzaj = viewModel.TipSadrzajaId;

            _context.Add(tecaj);
            await _context.SaveChangesAsync();

            return RedirectToAction("ModeratorTecaji");
        }

        [Authorize(Roles = "Moderator")]
        [HttpDelete]
        [Route("Tecaji/Obrisi/{id}")]
        public async Task<IActionResult> ModeratorObrisiTecaj(int id)
        {
            var tecaj = await _context.Tecaji.FindAsync(id);
            if (tecaj == null)
            {
                return NotFound();
            }

            _context.Tecaji.Remove(tecaj);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
