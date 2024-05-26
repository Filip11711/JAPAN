using JAPAN.Data;
using JAPAN.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        // GET: Forum/AllForum
        public async Task<IActionResult> AllForum()
        {
            var forumQuestions = await _context.ForumPitanja.Include(fp => fp.Korisnik).ToListAsync();
            return View(forumQuestions);
        }

        // GET: Forum/Odgovori/5
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

        // GET: Forum/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Forum/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Naslov,Sadrzaj,Idkorisnik")] ForumPitanje forumPitanje)
        {
            if (ModelState.IsValid)
            {
                forumPitanje.Kreirano = DateOnly.FromDateTime(DateTime.Now);
                _context.Add(forumPitanje);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(AllForum));
            }
            return View(forumPitanje);
        }
    }
}
