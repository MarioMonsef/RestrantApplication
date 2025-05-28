using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.ViewModels.Review
{
    public class AddReveiwViewModel
    {
        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
    }
}
