using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;


namespace ldapGroupIS
{
    class Program
    {
        static void Main(string[] args)
        {

           const bool DEBUG = true;
           Ldap ldap = new Ldap();

           string groupname = "etuchimie";
           string username = "stef";
           string gid_group_gidnumber = ldap.get_gid_number_of_group(groupname);
           string gid_uid_gidnumber  = ldap.get_gid_number_of_user(username); 

           if( gid_uid_gidnumber == gid_group_gidnumber 
                && gid_group_gidnumber != "" && gid_uid_gidnumber != ""
                && gid_group_gidnumber != "False" && gid_uid_gidnumber != "False")

            {
                Console.WriteLine("1");
                if(DEBUG)
                {
                  Console.WriteLine("User {0} exists in group {1} | {2} : {3}", username, groupname, gid_group_gidnumber, gid_uid_gidnumber );
                }

            }
            else
            {
                Console.WriteLine("0");
                if (DEBUG)
                {
                    Console.Write("User {0} is not in group {1} | {2} : {3}", username, groupname, gid_group_gidnumber, gid_uid_gidnumber);
                    if (gid_uid_gidnumber == "False")
                        Console.WriteLine(" | Username: {0} don't exists in ldap", username);
                    if (gid_group_gidnumber == "False")
                        Console.WriteLine(" | {0} group don't exists in ldap", groupname);

                }
            }

            Console.ReadKey();
        }
    }
}
