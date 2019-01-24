using System.ComponentModel.DataAnnotations;

namespace AppreciationCards.Models
{
    public enum XeroValueEnum
    {
        [Display(Name = "#Beautiful")]
        Beautiful,

        [Display(Name = "#Champion")]
        Champion,

        [Display(Name = "#Challenge")]
        Challenge,

        [Display(Name = "#Human")]
        Human,

        [Display(Name = "#Ownership")]
        Ownership
    }
}
