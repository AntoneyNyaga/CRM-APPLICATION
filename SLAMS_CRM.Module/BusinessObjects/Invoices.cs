using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;

namespace SLAMS_CRM.Module.BusinessObjects.OrderManagement
{
    [DefaultClassOptions]
    [DefaultProperty("InvoiceNumber")]
    [ImageName("BO_Invoice")]
    [NavigationItem("Orders")]

    public class Invoice : BaseObject
    {
        public Invoice(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            InvoiceDate = DateTime.Now;
        }


        [Association("Account-Invoices")]
        public Account Account { get => account; set => SetPropertyValue(nameof(Account), ref account, value); }

        Account account;
        DateTime invoiceDate;
        string invoiceNumber;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        [VisibleInDetailView(false)]
        //[ReadOnly(true)]
        public string InvoiceNumber
        {
            get { return invoiceNumber; }
            set { SetPropertyValue(nameof(InvoiceNumber), ref invoiceNumber, value); }
        }

        public DateTime InvoiceDate
        {
            get { return invoiceDate; }
            set { SetPropertyValue(nameof(InvoiceDate), ref invoiceDate, value); }
        }

        [Association("Invoice-Products")]
        public XPCollection<Product> Products { get { return GetCollection<Product>(nameof(Products)); } }

        [Browsable(false)]
        [Association("Invoice-PurchaseOrders")]
        public XPCollection<PurchaseOrder> PurchaseOrders
        {
            get
            {
                return GetCollection<PurchaseOrder>(nameof(PurchaseOrders));
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (Session.IsNewObject(this))
            {
                GenerateInvoiceNumber();
            }
        }

        private void GenerateInvoiceNumber()
        {
            const string InvoiceNumberFormat = "INV{0}{1}{2:0000}";
            var lastInvoice = Session.Query<Invoice>()?.OrderByDescending(i => i.InvoiceDate).FirstOrDefault();
            if (lastInvoice != null)
            {
                var year = lastInvoice.InvoiceDate.Year;
                var month = lastInvoice.InvoiceDate.Month;
                var sequence = int.Parse(lastInvoice.InvoiceNumber[7..]);
                sequence++;
                var newInvoiceNumber = string.Format(InvoiceNumberFormat, year, month, sequence);
                InvoiceNumber = newInvoiceNumber;
            }
            else
            {
                InvoiceNumber = string.Format(InvoiceNumberFormat, DateTime.Today.Year, DateTime.Today.Month, 1);
            }
        }
    }

    internal class PurchaseOrder
    {
    }
}
