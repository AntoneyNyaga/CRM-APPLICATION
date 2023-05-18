using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Order Management")]
    [ImageName("MainMenuItem")]

    public class Bill
    {
        public string BillNumber { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Amount { get; set; }
        // Add other properties as needed

        public void SaveBill()
        {
            // Perform the logic to save the bill into the system
            // This could involve interacting with a database or any other data storage mechanism
            // Implement the necessary code to persist the bill details
        }
    }
}

