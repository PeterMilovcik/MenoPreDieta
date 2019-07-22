using System.Collections.Generic;
using System.Linq;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class Picks : List<PickEntity>
    {
        public PickEntity Selected { get; set; }

        public List<PickEntity> With(int nameId) =>
            this.Where(pair => pair.FirstNameId == nameId ||
                               pair.SecondNameId == nameId).ToList();

        public List<PickEntity> NotProcessed => 
            this.Where(pair => !pair.IsProcessed).ToList();

        public List<PickEntity> Processed => 
            this.Where(np => np.IsProcessed).ToList();

        public List<int> NameIds()
        {
            var notRemovedNameIds = new HashSet<int>();
            App.Names.Picks.ForEach(namePick =>
            {
                notRemovedNameIds.Add(namePick.FirstNameId);
                notRemovedNameIds.Add(namePick.SecondNameId);
            });
            return notRemovedNameIds.ToList();
        }
    }
}