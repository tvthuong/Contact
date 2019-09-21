using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SQLite;
namespace DanhBa.Services
{
    public interface IDatabaseService<T>
    {
        SQLiteAsyncConnection Connection { set; get; }
        void AddNewElement(T obj);
        void DeleteElement(T obj);
        void SaveElement(T obj);
        Task<ObservableCollection<T>> GetAllElements();
        Task<bool> IsIdExist(string id);
    }
}