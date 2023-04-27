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

        private decimal _totalAmount;
        [PersistentAlias(nameof(CalculateTotalAmount))]
        public decimal TotalAmount
        {
            get { return _totalAmount; }
        }
        private decimal CalculateTotalAmount
        {
            get { return CalculateTotal(); }
        }

        [Association("SalesOrder-SalesOrderDetails")]
        public XPCollection<SalesOrderDetail> Details
        {
            get { return GetCollection<SalesOrderDetail>(nameof(Details)); }
        }

        private decimal CalculateTotal()
        {
            decimal total = 0m;
            foreach (SalesOrderDetail detail in Details)
            {
                total += detail.LineTotal;
            }
            return total;
        }
    }

    public class SalesOrderDetail : BaseObject
    {
        public SalesOrderDetail(Session session) : base(session) { }

        private SalesOrder _salesOrder;
        [Association("SalesOrder-SalesOrderDetails")]
        public SalesOrder SalesOrder
        {
            get { return _salesOrder; }
            set { SetPropertyValue(nameof(SalesOrder), ref _salesOrder, value); }
        }

        private string _productCode;
        [Size(10)]
        public string ProductCode
        {
            get { return _productCode; }
            set { SetPropertyValue(nameof(ProductCode), ref _productCode, value); }
        }

        private decimal _unitPrice;
        public decimal UnitPrice
        {
            get { return _unitPrice; }
            set { SetPropertyValue(nameof(UnitPrice), ref _unitPrice, value); }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set { SetPropertyValue(nameof(Quantity), ref _quantity, value); }
        }

        private decimal _lineTotal;
        [PersistentAlias(nameof(CalculateLineTotal))]
        public decimal LineTotal
        {
            get { return _lineTotal; }
        }
        private decimal CalculateLineTotal
        {
            get { return UnitPrice * Quantity; }
        }
    }
}
