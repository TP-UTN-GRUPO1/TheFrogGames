using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.MercadoPago.Response
{
    public class MPCheckoutPayload
    {
        public int OrderId { get; set; }
        public string PayerEmail { get; set; } = string.Empty;
        public List<MPItem> Items { get; set; } = new();
    }
}
