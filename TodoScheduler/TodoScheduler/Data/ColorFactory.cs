using System.Collections.Generic;

namespace TodoScheduler.Data
{
    public class HexColor
    {
        public string Name { get; set; }
        public string HexValue { get; set; }
    }

    public static class ColorFactory
    {
        public static IEnumerable<HexColor> Colors { get; private set; } = new List<HexColor>()
        {
            new HexColor() { Name = "Default", HexValue = "#7635EB" },
            new HexColor() { Name = "Cyan", HexValue = "#607D8B" },
            new HexColor() { Name = "Dark Purple", HexValue = "#673AB7" },
            new HexColor() { Name = "Grey", HexValue = "#9E9E9E" },
            new HexColor() { Name = "Lime", HexValue = "#CDDC39" },
            new HexColor() { Name = "Pink", HexValue = "#E91E63" },
            new HexColor() { Name = "Red", HexValue = "#D32F2F" },
            new HexColor() { Name = "Dark Orange", HexValue = "#FF5722" },
            new HexColor() { Name = "Green", HexValue = "#4CAF50" },
            new HexColor() { Name = "Yellow", HexValue = "#FFEB3B" }
        };
    }
}
