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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceModel.Channels;
using System.Text;

namespace SLAMS_CRM.Module.BusinessObjects
{
    [DefaultClassOptions]

    [NavigationItem("Accounting")]
    [DefaultProperty("Description")]
    [Persistent("Quote")]
    [ImageName("BO_Quote")]


    public class Quote : BaseObject
    {
        public Quote(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            _lastFollowUp = DateTime.Now;
        }



        [Size(50)]
        [RuleRequiredField("RuleRequiredField for Quote.Title", DefaultContexts.Save)]
        public string Title { get => title; set => SetPropertyValue(nameof(Title), ref title, value); }

        // one to one relationship between Quote and Account
        public Account Account { get; set; }

        public Opportunity Opportunity { get; set; }

        [RuleRequiredField("RuleRequiredField for Quote.Contact", DefaultContexts.Save)]
        public Contact Contact { get; set; }

        [RuleRequiredField("RuleRequiredField for Quote.ShippingAddress", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [DevExpress.Xpo.Aggregated]
        public Address ShippingAddress
        {
            get => shippingAddress;
            set => SetPropertyValue(nameof(ShippingAddress), ref shippingAddress, value);
        }

        [RuleRequiredField("RuleRequiredField for Quote.BillingAddress", DefaultContexts.Save)]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [DevExpress.Xpo.Aggregated]

        public Address BillingAddress
        {
            get => billingAddress;
            set => SetPropertyValue(nameof(BillingAddress), ref billingAddress, value);
        }

        [RuleRequiredField("RuleRequiredField for Quote.Product", DefaultContexts.Save)]
        [VisibleInListView(false)]
        public Product Product { get; set; }


        Address billingAddress;
        Address shippingAddress;
        string title;
        string approvalIssues;
        DateTime validUntil;

        [RuleRequiredField("RuleRequiredField for Quote.DateCreated", DefaultContexts.Save)]
        public DateTime ValidUntil
        {
            get => validUntil;
            set => SetPropertyValue(nameof(ValidUntil), ref validUntil, value);
        }

        /*public ApplicationUser AssignedTo
        {
            get => assignedTo;
            set => SetPropertyValue(nameof(AssignedTo), ref assignedTo, value);
        }*/

        [Association("ApplicationUser-Quote")]
        public XPCollection<ApplicationUser> AssignedUsers
        {
            get
            {
                return GetCollection<ApplicationUser>(nameof(AssignedUsers));
            }
        }


        // the one part of the Association
        //[Association("Quote-Products")]
        //public XPCollection<Product> Products { get { return GetCollection<Product>(nameof(Products)); } }

        private decimal _price;
        [RuleValueComparison(ValueComparisonType.GreaterThan, 0)]
        public decimal Price { get => _price; set => SetPropertyValue(nameof(Price), ref _price, value); }

        private ApprovalStatus _status;

        public ApprovalStatus ApprovalStatus
        {
            get => _status;
            set => SetPropertyValue(nameof(ApprovalStatus), ref _status, value);
        }

        private QuoteStage quoteStage;

        //[RuleRequiredField("RuleRequiredField for Quote.QuoteStage", DefaultContexts.Save)]
        public QuoteStage QuoteStage
        {
            get => quoteStage;
            set => SetPropertyValue(nameof(QuoteStage), ref quoteStage, value);
        }

        private InvoiceStatus invoiceStatus;

        public InvoiceStatus InvoiceStatus
        {
            get => invoiceStatus;
            set => SetPropertyValue(nameof(InvoiceStatus), ref invoiceStatus, value);
        }

        [RuleRequiredField("RuleRequiredField for Quote.ApprovalIssues", DefaultContexts.Save)]
        [Size(4096)]
        public string ApprovalIssues
        {
            get => approvalIssues;
            set => SetPropertyValue(nameof(ApprovalIssues), ref approvalIssues, value);
        }

        [Action(
            Caption = "Follow Up",
            ConfirmationMessage = "Are you sure you want to follow up on this quote?",
            ImageName = "FollowUp")]
        public void FollowUpAction() { FollowUp(); }

        [Action(
            Caption = "Send Quote",
            ConfirmationMessage = "Are you sure you want to send this quote to the customer?",
            ImageName = "Send")]
        public void SendQuoteAction() { SendQuote(); }

        public string GenerateProposal()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Proposal for {Title} - {ValidUntil.ToShortDateString()}");
            sb.AppendLine("-------------------------------------------------");
            sb.AppendLine($"Quote Status: {QuoteStage.ToString()}");
            sb.AppendLine($"Approval Status: {ApprovalStatus.ToString()}");
            sb.AppendLine($"Assigned To: {(AssignedUsers != null ? string.Join(", ", AssignedUsers.Select(u => u.UserName)) : "Not Assigned")}");
            sb.AppendLine();
            sb.AppendLine("Billing Address:");
            sb.AppendLine(BillingAddress.ToString());
            sb.AppendLine();
            sb.AppendLine("Shipping Address:");
            sb.AppendLine(ShippingAddress.ToString());
            sb.AppendLine();
            sb.AppendLine("Products:");
            sb.AppendLine($"{Product.Name} - {Product.Description} - {Price:C}");
            sb.AppendLine();
            sb.AppendLine($"Total Price: {Price:C}");
            sb.AppendLine("-------------------------------------------------");
            sb.AppendLine("Thank you for your business!");
            return sb.ToString();
        }


        private DateTime _lastFollowUp;

        [ReadOnly( true )]
        public DateTime LastFollowUp
        {
            get => _lastFollowUp;
            set => SetPropertyValue(nameof(LastFollowUp), ref _lastFollowUp, value);
        }


        public void FollowUp()
        {
            if(QuoteStage == QuoteStage.Sent && LastFollowUp.AddDays(7) < DateTime.Now)
            {
                // create a MailMessage object
                MailMessage mail = new MailMessage();

                // set the email address of the recipient
                mail.To.Add(Contact.Email);

                // set the subject of the email
                mail.Subject = "Follow-up on Quote #" + Title;

                // set the body of the email
                mail.Body = "Dear Customer," +
                    Environment.NewLine +
                    Environment.NewLine +
                    "We hope this email finds you well. We wanted to follow up on the quote we sent you " +
                    "on " +
                    ValidUntil.ToShortDateString() +
                    ". Please let us know if you have any " +
                    "questions or concerns about the quote." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Thank you for considering our services." +
                    Environment.NewLine +
                    Environment.NewLine +
                    "Best regards," +
                    Environment.NewLine +
                    "SLAMS CRM Team";

                /*// create a SmtpClient object
                SmtpClient smtp = new SmtpClient();

                // set the SMTP server details
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;

                // set the credentials for the SMTP server via google app password
                smtp.Credentials = new NetworkCredential("flaughters@gmail.com", "#");

                // send the email
                smtp.Send(mail);*/

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    // set the credentials for the SMTP server via google app password
                    smtp.Credentials = new NetworkCredential("flaughters@gmail.com", "#");
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                // update the last follow-up date
                LastFollowUp = DateTime.Now;
            }
        }

        public void SendQuote()
        {
            // Generate the proposal and send it to the customer via email
            string proposal = GenerateProposal();
            string emailBody = $"Dear {Contact.DisplayName},\n\nPlease find attached the proposal for {Title}.\n\n{proposal}\n\nKind regards,\n\nSLAMS CRM Team";

            MailMessage message = new MailMessage();
            message.From = new MailAddress("flaughters@gmail.com");
            message.To.Add(Account.EmailAddress);
            message.Subject = $"Proposal for {Title}";
            message.Body = emailBody;

            Attachment attachment = new Attachment(
                new MemoryStream(Encoding.UTF8.GetBytes(proposal)),
                $"Proposal - {Title}.txt");
            message.Attachments.Add(attachment);

            /*SmtpClient client = new SmtpClient("smtp.gmail.com");
            client.Send(message);*/

            using(SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
            {
                smtp.Credentials = new NetworkCredential("flaughters@gmail.com", "#");
                smtp.EnableSsl = true;
                smtp.Send(message);
            }

            // Update the QuoteStage and InvoiceStatus properties
            QuoteStage = QuoteStage.Sent;
            InvoiceStatus = InvoiceStatus.NotInvoiced;
            Save();
        }
    }

    public enum ApprovalStatus
    {
        Approved,
        NotApproved
    }

    public enum QuoteStage
    {
        Draft,
        Sent,
        OnHold,
        Accepted,
        Declined
    }

    public enum InvoiceStatus
    {
        NotInvoiced,
        Invoiced
    }
}

/*agsiachvqpoekxow for 509*/
/*xlspwfcjtweekxvl for 209*/