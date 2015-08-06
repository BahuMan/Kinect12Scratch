using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;


namespace bhuylebr.kinect.scratch
{

    /**
     * This class implements an extension for Scratch.
     * For the program, see http://scratch.mit.edu
     * for the specification of the extension, see http://wiki.scratch.mit.edu/wiki/Scratch_Extension
     * 
     * I'm using the same extension/webapi as my java multiplayer extension, so this server and the java server can talk to each other.
     * Most important difference: this class does not (yet) implement a way to connect to an upstream server.
     * 
     * @TODO: implement upstream linking
     * @TODO: build this class into the Kinect Application
     */
    public class MPScratchController : ApiController
    {

        //owin/katana/webapi seems to construct a new class for each response, so the easiest way to retain data between calls is to make it status
        //this map stores all message names and their values
        //it can be retrieved using poll (http://localhost:8000/poll) and updated using writemessage (http://localhost:8000/writemessage/name/value)
        private static IDictionary<string, string> scratchVariables = null;

        public MPScratchController()
        {
            if (scratchVariables == null) scratchVariables = new Dictionary<string, string>(100);
        }

        [Route("poll")]
        [HttpGet]
        public HttpResponseMessage Poll()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            var content = new StringBuilder();
            lock (scratchVariables)
            {
                content.AppendLine("nrmessages " + scratchVariables.Count);
                foreach (KeyValuePair<string, string> kval in scratchVariables)
                {
                    content.Append(kval.Key);
                    content.Append(" ");
                    content.AppendLine(kval.Value);
                }

            }

            resp.Content = new StringContent(content.ToString(), Encoding.UTF8, "text/plain");
            return resp;
        }

        [Route("resetAll")]
        [HttpGet]
        public HttpResponseMessage resetAll()
        {
            lock (scratchVariables)
            {
                scratchVariables.Clear();
            }
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent("Cleared", Encoding.UTF8, "text/plain");
            return resp;
        }

        [Route("crossDomain")]
        [HttpGet]
        public HttpResponseMessage crossDomain()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            var content = new StringBuilder();
            content.Append("<cross-domain-policy> <allow-access-from domain=\"*\" to-ports=\"" + ScratchWebServer.PORTNUMBER + "\"/> </cross-domain-policy>\0");
            resp.Content = new StringContent(content.ToString(), Encoding.UTF8, "text/xml");
            return resp;
        }

        [Route("nrMessages")]
        [HttpGet]
        public HttpResponseMessage nrMessages()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            var content = new StringBuilder();
            resp.Content = new StringContent(scratchVariables.Count.ToString(), Encoding.UTF8, "text/plain");
            return resp;
        }

        [Route("writeMessage/{msgName}/{msgValue}")]
        [HttpGet]
        public HttpResponseMessage writeMessage(string msgName, string msgValue)
        {
            try
            {
                string variablename = "readmessage/" + msgName;
                lock (scratchVariables)
                {
                    scratchVariables[variablename] = msgValue;
                }
                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                var content = new StringBuilder();
                content.Append("total messages: " + scratchVariables.Count);
                resp.Content = new StringContent(content.ToString(), Encoding.UTF8, "text/plain");
                return resp;
            }
            catch (Exception ex)
            {
                scratchVariables[msgName] = msgValue;
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                var content = new StringBuilder();
                content.AppendLine(ex.Message);
                content.AppendLine(ex.ToString());
                resp.Content = new StringContent(content.ToString(), Encoding.UTF8, "text/plain");
                return resp;
            }

        }

        [Route("writeVariable/{blockName}/{varName}/{varValue}")]
        [HttpGet]
        public HttpResponseMessage writeVariable(string blockName, string msgName, string msgValue)
        {
            try
            {
                string variablename = blockName + "/" + msgName;
                lock (scratchVariables)
                {
                    scratchVariables[variablename] = msgValue;
                }
                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                var content = new StringBuilder();
                content.Append("total messages: " + scratchVariables.Count);
                resp.Content = new StringContent(content.ToString(), Encoding.UTF8, "text/plain");
                return resp;
            }
            catch (Exception ex)
            {
                scratchVariables[msgName] = msgValue;
                var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError);
                var content = new StringBuilder();
                content.AppendLine(ex.Message);
                content.AppendLine(ex.ToString());
                resp.Content = new StringContent(content.ToString(), Encoding.UTF8, "text/plain");
                return resp;
            }

        }

        public void writeKinectVariable(int body, string jointname, float x, float y, bool istracked)
        {
            StringBuilder sb = new StringBuilder(200);

            lock (scratchVariables)
            {

                scratchVariables["kinect/x/" + jointname + "/" + body] = "" + x;
                scratchVariables["kinect/y/" + jointname + "/" + body] = "" + y;
                scratchVariables["kinect/istracked/" + jointname + "/" + body] = "" + istracked;
            }

        }

    }
}
