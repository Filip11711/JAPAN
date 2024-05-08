﻿using Auth0.AspNetCore.Authentication;
using System.Security.Claims;
using JAPAN.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using JAPAN.Data.Entities;
using JAPAN.ViewModels;

namespace JAPAN.Controllers
{
    public class AccountController : Controller
    {   
        private readonly JapanContext _context;

        public AccountController(JapanContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> LoginCallback()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var ime = User.Claims.FirstOrDefault(c => c.Type == "nickname")?.Value;

            if (userId != null && ime != null)
            {
                var user = await _context.Korisnici.FirstOrDefaultAsync(u => u.Identifikator == userId);
                var Role = await _context.Uloge.FirstOrDefaultAsync(u => u.Naziv == "Korisnik");
                if (user == null && Role != null)
                {
                    user = new Korisnik
                    {
                        Identifikator = userId,
                        Korisnickoime = ime,
                        Iduloga = Role.Id,
                        Uloga = Role
                    };
                    _context.Korisnici.Add(user);
                    await _context.SaveChangesAsync();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task Login(string returnUrl = "/Account/LoginCallback")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in 
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        [Authorize]
        public async Task<IActionResult> Profile()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            var user = await _context.Korisnici.Include(u => u.Statistike)
                                               .ThenInclude(v => v.Tecaj)
                                               .Include(u => u.Statistike)
                                               .ThenInclude(v => v.Ispit)
                                               .Include(u => u.Uloga)
                                               .FirstOrDefaultAsync(u => u.Identifikator == userId);

            
            List<Statistika> statistike_tecaja = user.Statistike.Where(s => s.Idispit == null).ToList();
            statistike_tecaja.OrderBy(v => v.Tecaj.Pozicija);
            List<Statistika> statistike_ispita = user.Statistike.Where(s => s.Idtecaj == null).ToList();
            statistike_tecaja.OrderBy(v => v.Ispit.Pozicija);

            return View(new UserProfileViewModel
            {
                Identifikator = userId,
                Korisnickoime = user.Korisnickoime,
                Uloga = user.Uloga.Naziv,
                Statistike_tecaja = statistike_tecaja,
                Statistike_ispita = statistike_ispita
            });
        }

    }
}
