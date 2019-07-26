using SQLite;

namespace MenoPreDieta.Entities
{
    public class NameEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Value { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsLiked { get; set; }
        public string NameDay { get; set; }
        public string Description { get; set; }
    }
}