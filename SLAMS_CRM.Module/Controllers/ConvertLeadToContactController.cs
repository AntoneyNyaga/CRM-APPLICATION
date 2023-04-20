using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using SLAMS_CRM.Module.BusinessObjects;

public class ConvertLeadToContactController : ObjectViewController<ListView, Lead>
{
    public ConvertLeadToContactController()
    {
        // Create the "Convert to Contact" action
        var convertAction = new SimpleAction(this, "ConvertLeadToContact", PredefinedCategory.Edit)
        {
            Caption = "Convert to Contact",
            ToolTip = "Convert this lead to a contact",
            ImageName = "BO_Contact"
        };

        convertAction.Execute += ConvertAction_Execute;

        // Enable the action when a lead is selected with LeadStatus = Qualified
        convertAction.TargetObjectsCriteria = "[LeadStatus] == 'Qualified'";
    }

    private void ConvertAction_Execute(object sender, SimpleActionExecuteEventArgs e)
    {
        var selectedLeads = View.SelectedObjects;

        if (selectedLeads == null || selectedLeads.Count == 0)
            return;

        var objectSpace = View.ObjectSpace;
        var session = ((XPObjectSpace)objectSpace).Session;

        // Start a transaction to ensure data consistency
        using (var uow = new UnitOfWork(session.DataLayer))
        {
            // Loop through the selected leads
            foreach (Lead lead in selectedLeads)
            {
                // Create a new contact object and copy over relevant properties
                var contact = new Contact(session)
                {
                    FirstName = lead.FirstName,
                    MiddleName = lead.MiddleName,
                    LastName = lead.LastName,
                    Email = lead.Email,
                    Company = session.GetObjectByKey<Company>(lead.Company.Oid),
                    Account = session.GetObjectByKey<Account>(lead.Account.Oid),
                    JobTitle = lead.JobTitle,
                    Photo = lead.Photo
                };

                // Add the phone numbers of the lead to the contact
                foreach (var phoneNumber in lead.PhoneNumbers)
                {
                    var phone = new PhoneNumber(session)
                    {
                        Number = phoneNumber.Number,
                        PhoneType = phoneNumber.PhoneType,
                        //Party = phoneNumber.Party
                    };
                    contact.PhoneNumbers.Add(phone);
                }

                // Delete the lead object
                lead.LeadStatus = LeadStatus.Qualified;
                lead.Save();
               // objectSpace.Delete(lead);
            }

            objectSpace.CommitChanges();
        }

        // Refresh the view to show the updated data
        View.ObjectSpace.Refresh();
    }
}
