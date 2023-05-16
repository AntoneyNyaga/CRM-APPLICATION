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
    [ImageName("BO_invoice")]
    
    public class Invoice : BaseObject
    {
        // https://docs.devexpress.com/CodeRushForRoslyn/118557
        public Invoice(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        public string InvoiceNumber { get; set; }= "INV-" + DateTime.Now.ToString("yyyyMMddHHmmss");
        public DateTime InvoiceDate { get; set; } = DateTime.Now;
        public Account BilledTo { get; set; }
        public DateTime DueDate { get; set; } = DateTime.Now.AddDays(30);
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Total { get; set; }
    }
}