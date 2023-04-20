using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using SLAMS_CRM.Module.BusinessObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskStatus = DevExpress.Persistent.Base.General.TaskStatus;


namespace SLAMS_CRM.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AssignmentActionsController : ObjectViewController<ObjectView, Assignment>
    {
        private ChoiceActionItem setPriorityItem;
        private ChoiceActionItem setStatusItem;
        private SingleChoiceAction SetTaskAction;

        public AssignmentActionsController()
        {
            SetTaskAction = new SingleChoiceAction(
                this,
                "SetTaskAction",
                DevExpress.Persistent.Base.PredefinedCategory.Edit)
            {
                Caption = "Set Task",
                SelectionDependencyType = SelectionDependencyType.RequireMultipleObjects,
                ImageName = "Task",
                ItemType = SingleChoiceActionItemType.ItemIsOperation
            };
            SetTaskAction.Execute += SetTaskAction_Execute;

            setPriorityItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(Assignment), "Priority"), null);
            SetTaskAction.Items.Add(setPriorityItem);
            FillItemWithEnumValues(setPriorityItem, typeof(Priority));
            setStatusItem = new ChoiceActionItem(CaptionHelper.GetMemberCaption(typeof(Assignment), "Status"), null);
            SetTaskAction.Items.Add(setStatusItem);
            FillItemWithEnumValues(setStatusItem, typeof(TaskStatus));
        }

        private void UpdateSetTaskActionState()
        {
            bool isGranted = true;

            SecurityStrategy security = Application.GetSecurityStrategy();
            foreach(object selectedObject in View.SelectedObjects)
            {
                bool isPriorityGranted = security.IsGranted(
                    new PermissionRequest(
                        ObjectSpace,
                        typeof(Assignment),
                        SecurityOperations.Write,
                        selectedObject,
                        nameof(Assignment.Priority)));
                bool isStatusGranted = security.IsGranted(
                    new PermissionRequest(
                        ObjectSpace,
                        typeof(Assignment),
                        SecurityOperations.Write,
                        selectedObject,
                        nameof(Assignment.Status)));
                if(!isPriorityGranted || !isStatusGranted)
                {
                    isGranted = false;
                }
            }
            SetTaskAction.Enabled.SetItemValue("SecurityAllowance", isGranted);
        }

        private void FillItemWithEnumValues(ChoiceActionItem parentItem, Type enumType)
        {
            EnumDescriptor ed = new EnumDescriptor(enumType);
            foreach(object current in Enum.GetValues(enumType))
            {
                ChoiceActionItem item = new ChoiceActionItem(ed.GetCaption(current), current);
                item.ImageName = ImageLoader.Instance.GetEnumValueImageName(current);
                parentItem.Items.Add(item);
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            View.SelectionChanged += View_SelectionChanged;
            UpdateSetTaskActionState();
        }

        private void View_SelectionChanged(object sender, EventArgs e) { UpdateSetTaskActionState(); }

        private Assignment GetObject(
            Assignment obj,
            IObjectSpace objectSpace,
            IObjectSpace newObjectSpace,
            ref int newObjectsCount)
        {
            if(objectSpace.IsNewObject(obj))
            {
                newObjectsCount++;
                return obj;
            }
            return newObjectSpace.GetObject(obj);
        }

        private void SetTaskAction_Execute(object sender, SingleChoiceActionExecuteEventArgs args)
        {
            IObjectSpace objectSpace = View is ListView
                ? Application.CreateObjectSpace(typeof(Assignment))
                : View.ObjectSpace;
            int newObjectsCount = 0;
            ArrayList objectsToProcess = new ArrayList(args.SelectedObjects);
            if(args.SelectedChoiceActionItem.ParentItem == setPriorityItem)
            {
                foreach(object obj in objectsToProcess)
                {
                    Assignment objInNewObjectSpace = GetObject(
                        (Assignment)obj,
                        View.ObjectSpace,
                        objectSpace,
                        ref newObjectsCount);
                    objInNewObjectSpace.Priority = (Priority)args.SelectedChoiceActionItem.Data;
                }
            } else if(args.SelectedChoiceActionItem.ParentItem == setStatusItem)
            {
                foreach(object obj in objectsToProcess)
                {
                    Assignment objInNewObjectSpace = GetObject(
                        (Assignment)obj,
                        View.ObjectSpace,
                        objectSpace,
                        ref newObjectsCount);
                    objInNewObjectSpace.Status = (TaskStatus)args.SelectedChoiceActionItem.Data;
                }
            }
            if(View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View)
            {
                objectSpace.CommitChanges();
            }
            if((View is ListView) && (newObjectsCount != objectsToProcess.Count))
            {
                objectSpace.CommitChanges();
                View.ObjectSpace.Refresh();
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
