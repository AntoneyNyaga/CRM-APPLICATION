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
    public class CreditNote : BaseObject
    {
        public CreditNote(Session session) : base(session) { }

        private string _creditNoteNumber;
        [RuleRequiredField]
        [Size(10)]
        public string CreditNoteNumber
        {
            get { return _creditNoteNumber; }
            set { SetPropertyValue(nameof(CreditNoteNumber), ref _creditNoteNumber, value); }
        }

        private DateTime _creditNoteDate;
        [RuleRequiredField]
        public DateTime CreditNoteDate
        {
            get { return _creditNoteDate; }
            set { SetPropertyValue(nameof(CreditNoteDate), ref _creditNoteDate, value); }
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

        [Association("CreditNote-CreditNoteDetails")]
        public XPCollection<CreditNoteDetail> Details
        {
            get { return GetCollection<CreditNoteDetail>(nameof(Details)); }
        }

        private decimal CalculateTotal()
        {
            decimal total = 0m;
            foreach (CreditNoteDetail detail in Details)
            {
                total += detail.Amount;
            }
            return total;
        }
    }

    public class CreditNoteDetail : BaseObject
    {
        public CreditNoteDetail(Session session) : base(session) { }

        private CreditNote _creditNote;
        [Association("CreditNote-CreditNoteDetails")]
        public CreditNote CreditNote
        {
            get { return _creditNote; }
            set { SetPropertyValue(nameof(CreditNote), ref _creditNote, value); }
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
