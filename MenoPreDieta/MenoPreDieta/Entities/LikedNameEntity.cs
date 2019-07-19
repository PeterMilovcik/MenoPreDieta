using SQLite;

namespace MenoPreDieta.Entities
{
    public class LikedNameEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int NameId { get; set; }
    }
}