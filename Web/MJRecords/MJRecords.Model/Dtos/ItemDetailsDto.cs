using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MJRecords.Model
{
    public class ItemDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }

        public string Description { get; set; }
        public decimal Price { get; set; }

        public string Justification { get; set; }
        public string PurchaseLocation { get; set; }

        public string Status { get; set; }

        public decimal ItemSubtotal { get; set; }
        public decimal ItemTaxTotal { get; set; }
        public decimal ItemGrandTotal { get; set; }
    }

}
