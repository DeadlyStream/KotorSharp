using AuroraIO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models._2da {
    public static class Array2DASCIIOutputExtensions {
        public static string asciiOutput(this Array2D array) {
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Join("\t", array.columns));
            sb.Append("\n");
            sb.Append(String.Join("\n", array.Select(r => {
                return String.Join("\t", r.rowValues.Select(value => value).ToArray());
            }).ToArray()));
            return sb.ToString();
        }
    }
}
