using System.Security.Claims;
using JAPAN.Data;
using JAPAN.Data.Entities;
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

            var ispit = await _context.Ispiti.Include(i => i.Pitanja)
                                             .ThenInclude(p => p.Odgovori)
                                              .FirstOrDefaultAsync(i => i.Id == id);

            if (ispit == null)
            {
                return NotFound();
            }

            var viewModel = new KorisnikIspitViewModel
            {
                Korisnik = user,
                Ispit = ispit
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Korisnik")]
        [Route("Ispit/Zavrsi")]
        public async Task<IActionResult> KorisnikZavrsiIspit(KorisnikIspitOdgovoriViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Korisnici.FirstOrDefaultAsync(u => u.Id == model.UserId);
            var ispit = await _context.Ispiti.Include(i => i.Pitanja)
                                             .ThenInclude(p => p.Odgovori)
                                             .FirstOrDefaultAsync(t => t.Id == model.IspitId);

            int correct = 0;
            int total = 0;

            foreach (var entry in model.PitanjeOdgovori)
            {
                int pitanjeId = entry.Key;
                int? odgovorId = string.IsNullOrEmpty(entry.Value.ToString()) ? null : entry.Value;

                if (odgovorId != null)
                {
                    if (ispit.Pitanja.Where(p => p.Id == pitanjeId).First().Odgovori.Where(o => o.Id == odgovorId).First().Tocno == 1)
                    {
                        correct++;
                    }
                }

                total++;
            }

            foreach (var entry in model.PitanjeOtvoreniOdgovori)
            {
                int pitanjeId = entry.Key;
                string answerText = entry.Value;

                if (ispit.Pitanja.Where(p => p.Id == pitanjeId).First().Odgovori.First().Tekst.ToLower() == answerText.ToLower())
                {
                    correct++;
                }

                total++;
            }

            string rezultat = correct.ToString() + "/" + total.ToString();

            var statistika = await _context.Statistike.FirstOrDefaultAsync(s => s.Idkorisnik == model.UserId && s.Idispit == model.IspitId);

            if (statistika == null)
            {
                statistika = new Statistika
                {
                    Rezultat = rezultat,
                    Zavrseno = DateOnly.FromDateTime(DateTime.Now),
                    Idkorisnik = model.UserId,
                    Idispit = model.IspitId,
                    Korisnik = user,
                    Ispit = ispit
                };
            }
            else
            {
                statistika.Rezultat = rezultat;
                statistika.Zavrseno = DateOnly.FromDateTime(DateTime.Now);
            }

            _context.Update(statistika);
            await _context.SaveChangesAsync();

            return RedirectToAction("KorisnikIspiti");
        }
    }
}
