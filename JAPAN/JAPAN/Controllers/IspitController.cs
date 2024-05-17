using System.Security.Claims;
using JAPAN.Data;
using JAPAN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JAPAN.Controllers
{
    public class IspitController : Controller
    {
        private readonly JapanContext _context;

        public IspitController(JapanContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Korisnik")]
        [Route("Ispit")]
        public async Task<IActionResult> KorisnikIspiti()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Korisnici.Include(u => u.Statistike)
                                               .FirstOrDefaultAsync(u => u.Identifikator == userId);
            var ispiti = await _context.Ispiti.Include(i => i.Tezina)
                                              .OrderBy(i => i.Pozicija)
                                              .ToListAsync();

            var rezultati = new Dictionary<int, string?>();
            foreach (var ispit in ispiti)
            {
                var rezultat = user.Statistike.FirstOrDefault(s => s.Idispit == ispit.Id)?.Rezultat;
                rezultati.Add(ispit.Id, rezultat);
            }

            var viewModel = new KorisnikIspitiViewModel
            {
                Ispiti = ispiti,
                Rezultati = rezultati
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Korisnik")]
        [Route("Ispit/{id}")]
        public async Task<IActionResult> KorisnikIspit(int id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Korisnici.Include(u => u.Statistike)
                                               .FirstOrDefaultAsync(u => u.Identifikator == userId);

            var ispit = await _context.Ispiti.Include(i => i.Tezina)
                                              .FirstOrDefaultAsync(i => i.Id == id);

            if (ispit == null)
            {
                return NotFound();
            }

            var rezultat = user.Statistike.FirstOrDefault(s => s.Idispit == id)?.Rezultat;

            var viewModel = new KorisnikIspitViewModel
            {
                Ispit = ispit,
                Rezultat = rezultat
            };

            return View(viewModel);
        }
    }
}
