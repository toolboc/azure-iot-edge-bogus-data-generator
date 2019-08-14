using System;

namespace IoTEdgeBogusDataGenerator.Models
{
   public class VehicleData
   {
      public int Id { get; set; }
      public int Speed { get; set; } 
      public string CurrentLocation { get; set; }
      public string Destination { get; set; }
      public string Ip { get; set; }
      public string Token { get; set; }
      public DateTime Timestamp { get; set; } 
   }

}