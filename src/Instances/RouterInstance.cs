using Itinero;
using Itinero.LocalGeo;
using Itinero.Profiles;
using Itinero.Navigation;
using System.IO;
using rideaway_backend.Exceptions;

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
            if (point1.IsError){
                throw new ResolveException("Location 1 could not be resolved");
            }
            var point2 = router.TryResolve(profile, to, 50);
            if (point2.IsError){
                throw new ResolveException("Location 2 could not be resolved");
            }
            
            return router.Calculate(profile, point1.Value, point2.Value);
        }
    }
}