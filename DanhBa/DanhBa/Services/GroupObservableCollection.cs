using System.Collections.Generic;
using System.Collections.ObjectModel;
namespace DanhBa.Services
{
    public class GroupObservableCollection: ObservableCollection<object>
    {
        public GroupObservableCollection(IEnumerable<object> collection) : base(collection)
        {
            Heading = "";
        }
        public string Heading { get; set; }
    }
}