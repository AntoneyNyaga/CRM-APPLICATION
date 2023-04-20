using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
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
using System.ComponentModel.DataAnnotations;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base.General;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.ObjectModel;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Clients and Leads")]
    [Persistent("Contact")]
    [ImageName("BO_Person")]


    public class Contact : Person
    {
        public Contact(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }


        string jobTitle;
        Company company;
        //string leadSource;
        Account account;

        [Size(50)]
        public string JobTitle { get => jobTitle; set => SetPropertyValue(nameof(JobTitle), ref jobTitle, value); }


        //[ExpandObjectMembers(ExpandObjectMembers.Never)]
        //[DevExpress.Xpo.Aggregated]
        [RuleRequiredField("RuleRequiredField for Contact.Company", DefaultContexts.Save)]

        public Company Company { get => company; set => SetPropertyValue(nameof(Company), ref company, value); }

        [RuleRequiredField("RuleRequiredField for Contact.Account", DefaultContexts.Save)]
        public Account Account { get => account; set => SetPropertyValue(nameof(Account), ref account, value); }

        public Lead ConvertedFrom { get; set; }


        [Browsable(false)]
        public IList<Quote> Quote { get; set; } = new ObservableCollection<Quote>();

        [Browsable(false)]
        [DevExpress.Xpo.Association("Contact-Communications")]
        public XPCollection<Communication> Communications
        {
            get { return GetCollection<Communication>(nameof(Communications)); }
        }
    }
}
