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
    public class Receipt : BaseObject
    {
        public Receipt(Session session) : base(session) { }

        private string _receiptNumber;
        [RuleRequiredField]
        [Size(10)]
        public string ReceiptNumber
        {
            get { return _receiptNumber; }
            set { SetPropertyValue(nameof(ReceiptNumber), ref _receiptNumber, value); }
        }

        private DateTime _receiptDate;
        [RuleRequiredField]
        public DateTime ReceiptDate
        {
            get { return _receiptDate; }
            set { SetPropertyValue(nameof(ReceiptDate), ref _receiptDate, value); }
        }

        private string _customerName;
        [Size(50)]
        public string CustomerName
        {
            get { return _customerName; }
            set { SetPropertyValue(nameof(CustomerName), ref _customerName, value); }
        }

        private string _customerAddress;
        [Size(100)]
        public string CustomerAddress
        {
            get { return _customerAddress; }
            set { SetPropertyValue(nameof(CustomerAddress), ref _customerAddress, value); }
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

        [Association("Receipt-ReceiptDetails")]
        public XPCollection<ReceiptDetail> Details
        {
            get { return GetCollection<ReceiptDetail>(nameof(Details)); }
        }

        private decimal CalculateTotal()
        {
            decimal total = 0m;
            foreach (ReceiptDetail detail in Details)
            {
                total += detail.Amount;
            }
            return total;
        }
    }

    public class ReceiptDetail : BaseObject
    {
        public ReceiptDetail(Session session) : base(session) { }

        private Receipt _receipt;
        [Association("Receipt-ReceiptDetails")]
        public Receipt Receipt
        {
            get { return _receipt; }
            set { SetPropertyValue(nameof(Receipt), ref _receipt, value); }
        }

        private string _description;
        [Size(50)]
        public string Description
        {
            get { return _description; }
            set { SetPropertyValue(nameof(Description), ref _description, value); }
        }

        private decimal _amount;
        public decimal Amount
        {
            get { return _amount; }
            set { SetPropertyValue(nameof(Amount), ref _amount, value); }
        }
    }
}
