using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace cSharpRegistryProject
{
    class Program
    {
        static void Main(string[] args)
        {
            IPHostEntry entry = Dns.GetHostEntry("192.168.0.46");
            var hostName = entry.HostName;

            Console.WriteLine("attempting to access device...");
            Console.WriteLine("Device name = {0}", hostName);

            RegistryKey environmentKey;
            string remoteName = hostName;

            try
            {
                // Open HKEY_CURRENT_USER\Environment 
                // on a remote computer.
                environmentKey = RegistryKey.OpenRemoteBaseKey(
                    RegistryHive.CurrentUser, remoteName).OpenSubKey(@"Volatile Environment");
            }
            catch (Exception e)
            {
                Console.WriteLine("{0}: {1}",
                    e.GetType().Name, e.Message);
                return;
            }
            /* this shows all of the possible information that can be used as the subkey instead of environment
            Console.WriteLine("subkeys = ");

            for (int i = 0; i < environmentKey.SubKeyCount; i++)
            {
                Console.WriteLine(environmentKey.GetSubKeyNames()[i].ToString());
            }*/
            
            // Print the values.
            Console.WriteLine("\nThere are {0} values for {1}.",
                environmentKey.ValueCount.ToString(),
                environmentKey.Name);


            String currentUserLoggedOn = "No one is logged on";
            foreach (string valueName in environmentKey.GetValueNames())
            {
                if(valueName == "USERNAME")
                {
                    currentUserLoggedOn = environmentKey.GetValue(valueName).ToString();
                }
                //Console.WriteLine("{0,-20}: {1}", valueName, environmentKey.GetValue(valueName).ToString());
            }

            Console.WriteLine("{0} is currently logged on", currentUserLoggedOn);

            // Close the registry key.
            environmentKey.Close();
        }

    }
}
