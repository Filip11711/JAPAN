using System.Collections;
using System.ComponentModel.DataAnnotations;
using JAPAN.Data.Entities;

namespace JAPAN.ViewModels
{
    public class NovoPitanjeViewModel
    {
        public Pitanje Pitanje { get; set; } = new Pitanje();

        [AtLeastOneElement]
        public List<Odgovor> Odgovori { get; set; } = [new Odgovor()];
    }

    public class AtLeastOneElementAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is IList list && list.Count > 0)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("The list must contain at least one element.");
        }
    }
}
