using System;
using System.Collections.Generic;
using System.Text;

namespace Top4ever.Domain.Transfer
{
    public class ReminderOrder
    {
        public Guid OrderID { get; set; }

        public IList<Guid> OrderDetailsIDList { get; set; }

        public string ReasonName { get; set; }

        public string EmployeeNo { get; set; }
    }
}
