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
    public class Quotation : BaseObject
    {
        public Quotation(Session session) : base(session) { }

        private string _quotationNumber;
        [RuleRequiredField]
        [Size(10)]
        public string QuotationNumber
        {
            get { return _quotationNumber; }
            set { SetPropertyValue(nameof(QuotationNumber), ref _quotationNumber, value); }
        }

        private DateTime _quotationDate;
        [RuleRequiredField]
        public DateTime QuotationDate
        {
            get { return _quotationDate; }
            set { SetPropertyValue(nameof(QuotationDate), ref _quotationDate, value); }
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

        [Association("Quotation-QuotationDetails")]
        public XPCollection<QuotationDetail> Details
        {
            get { return GetCollection<QuotationDetail>(nameof(Details)); }
        }

        private decimal CalculateTotal()
        {
            decimal total = 0m;
            foreach (QuotationDetail detail in Details)
            {
                total += detail.LineTotal;
            }
            return total;
        }
    }

    public class QuotationDetail : BaseObject
    {
        public QuotationDetail(Session session) : base(session) { }

        private Quotation _quotation;
        [Association("Quotation-QuotationDetails")]
        public Quotation Quotation
        {
            get { return _quotation; }
            set { SetPropertyValue(nameof(Quotation), ref _quotation, value); }
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
