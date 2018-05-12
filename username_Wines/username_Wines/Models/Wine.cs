using MedicalOffice.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace username_Wines.Models
{
    public class Wine_Type
    {
        public Wine_Type()
        {
            this.Wines = new HashSet<Wine>();
        }

        public int ID { get; set; }

        [Display(Name = "Type of Wine")]
        [Required(ErrorMessage = "Wine Type cannot be left blank.")]
        [StringLength(50, ErrorMessage = "Name cannot be more than 50 characters long.")]
        public string wtName { get; set; }

        public virtual ICollection<Wine> Wines { get; set; }
        
    }
    public class Wine : Auditable, IValidatableObject
    {
        public int ID { get; set; }

        [Display(Name ="Wine")]
        public string WineSummary
        {
            get {
                return wineName + " - " + wineYear;
            }
        }


        [Display(Name = "Name")]
        [Required(ErrorMessage = "Name cannot be left blank.")]
        [StringLength(70, ErrorMessage = "Name cannot be more than 70 characters long.")]
        [Index("IX_Unique_Wine",1,IsUnique =true)]
        public string wineName { get; set; }

        [Display(Name = "Year")]
        [Required(ErrorMessage = "Year cannot be left blank.")]
        [RegularExpression("^\\d{4}$", ErrorMessage = "The Wine Year number must be exactly 4 numeric digits.")]
        [StringLength(4)]
        [Index("IX_Unique_Wine", 2, IsUnique = true)]
        public string wineYear { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price cannot be left blank.")]
        [DataType(DataType.Currency)]
        public decimal winePrice { get; set; }

        [Display(Name = "Harvest Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? wineHarvest { get; set; }

        public int Wine_TypeID { get; set; }

        public virtual Wine_Type WineType { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if(wineHarvest.HasValue)
            {
                if (wineHarvest.GetValueOrDefault().Year != Convert.ToInt32(wineYear))
                {
                    yield return new ValidationResult("Harvest date must be during the Wine Year.", new[] { "wineHarvest" });
                }
            }
        }
    }
}