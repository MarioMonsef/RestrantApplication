using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestrantApplication.Core.Models
{
    public class BaseModel<T>
    {
        public T ID { get; set; }
    }
}
