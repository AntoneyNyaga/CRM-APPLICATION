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
    [NavigationItem("Inbox")]
    [DefaultProperty("Title")]
    [ImageName("BO_Scheduler")]
    [CreatableItem(false)]
    [DevExpress.ExpressApp.DC.XafDefaultProperty(nameof(Title))]
    public class CalendarEvent : BaseObject
    {
        public CalendarEvent(Session session) : base(session) { }

        private string title;
        public string Title
        {
            get { return title; }
            set { SetPropertyValue(nameof(Title), ref title, value); }
        }

        private DateTime start;
        public DateTime Start
        {
            get { return start; }
            set { SetPropertyValue(nameof(Start), ref start, value); }
        }

        private DateTime end;
        public DateTime End
        {
            get { return end; }
            set { SetPropertyValue(nameof(End), ref end, value); }
        }

        private bool allDayEvent;
        public bool AllDayEvent
        {
            get { return allDayEvent; }
            set { SetPropertyValue(nameof(AllDayEvent), ref allDayEvent, value); }
        }

        private string location;
        public string Location
        {
            get { return location; }
            set { SetPropertyValue(nameof(Location), ref location, value); }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { SetPropertyValue(nameof(Description), ref description, value); }
        }

        private CalendarEventType eventType;
        public CalendarEventType EventType
        {
            get { return eventType; }
            set { SetPropertyValue(nameof(EventType), ref eventType, value); }
        }
    }

    public enum CalendarEventType
    {
        Appointment,
        Meeting,
        Reminder,
        Pending,
        Other
    }
}