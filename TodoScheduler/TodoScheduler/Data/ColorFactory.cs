using System.Collections.Generic;
using System.Linq;
using TodoScheduler.Enums;

namespace TodoScheduler.Data
{
    public class HexColor
    {
        public string Name { get; set; }
        public string HexValue { get; set; }
        public ColorType ColorType { get; set; }
    }

    public static class ColorFactory
    {
        public static IEnumerable<HexColor> Colors { get; private set; } = new List<HexColor>()
        {
            new HexColor() { Name = "Default", HexValue = "#7635EB", ColorType = ColorType.Dark },
            new HexColor() { Name = "Cyan", HexValue = "#607D8B", ColorType = ColorType.Dark  },
            new HexColor() { Name = "Dark Purple", HexValue = "#673AB7", ColorType = ColorType.Dark },
            new HexColor() { Name = "Grey", HexValue = "#9E9E9E", ColorType = ColorType.Light },
            new HexColor() { Name = "Lime", HexValue = "#CDDC39", ColorType = ColorType.Light },
            new HexColor() { Name = "Pink", HexValue = "#E91E63", ColorType = ColorType.Dark },
            new HexColor() { Name = "Red", HexValue = "#D32F2F", ColorType = ColorType.Dark },
            new HexColor() { Name = "Dark Orange", HexValue = "#FF5722", ColorType = ColorType.Light },
            new HexColor() { Name = "Green", HexValue = "#4CAF50", ColorType = ColorType.Light },
            new HexColor() { Name = "Yellow", HexValue = "#FFEB3B", ColorType = ColorType.Light }
        };

        public static HexColor GetColorTypeByHexValue(string hexValue)
        {
            return Colors.Where(h => h.HexValue.ToLower() == hexValue.ToLower()).FirstOrDefault();
        }
    }
}
