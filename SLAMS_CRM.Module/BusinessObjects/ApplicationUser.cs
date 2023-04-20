using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;

namespace SLAMS_CRM.Module.BusinessObjects;

[MapInheritance(MapInheritanceType.ParentTable)]
[DefaultProperty(nameof(UserName))]
public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo {
    public ApplicationUser(Session session) : base(session) { }

    [Browsable(false)]
    [Aggregated, Association("User-LoginInfo")]
    public XPCollection<ApplicationUserLoginInfo> LoginInfo {
        get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
    }

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey) {
        ApplicationUserLoginInfo result = new ApplicationUserLoginInfo(Session);
        result.LoginProviderName = loginProviderName;
        result.ProviderUserKey = providerUserKey;
        result.User = this;
        return result;
    }

    //[Browsable(false)]
    //public IList<Quote> Quotes { get; } = new ObservableCollection<Quote>();

    [Association("ApplicationUser-Quote")]
    public XPCollection<Quote> AssignedProposals
    {
        get
        {
            return GetCollection<Quote>(nameof(AssignedProposals));
        }
    }

    //[Browsable(false)]
    //public IList<Opportunity> Opportunities { get; set; } = new ObservableCollection<Opportunity>();
    [Association("ApplicationUser-Opportunity")]
    public XPCollection<Opportunity> AssignedOpportunities
    {
        get
        {
            return GetCollection<Opportunity>(nameof(AssignedOpportunities));
        }
    }

    [Association("ApplicationUser-Assignment")]
    public XPCollection<Assignment> Tasks
    {
        get
        {
            return GetCollection<Assignment>(nameof(Tasks));
        }
    }
}
