using Microsoft.OneDrive.Sdk;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.UserProfiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EEANWorks.Microsoft
{
    public sealed class OneDriveConnectionManager
    {
        public OneDriveConnectionManager(string _siteUrl, string _username, string _password)
        {
            //m_personalUrl = GetUserPersonalUrlCSOM(_siteUrl, _username);
            DownloadFile("");

            SecureString securePassword = new SecureString();
            foreach (char c in _password)
            { securePassword.AppendChar(c); }

            m_onlineCredentials = new SharePointOnlineCredentials(_username, securePassword);
        }

        private readonly string m_personalUrl;
        private readonly SharePointOnlineCredentials m_onlineCredentials;

        public string GetUserPersonalUrlCSOM(string _siteUrl, string _userPrincipalName)
        {
            try
            {
                using (ClientContext cContext = new ClientContext("https://iberopuebla-my.sharepoint.com/personal/711943_iberopuebla_mx/_layouts/15/onedrive.aspx"))
                {
                    string result = null;

                    var user = cContext.Web.EnsureUser(_userPrincipalName);
                    cContext.Load(user);
                    cContext.ExecuteQuery();

                    PeopleManager peopleManager = new PeopleManager(cContext);
                    var userProperties = peopleManager.GetPropertiesFor(user.LoginName);
                    cContext.Load(userProperties);
                    cContext.ExecuteQuery();
                    result = userProperties.PersonalUrl;

                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return null;
            }
        }

        public string DownloadFile(string _fileName, string _clientSubFolder = "")
        {
            try
            {
                #region Load the data
                using (ClientContext cContext = new ClientContext(@"https://iberopuebla-admin.sharepoint.com/"))
                {
                    cContext.Credentials = m_onlineCredentials;
                    Web web = cContext.Web;

                    var lists = web.Lists;
                    cContext.Load(lists);
                    cContext.ExecuteQuery();

                    string listnames = "";
                    foreach (var list in lists)
                    {
                        cContext.Load(list);
                        cContext.ExecuteQuery();
                        listnames += list.Title + "\n";
                    }

                    MessageBox.Show(listnames);

                    //List list = web.Lists.GetByTitle(m_docLibrary);
                    return "";
                }
                #endregion
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception occurred : " + ex.Message
                    + "\nPlease check your internet connection and try again.");
                return null;
            }
        }
    }
}
