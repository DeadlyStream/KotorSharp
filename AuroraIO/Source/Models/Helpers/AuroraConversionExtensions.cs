using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuroraIO.Models;

namespace AuroraIO {
    internal static class AuroraConversionExtensions {

        internal static Array2D toArray2D(this AuroraResource resource) {
            return resource.toSpecificAuroraType<Array2D>();
        }

        internal static GFFObject toGFFObject(this AuroraResource resource) {
            return resource.toSpecificAuroraType<GFFObject>();
        }

        internal static ItemBlueprintObject toUTI(this AuroraResource resource) {
            return resource.toSpecificAuroraType<ItemBlueprintObject>();
        }

        private static T toSpecificAuroraType<T>(this AuroraResource resource) where T: AuroraResource {
            if (resource is T) {
                return (T)resource;
            } else {
                return default(T);
            }
        }
    }
}
