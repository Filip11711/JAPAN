﻿using Microsoft.AspNetCore.Mvc.Rendering;

namespace JAPAN.ViewModels
{
    public class UrediTecajViewModel
    {
        public int Id { get; set; }
        public string Naziv { get; set; } = null!;
        public string Opis { get; set; } = null!;
        public string Sadrzaj { get; set; } = null!;
        public int TezinaId { get; set; }
        public int TipSadrzajaId { get; set; }
    }
}
