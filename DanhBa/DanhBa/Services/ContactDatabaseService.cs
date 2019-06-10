using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using DanhBa.Models;
using SQLite;
using System.Linq;
namespace DanhBa.Services
{
    class ContactDatabaseService : IDatabaseService
    {
        public SQLiteAsyncConnection Connection { get; set; }
        public ContactDatabaseService()
        {
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "MyContact.db3");
            Connection = new SQLiteAsyncConnection(path);
            Connection.CreateTableAsync<Contact>().Wait();
        }
        public void AddNewElement(object obj)
        {
            Connection.InsertAsync(obj);
        }
        public void DeleteElement(object obj)
        {
            Connection.DeleteAsync(obj);
        }
        public void SaveElement(object obj)
        {
            Connection.UpdateAsync(obj);
        }
        public async Task<ObservableCollection<object>> GetAllElements()
        {
            IEnumerable<Contact> list = await Connection.Table<Contact>().ToListAsync();
            return await Task.FromResult(new ObservableCollection<object>(list));
        }
        public async Task<bool> IsIdExist(string id)
        {
            int count = await Connection.Table<Contact>().Where(contact => contact.Id == id).CountAsync();
            return await Task.FromResult(count > 0 ? true : false);
        }
    }
}