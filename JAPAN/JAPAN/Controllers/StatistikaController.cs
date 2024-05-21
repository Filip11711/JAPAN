using System.Security.Claims;
using JAPAN.Data;
using JAPAN.Data.Entities;
using JAPAN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JAPAN.Controllers
{
    public class StatistikaController : Controller
    {
        private readonly JapanContext _context;

        public StatistikaController(JapanContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Korisnik")]
        [Route("Statistika")]
        public async Task<IActionResult> KorisnikStatistike()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Korisnici.Include(u => u.Statistike)
                                               .ThenInclude(v => v.Tecaj)
                                               .ThenInclude(t => t.Tezina)
                                               .Include(u => u.Statistike)
                                               .ThenInclude(v => v.Ispit)
                                               .ThenInclude(i => i.Tezina)
                                               .Include(u => u.Uloga)
                                               .FirstOrDefaultAsync(u => u.Identifikator == userId);


            List<Statistika> statistike_tecaja = [.. user.Statistike.Where(s => s.Idispit == null).OrderBy(s => s.Tecaj.Pozicija)];
            List<Statistika> statistike_ispita = [.. user.Statistike.Where(s => s.Idtecaj == null).OrderBy(s => s.Ispit.Pozicija)];

            return View(new KorisnikStatistikeViewModel
            {
                Statistike_tecaja = statistike_tecaja,
                Statistike_ispita = statistike_ispita
            });
        }

        [Authorize(Roles = "Admin,Moderator")]
        [Route("Statistike")]
        public async Task<IActionResult> AllStatistike()
        {
            var statistike = await _context.Statistike.Include(s => s.Tecaj)
                                                      .Include(s => s.Ispit)
                                                      .Include(s => s.Korisnik)
                                                      .ToListAsync();

            var tecaji = await _context.Tecaji.Include(t => t.Tezina).ToListAsync();
            var ispiti = await _context.Ispiti.Include(i => i.Tezina).ToListAsync();

            var tecajStatistike = tecaji.Select(t => new TecajStatistike
            {
                Tecaj = t,
                Statistike = [.. statistike.Where(s => s.Idtecaj == t.Id).OrderBy(s => s.Idkorisnik)]
            }).ToList();

            var ispitStatistike = ispiti.Select(i => new IspitStatistike
            {
                Ispit = i,
                Statistike = [.. statistike.Where(s => s.Idispit == i.Id).OrderBy(s => s.Idkorisnik)]
            }).ToList();

            var viewModel = new StatistikeViewModel
            {
                TecajStatistike = tecajStatistike,
                IspitStatistike = ispitStatistike
            };

            return View(viewModel);

        }
    }
}
