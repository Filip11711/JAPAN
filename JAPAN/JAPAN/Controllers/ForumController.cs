using JAPAN.Data;
using JAPAN.Data.Entities;
using JAPAN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JAPAN.Controllers
{
    public class ForumController : Controller
    {
        private readonly JapanContext _context;

        public ForumController(JapanContext context)
        {
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> AllForum()
        {
            var forumQuestions = await _context.ForumPitanja.Include(fp => fp.Korisnik).ToListAsync();
            return View(forumQuestions);
        }

        [Authorize]
        public async Task<IActionResult> Odgovori(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumPitanje = await _context.ForumPitanja
                .Include(fp => fp.Korisnik)
                .Include(fp => fp.ForumOdgovori)
                .ThenInclude(fo => fo.Korisnik)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (forumPitanje == null)
            {
                return NotFound();
            }

            return View(forumPitanje);
        }

        [Authorize]
        public async Task<IActionResult> Create()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Korisnici.FirstOrDefaultAsync(u => u.Identifikator == userId);

            var viewModel = new ForumPitanjeViewModel 
            { 
                UserId = user.Id
            };

            return View(viewModel);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ForumPitanjeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var forumPitanje = new ForumPitanje
            {
                Naslov = model.Naslov,
                Sadrzaj = model.Sadrzaj,
                Kreirano = DateOnly.FromDateTime(DateTime.Now),
                Idkorisnik = model.UserId
            };

            _context.ForumPitanja.Add(forumPitanje);
            await _context.SaveChangesAsync();

            return RedirectToAction("AllForum");

        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddResponse(int ForumPitanjeId, string Sadrzaj)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Korisnici.FirstOrDefaultAsync(u => u.Identifikator == userId);

            var forumOdgovor = new ForumOdgovor
            {
                Idpitanje = ForumPitanjeId,
                Sadrzaj = Sadrzaj,
                Idkorisnik = user.Id,
                Kreirano = DateOnly.FromDateTime(DateTime.Now)
            };

            _context.ForumOdgovori.Add(forumOdgovor);
            await _context.SaveChangesAsync();

            return RedirectToAction("Odgovori", "Forum", new { id = ForumPitanjeId });
        }
    }
}