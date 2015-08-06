using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Owin.Hosting;
using System.Diagnostics;

namespace bhuylebr.kinect.scratch
{
    public class ScratchWebServer
    {
        public const string PORTNUMBER = "8000";
        public const string BASEADDRESS = "http://+:" + PORTNUMBER + "/";

        private static IDisposable theServer = null;
        private static MPScratchController ctrl = null;

        public static bool Start() {
            if (theServer == null) {
                ctrl = new MPScratchController();
                try
                {
                    theServer = WebApp.Start<KinectStartup>(url: BASEADDRESS);
                }
                catch (Exception e)
                {
                    //I'm guessing I get an exception because I'm not allowed to listen to this address/port
                    //so register this port to acl (requires admin)...
                    registerAddress(BASEADDRESS);
                    //...and then try again
                    theServer = WebApp.Start<KinectStartup>(url: BASEADDRESS);
                }
                

                return true;
            }
            return false;
        }

        public static bool Stop()
        {
            if (theServer != null)
            {
                theServer.Dispose();
                theServer = null;
                return true;
            }
            return false;
        }

        public static void kakawriteMessage(string name, string value) {
            ctrl.writeMessage(name, value);
        }

        public static void writeVariable(string blockName, string msgName, string msgValue)
        {
            ctrl.writeVariable(blockName, msgName, msgValue);
        }

        private static void registerAddress(string baseadress)
        {
            ProcessStartInfo psi = new ProcessStartInfo("netsh", "http add urlacl url=" + baseadress + " user=Everyone listen=yes");
            psi.Verb = "runas";
            psi.CreateNoWindow = true;
            psi.WindowStyle = ProcessWindowStyle.Hidden;
            psi.UseShellExecute = true;

            Process.Start(psi).WaitForExit();
        }
    }
}
