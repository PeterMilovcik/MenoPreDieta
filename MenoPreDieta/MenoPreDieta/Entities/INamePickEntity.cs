namespace MenoPreDieta.Entities
{
    public interface INamePickEntity
    {
        int Id { get; set; }
        int FirstNameId { get; set; }
        int SecondNameId { get; set; }
        int PickedNameId { get; set; }
        bool IsNamePicked { get; set; }
    }
}