using Itinero;
using Itinero.LocalGeo;
using Itinero.Profiles;
using Itinero.Navigation;
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

        public static Route Calculate(string profileName, Coordinate from, Coordinate to){
            Profile profile = RouterInstance.getRouter().Db.GetSupportedProfile(profileName);
            var point1 = router.TryResolve(profile, from, 50);
            var point2 = router.TryResolve(profile, to, 50);
            
            return router.Calculate(profile, point1.Value, point2.Value);
        }
    }
}