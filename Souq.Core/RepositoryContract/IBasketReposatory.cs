using Souq.Core.Entites.Basket;

namespace Souq.Core.RepositoryContract
{
    public interface IBasketReposatory
    {

        Task<CustomerBasket?> GetBasket(string BasketId);

        Task<CustomerBasket?> UpdateBasket(CustomerBasket Basket);

        Task<bool> DeleteBasket(string BasketId);

    }
}
