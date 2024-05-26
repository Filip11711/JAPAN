using System.Diagnostics;
using System.Security.Claims;
using JAPAN.Data;
using JAPAN.Data.Entities;
using JAPAN.Models;
using JAPAN.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JAPAN.Controllers
{
    public class HomeController : Controller
    {
        private readonly JapanContext _context;
        private static Ispit? _ispit;

        public HomeController(JapanContext context)
        {
            _context = context;
            if (_ispit == null)
            {
                _ispit = new Ispit
                {
                    Id = 1,
                    Naziv = "Inicijalni ispit",
                    Opis = "Ovo je inicijalni ispit za testiranje prethodne razine znanja te prijedloga težine.",
                    Pozicija = 1,
                    Idtezina = 1,
                    Tezina = new Tezina { Id = 1, Naziv = "Početnik" },
                    Pitanja = new List<Pitanje>()
                };
                InitializeIspit();
            }
        }

        private void InitializeIspit()
        {
            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 1,
                Tekst = "Koje slovo predstavlja 'あ' u Hiragani abecedi?",
                Odgovori = new List<Odgovor>
                {
                    new Odgovor { Id = 1, Tekst = "a", Tocno = 1 },
                    new Odgovor { Id = 2, Tekst = "i", Tocno = 0 },
                    new Odgovor { Id = 3, Tekst = "u", Tocno = 0 },
                    new Odgovor { Id = 4, Tekst = "e", Tocno = 0 }
                }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 2,
                Tekst = "Kako se piše 'サ' u Katakana abecedi?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "shi", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "su", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "sa", Tocno = 1 },
                new Odgovor { Id = 4, Tekst = "se", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 3,
                Tekst = "Što znači kanji znak '日'?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "mjesec", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "kiša", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "vjetar", Tocno = 0 },
                new Odgovor { Id = 4, Tekst = "sunce ili dan", Tocno = 1 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 4,
                Tekst = "Kako se piše broj 5 na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "六", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "七", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "四", Tocno = 0 },
                new Odgovor { Id = 4, Tekst = "五", Tocno = 1 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {   
                Id = 5,
                Tekst = "Kako se kaže 'dobro jutro' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "おはようございます", Tocno = 1 },
                new Odgovor { Id = 2, Tekst = "こんにちは", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "こんばんは", Tocno = 0 },
                new Odgovor { Id = 4, Tekst = "さようなら", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 6,
                Tekst = "Kako se kaže 'plava boja' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "赤色", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "青色", Tocno = 1 },
                new Odgovor { Id = 3, Tekst = "黄色", Tocno = 0 },
                new Odgovor { Id = 4, Tekst = "白色", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 7,
                Tekst = "Kako se kaže 'jesti' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "飲む", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "食べる", Tocno = 1 },
                new Odgovor { Id = 3, Tekst = "見る", Tocno = 0 },
                new Odgovor { Id = 4, Tekst = "話す", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 8,
                Tekst = "Kako se kaže 'mačka' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "犬", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "魚", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "猫", Tocno = 1 },
                new Odgovor { Id = 4, Tekst = "鳥", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 9,
                Tekst = "Kako se kaže 'jabuka' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "バナナ", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "みかん", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "ぶどう", Tocno = 0 },
                new Odgovor { Id = 4, Tekst = "りんご", Tocno = 1 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 10,
                Tekst = "Kako se kaže 'kiša' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "雨", Tocno = 1 },
                new Odgovor { Id = 2, Tekst = "雪", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "晴れ", Tocno = 0 },
                new Odgovor { Id = 4, Tekst = "曇り", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 11,
                Tekst = "Kako se kaže 'mjesec' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "日", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "年", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "月", Tocno = 1 },
                new Odgovor { Id = 4, Tekst = "時", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 12,
                Tekst = "Kako se kaže 'sjever' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "北", Tocno = 1 },
                new Odgovor { Id = 2, Tekst = "南", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "東", Tocno = 0 },
                new Odgovor { Id = 4, Tekst = "西", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 13,
                Tekst = "Kako se tradicionalno zove japanska odjeća?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "yukata", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "hakama", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "kimono", Tocno = 1 },
                new Odgovor { Id = 4, Tekst = "obi", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 14,
                Tekst = "Kako se kaže '2024.' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "二千二十五年", Tocno = 0 },
                new Odgovor { Id = 2, Tekst = "二千二十三年", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "二千二十四年", Tocno = 1 },
                new Odgovor { Id = 4, Tekst = "二千二十六年", Tocno = 0 }
            }
            });

            _ispit.Pitanja.Add(new Pitanje
            {
                Id = 15,
                Tekst = "Kako se kaže 'avion' na japanskom?",
                Odgovori = new List<Odgovor>
            {
                new Odgovor { Id = 1, Tekst = "飛行機", Tocno = 1 },
                new Odgovor { Id = 2, Tekst = "自動車", Tocno = 0 },
                new Odgovor { Id = 3, Tekst = "電車", Tocno = 0 },
                new Odgovor { Id = 4, Tekst = "船", Tocno = 0 }
            }
            });
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Korisnik")]
        [Route("InicijalniIspit")]
        public async Task<IActionResult> InicijalniIspit()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Korisnici.Include(u => u.Statistike)
                                               .FirstOrDefaultAsync(u => u.Identifikator == userId);

            var viewModel = new KorisnikIspitViewModel
            {
                Korisnik = user,
                Ispit = _ispit
            };

            return View(viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Korisnik")]
        [Route("InicijalniIspit/Zavrsi")]
        public async Task<IActionResult> ZavrsiInicijalniIspit(KorisnikIspitOdgovoriViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _context.Korisnici.FirstOrDefaultAsync(u => u.Id == model.UserId);
            var ispit = _ispit;

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

            string rezultat = correct.ToString() + "/" + total.ToString();
            double postotak = correct / total;
            string preporuka;

            if (postotak < 0.5)
            {
                user.Preporuka = "Početnik";
                preporuka = "Početnik";
            }
            else if (postotak < 0.8)
            {
                user.Preporuka = "Srednje znanje";
                preporuka = "Srednje znanje";
            }
            else
            {
                user.Preporuka = "Napredno";
                preporuka = "Napredno";
            }

            _context.Korisnici.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("RezultatIspita", "Home", new { rezultat = rezultat, preporuka = preporuka });
        }

        [Authorize]
        [Route("RezultatIspita")]
        public IActionResult RezultatIspita(string rezultat, string preporuka)
        {
            ViewBag.Rezultat = rezultat;
            ViewBag.Preporuka = preporuka;
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}