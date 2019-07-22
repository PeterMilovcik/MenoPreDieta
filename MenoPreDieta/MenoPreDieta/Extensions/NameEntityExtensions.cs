using MenoPreDieta.Entities;
using MenoPreDieta.Models;

namespace MenoPreDieta.Extensions
{
    public static class NameEntityExtensions
    {
        public static NameModel ToNameModel(this NameEntity entity) => new NameModel(entity.Id, entity.Value);
    }
}