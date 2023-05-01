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
    [ImageName("Payment")]
    public class Payment : BaseObject
{
    public Payment(Session session) : base(session) { }

    private decimal amount;
    public decimal Amount
    {
        get { return amount; }
        set { SetPropertyValue(nameof(Amount), ref amount, value); }
    }

    private DateTime paymentDate;
    public DateTime PaymentDate
    {
        get { return paymentDate; }
        set { SetPropertyValue(nameof(PaymentDate), ref paymentDate, value); }
    }

    private Customer customer;
    [Association("Customer-Payments")]
    public Customer Customer
    {
        get { return customer; }
        set { SetPropertyValue(nameof(Customer), ref customer, value); }
    }

    private PaymentType paymentType;
    public PaymentType PaymentType
    {
        get { return paymentType; }
        set { SetPropertyValue(nameof(PaymentType), ref paymentType, value); }
    }

    private string paymentReference;
    public string PaymentReference
    {
        get { return paymentReference; }
        set { SetPropertyValue(nameof(PaymentReference), ref paymentReference, value); }
    }
}

public enum PaymentType
{
    Cash,
    CreditCard,
    DebitCard,
    BankTransfer,
    Cheque,
    Other
}
    }

