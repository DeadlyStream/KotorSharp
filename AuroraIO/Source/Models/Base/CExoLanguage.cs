using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AuroraIO.Source.Models.Base
{
    public enum CExoLanguage
    {
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

    public static class CExoLanguageExtensions
    {
        public static string stringValue(this CExoLanguage languageID)
        {
            switch (languageID)
            {
                case CExoLanguage.EnglishMale: return "englishmale";
                case CExoLanguage.EnglishFemale: return "englishfemale";
                case CExoLanguage.FrenchMale: return "frenchmale";
                case CExoLanguage.FrenchFemale: return "frenchfemale";
                case CExoLanguage.GermanMale: return "germanmale";
                case CExoLanguage.GermanFemale: return "germanfemale";
                case CExoLanguage.ItalianMale: return "italianmale";
                case CExoLanguage.ItalianFemale: return "italianfemale";
                case CExoLanguage.SpanishMale: return "spanishmale";
                case CExoLanguage.SpanishFemale: return "spanishfemale";
                case CExoLanguage.PolishMale: return "polishmale";
                case CExoLanguage.PolishFemale: return "polishfemale";
                case CExoLanguage.KoreanMale: return "koreanmale";
                case CExoLanguage.KoreanFemale: return "koreanfemale";
                case CExoLanguage.ChineseTraditionalMale: return "chinesetraditionalmale";
                case CExoLanguage.ChineseTraditionalFemale: return "chinesetraditionalfemale";
                case CExoLanguage.ChineseSimplifiedMale: return "chinesesimplifiedmale";
                case CExoLanguage.ChineseSimplifiedFemale: return "chinesesimplifiedfemale";
                case CExoLanguage.JapaneseMale: return "japanesemale";
                case CExoLanguage.JapaneseFemale: return "japanesefemale";
                default: return null;
            }
        }
    }
}
