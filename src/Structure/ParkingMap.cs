using System.Collections.Generic;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using System;

namespace rideaway_backend.Structure {
    public class ParkingMap {
        private IList<Feature> features;

        public ParkingMap() {
            features = new List<Feature>();
        }

        public void Add(Feature f){
            features.Add(f);
        }

        public List<Feature> RadiusFeatures(double x, double y, int radius){

            List<Feature> inside = new List<Feature>();
            foreach (Feature f in features){
                Console.WriteLine(Distance(f, x, y));
                if(Distance(f, x, y) < radius){

                    inside.Add(f);
                }
            }
            return inside;
            
        }

        public double Distance(Feature f, double x, double y){
            Point p = (Point)f.Geometry;
            return Math.Sqrt(Math.Pow(p.Coordinates.Longitude - x, 2) + Math.Pow(p.Coordinates.Latitude - y, 2));
        }


    }
}