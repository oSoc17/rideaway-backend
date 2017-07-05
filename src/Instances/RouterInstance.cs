using Itinero;
using System.IO;

namespace rideaway_backend.Instance
{
    public static class RouterInstance
    {
        private static Router router;
        private static RouterDb routerDb;

        public static void initialize(){
            //load data in ram
            using (var stream = new FileInfo(@"mapdata/belgium.routerdb").OpenRead())
            {
                routerDb = RouterDb.Deserialize(stream);
            }
            //TODO load bicycle profile
            router = new Router(routerDb);
        }

        public static Router getRouter(){
            return router;
        }
    }
}