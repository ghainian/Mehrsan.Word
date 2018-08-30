using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mehrsan.Common
{
    public enum Languages
    {
        Danish = 1,
        English = 2,
        Persian = 3,
    }

    public class Layers
    {
        public static string UI { get; } = "UI";
        public static string Business { get; } = "Business";
        public static string DAL { get; } = "DAL";

    }
}
