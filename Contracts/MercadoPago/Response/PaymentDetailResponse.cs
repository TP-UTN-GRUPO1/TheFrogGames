using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.MercadoPago.Response
{
    public class PaymentDetailResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public decimal TransactionAmount { get; set; }
        public string ExternalReference { get; set; } = string.Empty;
    }
}
