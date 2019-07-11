using SQLite;

namespace MenoPreDieta.Entities
{
    public class NameEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Value { get; set; }

        public Gender Gender { get; set; }
    }
}