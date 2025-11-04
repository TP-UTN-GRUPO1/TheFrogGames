using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Game.Request
{
    public class UpdateKeyMetadataGameRequest
    {
        public string? Title {  get; set; }
        public decimal? Price { get; set; } 

        public Boolean? Available { get; set; }

    }
}
