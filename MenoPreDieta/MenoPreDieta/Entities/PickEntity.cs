using SQLite;

namespace MenoPreDieta.Entities
{
    public class PickEntity
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int FirstNameId { get; set; }
        public int SecondNameId { get; set; }
        public int PickedNameId { get; set; }
        public bool IsProcessed { get; set; }
    }
}