using Itinero;
using Itinero.LocalGeo;
using Itinero.Profiles;
using Itinero.Navigation;
using System.IO;
using rideaway_backend.Exceptions;
using System;
using rideaway_backend.FileMonitoring;

namespace rideaway_backend.Instance
{
    public static class RouterInstance
    {
        private static Router router;
        private static RouterDb routerDb;
        private static FilesMonitor<FileInfo> monitor;

        public static void initialize(){
            //load data in ram
            using (var stream = new FileInfo(@"mapdata/belgium.routerdb").OpenRead())
            {
                routerDb = RouterDb.Deserialize(stream);
            }
            router = new Router(routerDb);
            monitor = new FilesMonitor<FileInfo>((f) =>
            {
                using (var stream = new FileInfo(@"mapdata/belgium.routerdb").OpenRead())
                {
                    routerDb = RouterDb.Deserialize(stream);
                }
                router = new Router(routerDb);
                return true;
            }, new FileInfo(@"mapdata/belgium.routerdb"));
            monitor.Start();
            monitor.AddFile("mapdata/belgium.routerdb");
        }

        public static Router getRouter(){
            return router;
        }

        public static Route Calculate(string profileName, Coordinate from, Coordinate to){
            Vehicle vehicle = RouterInstance.getRouter().Db.GetSupportedVehicle("bicycle");
            
            var point1 = router.TryResolve(vehicle.Profile(profileName), from, 50);
            if (point1.IsError){
                throw new ResolveException("Location 1 could not be resolved");
            }
            var point2 = router.TryResolve(vehicle.Profile(profileName), to, 50);
            if (point2.IsError){
                throw new ResolveException("Location 2 could not be resolved");
            }
            
            return router.Calculate(vehicle.Profile(profileName), point1.Value, point2.Value);
        }
    }
}