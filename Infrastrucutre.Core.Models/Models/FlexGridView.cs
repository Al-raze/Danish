using System.Collections.Generic;

namespace Infrastructure.Core.Models
{
   public class FlexGridView
    {
        public int page;
        public int total;
        public List<FlexiGridRow> rows = new List<FlexiGridRow>();
    }
}
