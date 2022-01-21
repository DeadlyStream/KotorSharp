using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO {
    public enum GFFLanguage {
        Undefined = -1,
        EnglishMale = 0,
        EnglishFemale = 1,
        FrenchMale = 2,
        FrenchFemale = 3,
        GermanMale = 4,
        GermanFemale = 5,
        ItalianMale = 6,
        ItalianFemale = 7,
        SpanishMale = 8,
        SpanishFemale = 9,
        PolishMale = 10,
        PolishFemale = 11,
        KoreanMale = 256,
        KoreanFemale = 257,
        ChineseTraditionalMale = 258,
        ChineseTraditionalFemale = 259,
        ChineseSimplifiedMale = 260,
        ChineseSimplifiedFemale = 261,
        JapaneseMale = 262,
        JapaneseFemale = 263
    }

    public static class GFFLanguageExtensions {
        public static string stringValue(this GFFLanguage languageID) {
            switch (languageID) {
                case GFFLanguage.EnglishMale: return "englishmale";
                case GFFLanguage.EnglishFemale: return "englishfemale";
                case GFFLanguage.FrenchMale: return "frenchmale";
                case GFFLanguage.FrenchFemale: return "frenchfemale";
                case GFFLanguage.GermanMale: return "germanmale";
                case GFFLanguage.GermanFemale: return "germanfemale";
                case GFFLanguage.ItalianMale: return "italianmale";
                case GFFLanguage.ItalianFemale: return "italianfemale";
                case GFFLanguage.SpanishMale: return "spanishmale";
                case GFFLanguage.SpanishFemale: return "spanishfemale";
                case GFFLanguage.PolishMale: return "polishmale";
                case GFFLanguage.PolishFemale: return "polishfemale";
                case GFFLanguage.KoreanMale: return "koreanmale";
                case GFFLanguage.KoreanFemale: return "koreanfemale";
                case GFFLanguage.ChineseTraditionalMale: return "chinesetraditionalmale";
                case GFFLanguage.ChineseTraditionalFemale: return "chinesetraditionalfemale";
                case GFFLanguage.ChineseSimplifiedMale: return "chinesesimplifiedmale";
                case GFFLanguage.ChineseSimplifiedFemale: return "chinesesimplifiedfemale";
                case GFFLanguage.JapaneseMale: return "japanesemale";
                case GFFLanguage.JapaneseFemale: return "japanesefemale";
                default: return null;
            }
        }
        public static GFFLanguage toGFFLanguage(this string s) {
            if (GFFLanguage.EnglishMale.stringValue() == s) { return GFFLanguage.EnglishMale; }
            else if (GFFLanguage.EnglishFemale.stringValue() == s) { return GFFLanguage.EnglishFemale; }
            else if (GFFLanguage.FrenchMale.stringValue() == s) { return GFFLanguage.FrenchMale; }
            else if (GFFLanguage.FrenchFemale.stringValue() == s) { return GFFLanguage.FrenchFemale; }
            else if (GFFLanguage.GermanMale.stringValue() == s) { return GFFLanguage.GermanMale; }
            else if (GFFLanguage.GermanFemale.stringValue() == s) { return GFFLanguage.GermanFemale; }
            else if (GFFLanguage.ItalianMale.stringValue() == s) { return GFFLanguage.ItalianMale; }
            else if (GFFLanguage.ItalianFemale.stringValue() == s) { return GFFLanguage.ItalianFemale; }
            else if (GFFLanguage.SpanishMale.stringValue() == s) { return GFFLanguage.SpanishMale; }
            else if (GFFLanguage.SpanishFemale.stringValue() == s) { return GFFLanguage.SpanishFemale; }
            else if (GFFLanguage.PolishMale.stringValue() == s) { return GFFLanguage.PolishMale; }
            else if (GFFLanguage.PolishFemale.stringValue() == s) { return GFFLanguage.PolishFemale; }
            else if (GFFLanguage.KoreanMale.stringValue() == s) { return GFFLanguage.KoreanMale; }
            else if (GFFLanguage.KoreanFemale.stringValue() == s) { return GFFLanguage.KoreanFemale; }
            else if (GFFLanguage.ChineseTraditionalMale.stringValue() == s) { return GFFLanguage.ChineseTraditionalMale; }
            else if (GFFLanguage.ChineseTraditionalFemale.stringValue() == s) { return GFFLanguage.ChineseTraditionalFemale; }
            else if (GFFLanguage.ChineseSimplifiedMale.stringValue() == s) { return GFFLanguage.ChineseSimplifiedMale; }
            else if (GFFLanguage.ChineseSimplifiedFemale.stringValue() == s) { return GFFLanguage.ChineseSimplifiedFemale; }
            else if (GFFLanguage.JapaneseMale.stringValue() == s) { return GFFLanguage.JapaneseMale; }
            else if (GFFLanguage.JapaneseFemale.stringValue() == s) { return GFFLanguage.JapaneseFemale; }
            else { return GFFLanguage.Undefined; }
        }
    }
}
