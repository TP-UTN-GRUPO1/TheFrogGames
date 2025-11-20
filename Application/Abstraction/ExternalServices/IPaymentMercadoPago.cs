using Contracts.MercadoPago.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Abstraction.ExternalServices
{
    public interface IPaymentMercadoPago
    {
        Task<CheckoutResponse> CreateCheckoutAsync(MPCheckoutPayload payload);
    }
}
