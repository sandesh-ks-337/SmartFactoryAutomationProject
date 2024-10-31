using System.Threading.Tasks;

namespace SmartFactory_DataConsistencyCheck.Services
{
    public interface IDataConsistencyService
    {
        Task<bool> CheckOrderDataConsistency();
        Task<bool> CheckProductDataConsistency();
    }
}
