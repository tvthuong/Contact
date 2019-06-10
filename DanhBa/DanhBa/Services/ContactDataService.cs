using System;
using System.Linq;
using System.Threading.Tasks;
using DanhBa.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace DanhBa.Services
{
    public class ContactDataService : IDataService
    {
        public ContactDataService()
        {
            DatabaseService = new ContactDatabaseService();
        }
        public ObservableCollection<object> Elements { get; set; }
        public IDatabaseService DatabaseService { get; set; }
        public async void AddNewElement(object obj)
        {
            Contact contact = (Contact)obj;
            do
            {
                contact.Id = Guid.NewGuid().ToString();
            } while (await DatabaseService.IsIdExist(contact.Id));
            DatabaseService.AddNewElement(contact);
            Elements.Add(contact);
        }
        public void DeleteElement(object obj)
        {
            Contact contact = (Contact)obj;
            DatabaseService.DeleteElement(contact);
            Elements.Remove(contact);
        }
        public Task<object> GetElementById(string id)
        {
            return Task.FromResult(Elements.FirstOrDefault(contact => ((Contact)contact).Id == id));
        }
        public async Task<ObservableCollection<GroupObservableCollection>> GetAllElements()
        {
            if (Elements == null)
                Elements = await DatabaseService.GetAllElements();
            return await Task.FromResult(Grouping(Elements));
        }
        public async void SaveElement(object obj)
        {
            Contact contact = (Contact)obj;
            Contact old = (Contact) await GetElementById(contact.Id);
            DatabaseService.SaveElement(contact);
            Elements[Elements.IndexOf(old)] = contact;
        }
        public Task<ObservableCollection<GroupObservableCollection>> FilterElements(string filter)
        {
            return Task.FromResult(Grouping(new ObservableCollection<object>(Elements.Where(contact => ((Contact)contact).FullName.ToLower().Contains(filter.ToLower()) || ((Contact)contact).Phone.Contains(filter)))));
        }
        private ObservableCollection<GroupObservableCollection> Grouping(ObservableCollection<object> collection)
        {
            ObservableCollection<GroupObservableCollection> result = new ObservableCollection<GroupObservableCollection>();
            IEnumerable<object> grouplist = null;
            grouplist = collection.Where(contact => !Regex.IsMatch(((Contact)contact).FullName.ToCharArray()[0].ToString(), "^[a-zA-Z0-9]$")).OrderBy(contact => ((Contact)contact).FullName);
            if (grouplist.Count() > 0)
                result.Add(new GroupObservableCollection(grouplist));
            grouplist = collection.Where(contact => ((Contact)contact).FullName.ToCharArray()[0] >= '0' && ((Contact)contact).FullName.ToCharArray()[0] <= '9').OrderBy(contact => ((Contact)contact).FullName);
            if (grouplist.Count() > 0)
                result.Add(new GroupObservableCollection(grouplist) { Heading = "0 - 9" });
            for (char i = 'A'; i <= 'Z'; i++)
            {
                grouplist = collection.Where(contact => ((Contact)contact).FullName.ToLower().StartsWith(i.ToString().ToLower())).OrderBy(contact => ((Contact)contact).FullName);
                if (grouplist.Count() > 0)
                    result.Add(new GroupObservableCollection(grouplist) { Heading = i.ToString() });
            }
            return result;
        }
    }
}