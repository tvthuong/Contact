using DanhBa.Business.Models;
namespace DanhBa.Models
{
    public class Contact
    {
        public ContactEntity Entity { get; }
        public int Id => Entity.Id;
        public string FirstName => Entity.FirstName;
        public string LastName => Entity.LastName;
        public string Company => Entity.Company;
        public string JobTitle => Entity.JobTitle;
        public string Email => Entity.Email;
        public string Phone => Entity.Phone;
        public string Street => Entity.Street;
        public string City => Entity.City;
        public string PostalCode => Entity.PostalCode;
        public string State => Entity.State;
        public string PhotoUrl => Entity.PhotoUrl;
        public bool HasPhoneNumber => !string.IsNullOrEmpty(Phone);
        public bool HasEmailAddress => !string.IsNullOrEmpty(Email);
        public bool HasAddress => !string.IsNullOrEmpty(Street) || !string.IsNullOrEmpty(State);
        public string StatePostal => State + (!string.IsNullOrEmpty(State) && !string.IsNullOrEmpty(PostalCode) ? " - " : "") + (!string.IsNullOrEmpty(PostalCode) ? "(" : "") + PostalCode + (!string.IsNullOrEmpty(PostalCode) ? ")" : "");
        public string FullName => FirstName.Trim() + (!string.IsNullOrEmpty(FirstName.Trim()) && !string.IsNullOrEmpty(LastName.Trim()) ? " " : "") + LastName.Trim();
        public string ShortName => FullName[0].ToString();

        public Contact(ContactEntity entity = null)
        {
            Entity = entity ?? new ContactEntity();
        }

        public bool IsContainsText(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                return true;
            }
            text = text.Trim();
            return FullName.ToLower().Contains(text.ToLower()) || Phone.Contains(text);
        }
    }
}