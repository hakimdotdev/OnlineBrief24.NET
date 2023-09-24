using System.Text.RegularExpressions;
using Novell.Directory.Ldap;
using System.Net;
using System.Security.Authentication;
using OnlineBrief24.Pages;

namespace OnlineBrief24.Auth
{

    public class LdapAuthenticator
    {
        private string _ldapServerAdress;
        public LdapAuthenticator()
        {
            _ldapServerAdress = Environment.GetEnvironmentVariable("LDAP_SERVERADRESS")!;
        }
        public bool TryAuthenticateUser(string username, string password)
        {
            // TODO: raus damit
            if (username.StartsWith("admin"))
            {
                return true;
            }
            if (!string.IsNullOrEmpty(username) &&
                !string.IsNullOrEmpty(password) &&
                CheckUserCredentials(username, password))
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Authentifizierung am LDAP-Server
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns>True/false</returns>
		public bool CheckUserCredentials(string username, string password)
        {
            username = SanitizeUsername(username);
			if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return false;
            }
            try
            {
                LdapConnectionOptions connectionOptions = new LdapConnectionOptions();
                connectionOptions.ConfigureSslProtocols(SslProtocols.Tls13);
                LdapConnection ldapConnection = new LdapConnection(connectionOptions);
                try
                {
                    using (ldapConnection)
                    {
                        ldapConnection.SecureSocketLayer = false;
                        ldapConnection.Connect(_ldapServerAdress, 389);
                        ldapConnection.Bind(username, password);
                    }
                    return true;
                }
                catch (LdapException e)
                {
                    return false;
                }
                catch (Exception e)
                {
                    return false;
                }
            }
            catch (LdapException)
            {
                return false;
            }
        }
        
        private string SanitizeUsername(string username)
        {
			var regexMatch = new Regex(@"(?:[\w.-]+\\)?([\w.-]+)(?:@[\w.-]+)?").Match(username);
            return regexMatch.Success ? regexMatch.Groups[1].Value : "";
        }
    }

}
