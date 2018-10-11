// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using Bogus;
using Bogus.DataSets;
using IoTEdgeBogusDataGenerator.Models;

namespace IoTEdgeBogusDataGenerator
{
    public class BogusDataFactory
    {
        public static Object CreateBogusData(int counter)
        {
            //Set the randomzier seed if you wish to generate repeatable data sets.
            Randomizer.Seed = new Random(counter);

            var fruit = new[] { "apple", "banana", "orange", "strawberry", "kiwi" };

            var testOrder = new Faker<Order>()
                //Ensure all properties have rules. By default, StrictMode is false
                //Set a global policy by using Faker.DefaultStrictMode
                .StrictMode(true)
                //OrderId is deterministic
                .RuleFor(o => o.OrderId, f => counter)
                //Pick some fruit from a basket
                .RuleFor(o => o.Item, f => f.PickRandom(fruit))
                //A random quantity from 1 to 10
                .RuleFor(o => o.Quantity, f => f.Random.Number(1, 10));

            return testOrder.Generate();
        }
    }
}