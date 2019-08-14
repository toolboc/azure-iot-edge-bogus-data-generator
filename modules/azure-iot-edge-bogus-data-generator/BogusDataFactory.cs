// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Bogus;
using Bogus.DataSets;
using IoTEdgeBogusDataGenerator.Models;
using System.Collections.Generic;

namespace IoTEdgeBogusDataGenerator
{
    public class BogusDataFactory
    {


        public static Object CreateBogusData(int counter)
        {
            //Set the randomzier seed if you wish to generate repeatable data sets.
            Randomizer.Seed = new Random(counter);

            var vehicleData = new Faker<VehicleData>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode
                .StrictMode(true)
                //Id is deterministic
                .RuleFor(o => o.Id, f => counter)
                //Generate Random value for Speed
                .RuleFor(o => o.Speed, f => f.Random.Number(0, 100))
                //Generate Random value for CurrentLocation
                .RuleFor(o => o.CurrentLocation, (f,o) => GetFakeCurrentLocation(f, o))
                //Generate Random value for Destination
                .RuleFor(o => o.Destination, (f,o) => GetFakeDestination(f, o.CurrentLocation))
                //Generate Fake Ip Address
                .RuleFor(o => o.Ip, f => f.Internet.Ip())
                //Get Fake Items
                .RuleFor(o => o.Token, f => f.Random.AlphaNumeric(10))
                //Stamp with the current time
                .RuleFor(o => o.Timestamp, f => DateTime.UtcNow);

            return vehicleData.Generate();
        }

        public static string GetFakeCurrentLocation(Faker f, VehicleData o)
        {
            //Latitude range( 49.95- 67.5) Longitude range( 25.10 – 135.05) 
            double LatitudeMin = 49.95;
            double LatitudeMax = 67.5;
            double LongitudeMin = 25.10;
            double LongitudeMax = 135.05;

            double CurrentLatitude = 0;
            double CurrentLongitude = 0;

            //Introduce bias to approximately 1/10 samples
            if(o.Id % 10 == 0)
            {
                //Moscow Coordinates
                var CommonLatitude = 55.75;
                var CommonLongitude = 37.61;

                var LatitudeRange = 1;
                var LongitudeRange = 1;

                CurrentLatitude = LatitudeRange * f.Random.Double() + CommonLatitude;
                CurrentLongitude = LongitudeRange * f.Random.Double() + CommonLongitude;

                o.Speed = f.Random.Number(0,25);
                
                Console.WriteLine("Correlation Forced");
            }
            else
            {
                var LatitudeRange = LatitudeMax - LatitudeMin;
                var LongitudeRange = LongitudeMax - LongitudeMin;

                CurrentLatitude = LatitudeRange * f.Random.Double() + LatitudeMin;
                CurrentLongitude = LongitudeRange * f.Random.Double() + LongitudeMin;
            }
            
            return String.Format("{0:0.00},{1:0.00}", CurrentLatitude, CurrentLongitude);            

        }
        public static string GetFakeDestination(Faker f, string currentLocation)
        {

            var CurrentLocation = currentLocation.Split(',');
            double CurrentLatitude = Convert.ToDouble(CurrentLocation[0]);
            double CurrentLongitude = Convert.ToDouble(CurrentLocation[1]);

            int DistanceMin = 10;
            int DistanceMax = 25;

            double DestinationLatitude = CurrentLatitude + f.Random.Int(DistanceMin, DistanceMax);
            double DestinationLongitude = CurrentLongitude + f.Random.Int(DistanceMin, DistanceMax);

            return String.Format("{0:0.00},{1:0.00}", DestinationLatitude, DestinationLongitude);

        }
    }
}