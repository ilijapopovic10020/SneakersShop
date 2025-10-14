using SneakersShop.Models;

namespace SneakersShop.Services.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressesModel>> GetAddressesAsync();
        Task<AddressesModel> GetAddressByIdAsync(int id);
        Task<bool> CreateAddressAsync(CreateAddressModel model);
        Task<bool> UpdateAddressAsync(int id, UpdateAddressModel model);
        Task<bool> DeleteAddressAsync(int id);
    }
}
