using DanhBa.Business.Models;
using SQLite;
using System.Collections.Generic;
using System.IO;

namespace DanhBa.Business
{
    public class ContactBD
    {
        public static ContactBD Instance { get; } = new ContactBD();

        public IEnumerable<ContactEntity> Contacts { get; private set; }

        private SQLiteAsyncConnection _connection;

        public void Initialize(string dbFolder)
        {
            string dbPath = Path.Combine(dbFolder, "Contact.db3");
            _connection = new SQLiteAsyncConnection(dbPath);
            CreateTables();
            GetEntities();
        }

        public void Insert(BaseEntity entity)
        {
            _connection.InsertAsync(entity).Wait();
            GetEntities();
        }

        public void Update(BaseEntity entity)
        {
            _connection.UpdateAsync(entity).Wait();
            GetEntities();
        }

        public void Delete<T>(int primaryKey)
        {
            _connection.DeleteAsync<T>(primaryKey).Wait();
            GetEntities();
        }

        private void GetEntities()
        {
            Contacts = _connection.Table<ContactEntity>().ToListAsync().Result;
        }

        private void CreateTables()
        {
            _connection.CreateTableAsync<ContactEntity>().Wait();
        }

        private ContactBD() { }
    }
}
