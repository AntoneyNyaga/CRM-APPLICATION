using DevExpress.DashboardCommon;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace SLAMS_CRM.Module.BusinessObjects
{
    //[DefaultClassOptions]
    //[NavigationItem("Inbox")]
    [Persistent("Communication")]
    [ImageName("Actions_EnvelopeOpen")]
    public class Communication : BaseObject
    {
        public Communication(Session session) : base(session) { }

        private DateTime _dateTime;
        [ModelDefault("DisplayFormat", "{0:G}")]
        [ModelDefault("EditMask", "G")]
        [ModelDefault("AllowEdit", "false")]
        public DateTime DateTime
        {
            get { return _dateTime; }
            set { SetPropertyValue(nameof(DateTime), ref _dateTime, value); }
        }

        private CommunicationType _type;
        public CommunicationType Type
        {
            get { return _type; }
            set { SetPropertyValue(nameof(Type), ref _type, value); }
        }

        private Contact _contact;
        [Association("Contact-Communications")]
        public Contact Contact
        {
            get { return _contact; }
            set { SetPropertyValue(nameof(Contact), ref _contact, value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Subject
        {
            get { return GetPropertyValue<string>(nameof(Subject)); }
            set { SetPropertyValue(nameof(Subject), value); }
        }

        [Size(SizeAttribute.Unlimited)]
        public string Body
        {
            get { return GetPropertyValue<string>(nameof(Body)); }
            set { SetPropertyValue(nameof(Body), value); }
        }
    }

    public enum CommunicationType
    {
        Email,
        Phone,
        Meeting,
        FollowUpTask
    }

}