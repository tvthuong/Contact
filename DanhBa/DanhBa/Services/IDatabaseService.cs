using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SQLite;
namespace DanhBa.Services
{
    public interface IDatabaseService
    {
        SQLiteAsyncConnection Connection { set; get; }
        void AddNewElement(object obj);
        void DeleteElement(object obj);
        void SaveElement(object obj);
        Task<ObservableCollection<object>> GetAllElements();
        Task<bool> IsIdExist(string id);
    }
}