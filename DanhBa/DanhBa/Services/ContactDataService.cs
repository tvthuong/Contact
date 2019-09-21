using System;
using System.Linq;
using System.Threading.Tasks;
using DanhBa.Models;
using System.Collections.ObjectModel;

namespace DanhBa.Services
{
    public class ContactDataService : IDataService<Contact, IGrouping<string, Contact>>
    {
        public ContactDataService()
        {
            DatabaseService = new ContactDatabaseService();
        }
        public ObservableCollection<Contact> Elements { get; set; }
        public IDatabaseService<Contact> DatabaseService { get; set; }
        public async void AddNewElement(Contact obj)
        {
            Contact contact = (Contact)obj;
            do
            {
                contact.Id = Guid.NewGuid().ToString();
            } while (await DatabaseService.IsIdExist(contact.Id));
            DatabaseService.AddNewElement(contact);
            Elements.Add(contact);
        }
        public void DeleteElement(Contact obj)
        {
            Contact contact = (Contact)obj;
            DatabaseService.DeleteElement(contact);
            Elements.Remove(contact);
        }
        public Task<Contact> GetElementById(string id)
        {
            return Task.FromResult(Elements.FirstOrDefault(contact => ((Contact)contact).Id == id));
        }
        public async Task<ObservableCollection<IGrouping<string, Contact>>> GetAllElements()
        {
            if (Elements == null)
                Elements = await DatabaseService.GetAllElements();
            return await Task.FromResult(Grouping(Elements));
        }
        public async void SaveElement(Contact obj)
        {
            Contact contact = (Contact)obj;
            Contact old = (Contact) await GetElementById(contact.Id);
            DatabaseService.SaveElement(contact);
            Elements[Elements.IndexOf(old)] = contact;
        }
        public Task<ObservableCollection<IGrouping<string, Contact>>> FilterElements(string filter)
        {
            return Task.FromResult(Grouping(new ObservableCollection<Contact>(Elements.Where(contact => ((Contact)contact).FullName.ToLower().Contains(filter.ToLower()) || ((Contact)contact).Phone.Contains(filter)))));
        }
        private ObservableCollection<IGrouping<string, Contact>> Grouping(ObservableCollection<Contact> collection)
        {
            return new ObservableCollection<IGrouping<string, Contact>>(collection.OrderBy(contact => ((Contact)contact).FullName).GroupBy(contact => contact.FullName.ToCharArray()[0].ToString()));
        }
    }
}