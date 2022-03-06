using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AuroraIO.Models;

namespace AuroraIO {
    public class ItemBlueprintObject {
        //private Dictionary<string, GFFFieldType> valueMap = new Dictionary<string, GFFFieldType>{
        //                                                { "TemplateResRef", GFFFieldType.RESREF },
        //                                                { "BaseItem", GFFFieldType.INT },
        //                                                { "LocalizedName", GFFFieldType.CEXOLOCSTRING },
        //                                                { "Description", GFFFieldType.CEXOLOCSTRING },
        //                                                { "DescIdentified", GFFFieldType.CEXOLOCSTRING },
        //                                                { "Tag", GFFFieldType.CEXOSTRING },
        //                                                { "Charges", GFFFieldType.BYTE },
        //                                                { "Cost", GFFFieldType.DWORD },
        //                                                { "Stolen", GFFFieldType.BYTE },
        //                                                { "StackSize", GFFFieldType.WORD },
        //                                                { "Plot", GFFFieldType.BYTE },
        //                                                { "AddCost", GFFFieldType.DWORD },
        //                                                { "Identified", GFFFieldType.BYTE },
        //                                                { "UpgradeLevel", GFFFieldType.BYTE },
        //                                                { "ModelVariation", GFFFieldType.BYTE },
        //                                                { "PropertiesList", GFFFieldType.LIST },
        //                                                { "PaletteID", GFFFieldType.BYTE },
        //                                                { "Comment", GFFFieldType.CEXOSTRING },
        //                                                { "KTInfoVersion", GFFFieldType.CEXOSTRING },
        //                                                { "KTInfoDate", GFFFieldType.CEXOSTRING },
        //                                                { "KTGameVerIndex", GFFFieldType.INT }
        //                                            };

        //public ItemBlueprintObject(AuroraResourceType fileType) {
        //}

        public void addProperty(ItemBlueprintPropertyInfo info) {
        }

        public void removeProperty() {

        }

    }

    public class ItemBlueprintPropertyInfo {
        public int costOptionRow;
        public int costValue;
        public int param1;
        public int param1Index;
        public int propertyValue;
        public int subtypeValue;

        public ItemBlueprintPropertyInfo(int propertyValue, int subtypeValue, int costOptionRow, int costValue, int param1Index, int param1) {
            this.propertyValue = propertyValue;
            this.subtypeValue = subtypeValue;
            this.costOptionRow = costOptionRow;
            this.costValue = costValue;
            this.param1Index = param1Index;
            this.param1 = param1;
        }
    }
}
