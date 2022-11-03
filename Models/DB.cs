using System.Collections.Generic;
using SystemMonitoring.Controls;
using SystemMonitoring.Models.Entity;

namespace SystemMonitoring.Models
{
    internal class Db
    {
        public static string DistrictName { get; set; } 
        public static double SizeWindow { get; set; }
        public static string Path { get; set; }
        public static string ConfigName { get; set; } = "settings";
        public static string ConfigFormat { get; set; } = "json";
        public static string DirectoryName { get; set; } = "SystemMonitoring";
        public static Seed SelectSeeding { get; set; }
        public static List<SensorDetails> Child { get; set; }
    }
}