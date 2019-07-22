using System.Collections.Generic;
using System.Linq;
using MenoPreDieta.Entities;

namespace MenoPreDieta.Core
{
    public class Picks : List<PickEntity>
    {
        public PickEntity Selected { get; set; }

        public List<PickEntity> NotProcessed => 
            this.Where(pair => !pair.IsProcessed).ToList();

        public List<PickEntity> Processed => 
            this.Where(np => np.IsProcessed).ToList();
    }
}