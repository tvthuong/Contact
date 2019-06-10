using System.Threading.Tasks;
using System.Collections.ObjectModel;
namespace DanhBa.Services
{
    public interface IDataService
    {
        IDatabaseService DatabaseService { get; set; }
        ObservableCollection<object> Elements { get; set; }
        Task<ObservableCollection<GroupObservableCollection>> GetAllElements();
        Task<ObservableCollection<GroupObservableCollection>> FilterElements(string filter);
        void AddNewElement(object obj);
        void DeleteElement(object obj);
        Task<object> GetElementById(string id);
        void SaveElement(object obj);
    }
}