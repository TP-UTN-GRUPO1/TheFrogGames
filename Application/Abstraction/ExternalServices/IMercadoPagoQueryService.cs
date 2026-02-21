using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.MercadoPago.Response;


namespace Application.Abstraction.ExternalServices
{
    public interface IMercadoPagoQueryService
    {
        Task<PaymentDetailResponse> GetPaymentAsync(string paymentId);
    }

}
