using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace bhuylebr.kinect.scratch
{
    public class KinectStartup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes(); 
            
            // Include this line to log all http messages
            // config.MessageHandlers.Add( new MessageLoggingHandler() );

            appBuilder.UseWebApi(config);
        }
    }
}
