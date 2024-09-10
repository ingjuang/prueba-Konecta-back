using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Responses
{
    public class PetitionResponse
    {
        public bool success { get; set; }
        public string message { get; set; }
        public object result { get; set; }
    }
}
