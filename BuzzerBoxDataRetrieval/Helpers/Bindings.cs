using System.Diagnostics;
using System.Reflection;
using BuzzerBoxDataRetrieval.DataProviders;
using BuzzerBoxDataRetrieval.Network;
using BuzzerEntities.Models;
using Ninject;
using Ninject.Modules;
using Ninject.Parameters;

namespace BuzzerBoxDataRetrieval.Helpers
{
    public class Bindings : NinjectModule
    {
        private static IKernel kernel;

        public static IKernel Kernel
        {
            get
            {
                if (kernel == null)
                {
                    kernel = new StandardKernel();
                    kernel.Load(Assembly.GetExecutingAssembly());
                }
                return kernel;
            }
        }

        public override void Load()
        {
            Debug.WriteLine("Loading Ninject bindings.");
            Bind<IHttpConnection>().To<HttpConnection>();
            Bind<IHttpDataRequestFactory>().To<HttpDataRequestFactory>();
            Bind<IStringDataConverter<Room>>().To<JsonDataConverter<Room>>();
            /*
            Bind<HttpDataProvider<Room>>()
                .ToSelf()
                .WithConstructorArgument("sessionToken", "8ltkI1WnUPRQ3hvj");
            */
            Bind<IDataProvider<Room>>().To<HttpDataProvider<Room>>();
        }
    }
}