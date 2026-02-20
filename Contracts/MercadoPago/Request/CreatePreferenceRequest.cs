using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Contracts.MercadoPago.Request
{
    public class CreatePreferenceRequest
    {
        public List<MPItem> Items { get; set; } = new();
    }
}