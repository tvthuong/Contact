using SQLite;

namespace DanhBa.Business.Models
{
    public class BaseEntity
    {
        [PrimaryKey]
        [AutoIncrement]
        public int Id { set; get; }
    }
}
