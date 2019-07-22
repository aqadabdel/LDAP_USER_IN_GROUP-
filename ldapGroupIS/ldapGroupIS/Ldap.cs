using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;


/* CHIMIE  */
namespace ldapGroupIS
{
    class Ldap
    {

        private static string _ldap_host, _ldap_port, _ldap_base, _ldap_username, _ldap_pw, _ldap_url;
        private static DirectoryEntry _DirEntry;

       
         

        public Ldap()
        {
            _ldap_base = "dc=iuta,dc=priv,dc=univ-lille1,dc=fr";
            _ldap_host = "the ¨LDAP IP";
            _ldap_port = "389";
            _ldap_url = "LDAP://" + _ldap_host + ":" + _ldap_port + "/";
            _ldap_username = "cn=admin," + _ldap_base;
            _ldap_pw = "THE LDAP PASSWORD";
            LDAP_connect();
        }
      

        public Ldap(string ldap_host, string ldap_port, string ldap_base, string ldap_username, string ldap_pw)
        {
            _ldap_host = ldap_host;
            _ldap_port = ldap_port;
            _ldap_url = "LDAP://" + ldap_host + ":" + ldap_port + "/";
            _ldap_base = ldap_base;
            _ldap_username = ldap_username + _ldap_base;
            _ldap_pw = ldap_pw;
            LDAP_connect();
        }

        private void LDAP_connect()
        {
            _DirEntry = new DirectoryEntry(_ldap_host + _ldap_base, _ldap_username, _ldap_pw, AuthenticationTypes.None);
        }

        public bool LDAP_search_user(string uid)
        {
            string ldapfilter = "(uid=" + uid + ")";

            using (_DirEntry)
            {
                DirectorySearcher ds = new DirectorySearcher(_DirEntry);
                ds.Filter = string.Format(ldapfilter, _ldap_username);
                SearchResult result = ds.FindOne();
                if (result != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public string get_gid_number_of_group(string group)
        {
            
            string group_filter = "(&(objectClass=posixgroup)(cn=" + group + "))";

            DirectorySearcher ds = new DirectorySearcher(new DirectoryEntry(_ldap_url + _ldap_base, _ldap_username, _ldap_pw, AuthenticationTypes.ReadonlyServer),
                                   string.Concat("(objectClass=posixgroup)"),
                                   new string[] { "cn","gidnumber" });
            
            ds.Filter = string.Format(group_filter, _ldap_username);
            SearchResult result = ds.FindOne();

            if (result != null)
            {
                return result.Properties["gidnumber"][0].ToString();
            }
        
            return false.ToString();
        }


        public string get_gid_number_of_user(string uid)
        {

            string group_filter = "(&(objectClass=posixaccount)(uid=" + uid + "))";

            DirectorySearcher ds = new DirectorySearcher(new DirectoryEntry(_ldap_url + _ldap_base, _ldap_username, _ldap_pw, AuthenticationTypes.ReadonlyServer),
                                   string.Concat("(objectClass=posixaccount)"),
                                   new string[] { "uid", "gidnumber" });

            ds.Filter = string.Format(group_filter, _ldap_username);
            SearchResult result = ds.FindOne();

            if( result != null)
            {
                return result.Properties["gidnumber"][0].ToString();
            }

            return false.ToString();
        }

        public List<string> LDAP_list_users()
        {
            string ldapfilter = "(uid=*)";
            List<string> resultat = new List<string>();

            DirectorySearcher ds = new DirectorySearcher(new DirectoryEntry(_ldap_url + _ldap_base, _ldap_username, _ldap_pw, AuthenticationTypes.ReadonlyServer));
            ds.Filter = string.Format(ldapfilter, _ldap_username);
            SearchResultCollection result = ds.FindAll();

            for (int i = 0; i < result.Count; i++)
            {
                resultat.Add(result[i].Properties["uid"][0].ToString());
            }
            return resultat;
        }
    }
}
