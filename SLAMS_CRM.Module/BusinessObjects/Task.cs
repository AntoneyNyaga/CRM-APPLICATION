using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraPrinting.Native;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Task = DevExpress.Persistent.BaseImpl.Task;


namespace SLAMS_CRM.Module.BusinessObjects
{

    [DefaultClassOptions]
    [NavigationItem("Inbox")]
    [ModelDefault("Caption", "Task")]
    [Appearance("FontColorRed", AppearanceItemType = "ViewItem", TargetItems = "*", Context = "ListView", Criteria = "Status=='Completed'", FontStyle = System.Drawing.FontStyle.Strikeout)]
    [RuleCriteria("Task_Status", DefaultContexts.Save, "IIF(Status != 'NotStarted' and Status != 'Deferred', AssignedTo is not null, True)", CustomMessageTemplate = @"The task must have an assignee when its Status is ""In progress"", ""Waiting for someone else"", or ""Completed"".", SkipNullOrEmptyValues = false)]
    [RuleCriteria("TaskIsNotStarted", DefaultContexts.Save, "Status != 'NotStarted'", CustomMessageTemplate = "Cannot set the task completed because it's not started.", TargetContextIDs = "MarkCompleted")]
    public class DemoTask : Task, IComparable
    {
        private Priority priority;
        private int estimatedWorkHours;
        private int actualWorkHours;
        public DemoTask(Session session)
            : base(session)
        {
        }
        [Appearance("PriorityBackColorPink", AppearanceItemType = "ViewItem", Criteria = "Priority=2", BackColor = "0xfff0f0")]
        public Priority Priority
        {
            get
            {
                return priority;
            }
            set
            {
                SetPropertyValue(nameof(Priority), ref priority, value);
            }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Priority = Priority.Normal;
        }
        [ToolTip("View, assign or remove application users for the current task")]
        [Association("ApplicationUser-DemoTask")]
        public XPCollection<ApplicationUser> ApplicationUsers
        {
            get
            {
                return GetCollection<ApplicationUser>(nameof(ApplicationUsers));
            }
        }
        public override string ToString()
        {
            return this.Subject;
        }
        [Action(ToolTip = "Postpone the task to the next day", ImageName = "State_Task_Deferred")]
        public void Postpone()
        {
            if (DueDate == DateTime.MinValue)
            {
                DueDate = DateTime.Now;
            }
            DueDate = DueDate + TimeSpan.FromDays(1);
        }
        [RuleValueComparison("Task_EstimatedWorkHours", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public int EstimatedWorkHours
        {
            get
            {
                return estimatedWorkHours;
            }
            set
            {
                SetPropertyValue<int>(nameof(EstimatedWorkHours), ref estimatedWorkHours, value);
            }
        }
        [RuleValueComparison("Task_ActualWorkHours", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0)]
        public int ActualWorkHours
        {
            get
            {
                return actualWorkHours;
            }
            set
            {
                SetPropertyValue<int>(nameof(ActualWorkHours), ref actualWorkHours, value);
            }
        }
    }
    public enum Priority {
    [ImageName("State_Priority_Low")]
    Low = 0,
    [ImageName("State_Priority_Normal")]
    Normal = 1,
    [ImageName("State_Priority_High")]
    High = 2
}
}