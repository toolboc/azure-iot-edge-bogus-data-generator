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

        static List<Item> ItemCatalog = new List<Item>()
        {
            new Item(){ Name = "Jump Seat Blade Replacement Pack, Medium" , Price = 43 },
            new Item(){ Name = "Sasquatch Ale", Price = 6 },
            new Item(){ Name = "Thüringer Rostbratwurst", Price = 124 },
            new Item(){ Name = "Seasonal High Power Strobe Lights (Azure, 36 Volt, 900 Lumens)", Price = 155 }
        };
        static List<string> Stores = new List<string>()
        {
            "tailwind-traders.com (web site)",
            "Redmond Center"
        };

        public static Object CreateBogusData(int counter)
        {
            //Set the randomzier seed if you wish to generate repeatable data sets.
            Randomizer.Seed = new Random(counter);

            var testOrder = new Faker<Order>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode
                .StrictMode(true)
                //OrderNumber is deterministic
                .RuleFor(o => o.OrderNumber, f => counter)
                //Generate Random GUID for PurchaseId
                .RuleFor(o => o.PurchaseId, f => Guid.NewGuid())
                //Pick a Random Store
                .RuleFor(o => o.StoreName, f => f.PickRandom(Stores))
                //Generate Fake Name
                .RuleFor(o => o.CustomerName, f => f.Name.FullName())
                //Generate Fake Email
                .RuleFor(o => o.Email, (f,o) => f.Internet.Email(o.CustomerName))
                //Generate Fake Address
                .RuleFor(o => o.ShippingAddress, f => f.Address.StreetAddress())
                //Get Fake Items
                .RuleFor(o => o.Items, f => GetFakeItems(f))
                //Get Amount Due
                .RuleFor(o => o.Amount_Due, (f,o) => GetAmountDue(o.Items))
                //Stamp with the current time
                .RuleFor(o => o.Timestamp, f => DateTime.UtcNow);

            return testOrder.Generate();
        }

        public static List<Item> GetFakeItems(Faker f)
        {
            List<Item> Items = new List<Item>();
            int numberOfItems = f.Random.Number(1,5);

            for(int i = 0; i < numberOfItems; i++)
            {
                var item = f.PickRandom(ItemCatalog);
                
                if(item.Name == "Jump Seat Blade Replacement Pack, Medium")
                  {
                    Items.Add(new Item(){Name = "Sasquatch Ale", Price = 6});
                    Console.WriteLine("Correlation Forced");
                  }  

                Items.Add(item);
            }

            return Items;
        }
        public static int GetAmountDue(List<Item> items)
        {
            int total = 0;

            foreach(var item in items)
            {
                total += item.Price;
            }

            return total;
        }
    }
}