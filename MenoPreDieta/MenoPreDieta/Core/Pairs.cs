using System.Collections.Generic;
using System.Linq;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class Pairs : List<INamePickEntity>
    {
        public INamePickEntity Selected { get; set; }

        public List<INamePickEntity> With(int nameId) =>
            this.Where(pair => pair.FirstNameId == nameId ||
                               pair.SecondNameId == nameId).ToList();

        public List<INamePickEntity> NotPicked() => 
            this.Where(pair => !pair.IsNamePicked).ToList();

        public List<INamePickEntity> Picked() => 
            this.Where(np => np.IsNamePicked).ToList();
    }
}