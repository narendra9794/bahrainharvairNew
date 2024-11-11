using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuthaganModel.Setting
{
  public class DateFormateSetttingModel
  {
    public Guid _id { get; set; }
    [Required(ErrorMessage = "SmallDateFormat is required.")]
    [Display(Name = "smallDateFormat")]
    public string smallDateFormat { get; set; }
    [Required(ErrorMessage = "DateFormatWithTime is required.")]
    [Display(Name = "dateFormatWithTime")]
    public string dateFormatWithTime { get; set; }
    [Required(ErrorMessage = "LongDateFormat is required.")]
    [Display(Name = "longDateFormat")]
    public string longDateFormat { get; set; }
    [Required(ErrorMessage = "LongDateFormatWithTime is required.")]
    [Display(Name = "longDateFormatWithTime")]
    public string longDateFormatWithTime { get; set; }
  }
}
