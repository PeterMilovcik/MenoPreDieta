using SQLite;

namespace MenoPreDieta.Entities
{
    public class BoyNameEntity : INameEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Value { get; set; }
    }
}