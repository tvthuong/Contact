using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace DanhBa.Services
{
    public interface IDataService<T,U>
    {
        IDatabaseService<T> DatabaseService { get; set; }
        ObservableCollection<T> Elements { get; set; }
        Task<ObservableCollection<U>> GetAllElements();
        Task<ObservableCollection<U>> FilterElements(string filter);
        void AddNewElement(T obj);
        void DeleteElement(T obj);
        Task<T> GetElementById(string id);
        void SaveElement(T obj);
    }
}