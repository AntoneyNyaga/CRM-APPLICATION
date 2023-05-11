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
    [ImageName("BO_Invoice")]

    public class SalesOrder : BaseObject
    {
        public SalesOrder(Session session) : base(session) { }

        private string _salesOrderNumber;
        [RuleRequiredField]
        [Size(10)]
        public string SalesOrderNumber
        {
            get { return _salesOrderNumber; }
            set { SetPropertyValue(nameof(SalesOrderNumber), ref _salesOrderNumber, value); }
        }

        private DateTime _orderDate;
        [RuleRequiredField]
        public DateTime OrderDate
        {
            get { return _orderDate; }
            set { SetPropertyValue(nameof(OrderDate), ref _orderDate, value); }
        }
    }
}