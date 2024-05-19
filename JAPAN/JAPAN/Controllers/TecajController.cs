using System.Security.Claims;
using JAPAN.Data;
using JAPAN.Data.Entities;
using JAPAN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    }
}
