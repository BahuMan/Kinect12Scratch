using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace bhuylebr.kinect.scratch
{
    public class KinectController: ApiController
    {
        [Route("polleke")]
        [HttpGet]
        public HttpResponseMessage Poll()
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            var content = new StringBuilder();
            content.AppendLine("<HEAD>");
            content.AppendLine("<BODY>");
            content.AppendLine("<H1>Hello, world!</H1>");
            content.AppendLine("</BODY>");
            content.AppendLine("</HEAD>");
            resp.Content = new StringContent(content.ToString(), Encoding.UTF8, "text/html");
            return resp;
        }

    }
}
