using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO {

    public enum AuroraResourceType {
        UNKNOWN = 0,
        BMP = 1,
        MVE = 2,
        TGA = 3,
        WAV = 4,
        PLT = 6,
        INI = 7,
        BMU = 8,
        MPG = 9,
        TXT = 10,
        WMA = 11,
        WMV = 12,
        XMV = 13,
        PLH = 2000,
        TEX = 2001,
        MDL = 2002,
        THG = 2003,
        FNT = 2005,
        LUA = 2007,
        SLT = 2008,
        NSS = 2009,
        NCS = 2010,
        MOD = 2011,
        ARE = 2012,
        SET = 2013,
        IFO = 2014,
        BIC = 2015,
        WOK = 2016,
        TwoDA = 2017,
        TLK = 2018,
        TXI = 2022,
        GIT = 2023,
        BTI = 2024,
        UTI = 2025,
        BTC = 2026,
        UTC = 2027,
        DLG = 2029,
        ITP = 2030,
        BTT = 2031,
        UTT = 2032,
        DDS = 2033,
        BTS = 2034,
        UTS = 2035,
        LTR = 2036,
        GFF = 2037,
        FAC = 2038,
        BTE = 2039,
        UTE = 2040,
        BTD = 2041,
        UTD = 2042,
        BTP = 2043,
        UTP = 2044,
        DFT = 2045,
        GIC = 2046,
        GUI = 2047,
        CSS = 2048,
        CCS = 2049,
        BTM = 2050,
        UTM = 2051,
        DWK = 2052,
        PWK = 2053,
        BTG = 2054,
        UTG = 2055,
        JRL = 2056,
        SAV = 2057,
        UTW = 2058,
        FourPC = 2059,
        SSF = 2060,
        HAK = 2061,
        NWM = 2062,
        BIK = 2063,
        NDB = 2064,
        PTM = 2065,
        PTT = 2066,
        NCM = 2067,
        MFX = 2068,
        MAT = 2069,
        MDB = 2070,
        SAY = 2071,
        TTF = 2072,
        TTC = 2073,
        CUT = 2074,
        KA = 2075,
        JPG = 2076,
        ICO = 2077,
        OGG = 2078,
        SPT = 2079,
        SPW = 2080,
        WFX = 2081,
        UGM = 2082,
        QDB = 2083,
        QST = 2084,
        NPC = 2085,
        SPN = 2086,
        UTX = 2087,
        MMD = 2088,
        SMM = 2089,
        UTA = 2090,
        MDE = 2091,
        MDV = 2092,
        MDA = 2093,
        MBA = 2094,
        OCT = 2095,
        BFX = 2096,
        PDB = 2097,
        TheWitcherSave = 2098,
        PVS = 2099,
        CFX = 2100,
        LUC = 2101,
        PRB = 2103,
        CAM = 2104,
        VDS = 2105,
        BIN = 2106,
        WOB = 2107,
        API = 2108,
        Properties = 2109,
        PNG = 2110,
        LYT = 3000,
        VIS = 3001,
        RIM = 3002,
        PTH = 3003,
        LIP = 3004,
        BWM = 3005,
        TXB = 3006,
        TPC = 3007,
        MDX = 3008,
        RSV = 3009,
        SIG = 3010,
        MAB = 3011,
        QST2 = 3012,
        STO = 3013,
        HEX = 3015,
        MDX2 = 3016,
        TXB2 = 3017,
        FSM = 3022,
        ART = 3023,
        AMP = 3024,
        CWA = 3025,
        BIP = 3028,
        MDB2 = 4000,
        MDA2 = 4001,
        SPT2 = 4002,
        GR2 = 4003,
        FXA = 4004,
        FXE = 4005,
        JPG2 = 4007,
        PWC = 4008,
        OneDA = 9996,
        ERF = 9997,
        BIF = 9998,
        KEY = 9999,
    }

    public static class AuroraResourceTypeExtensions {
        public static AuroraResourceType[] compiledResources() {
            return new AuroraResourceType[] {
                        AuroraResourceType.BMP,
                        AuroraResourceType.MVE,
                        AuroraResourceType.TGA,
                        AuroraResourceType.WAV,
                        AuroraResourceType.PLT,
                        AuroraResourceType.INI,
                        AuroraResourceType.BMU,
                        AuroraResourceType.MPG,
                        AuroraResourceType.WMA,
                        AuroraResourceType.WMV,
                        AuroraResourceType.XMV,
                        AuroraResourceType.PLH,
                        AuroraResourceType.TEX,
                        AuroraResourceType.MDL,
                        AuroraResourceType.THG,
                        AuroraResourceType.FNT,
                        AuroraResourceType.LUA,
                        AuroraResourceType.SLT,
                        AuroraResourceType.NSS,
                        AuroraResourceType.NCS,
                        AuroraResourceType.MOD,
                        AuroraResourceType.ARE,
                        AuroraResourceType.SET,
                        AuroraResourceType.IFO,
                        AuroraResourceType.BIC,
                        AuroraResourceType.WOK,
                        AuroraResourceType.TwoDA,
                        AuroraResourceType.TLK,
                        AuroraResourceType.TXI,
                        AuroraResourceType.GIT,
                        AuroraResourceType.BTI,
                        AuroraResourceType.UTI,
                        AuroraResourceType.BTC,
                        AuroraResourceType.UTC,
                        AuroraResourceType.DLG,
                        AuroraResourceType.ITP,
                        AuroraResourceType.BTT,
                        AuroraResourceType.UTT,
                        AuroraResourceType.DDS,
                        AuroraResourceType.BTS,
                        AuroraResourceType.UTS,
                        AuroraResourceType.LTR,
                        AuroraResourceType.GFF,
                        AuroraResourceType.FAC,
                        AuroraResourceType.BTE,
                        AuroraResourceType.UTE,
                        AuroraResourceType.BTD,
                        AuroraResourceType.UTD,
                        AuroraResourceType.BTP,
                        AuroraResourceType.UTP,
                        AuroraResourceType.DFT,
                        AuroraResourceType.GIC,
                        AuroraResourceType.GUI,
                        AuroraResourceType.CSS,
                        AuroraResourceType.CCS,
                        AuroraResourceType.BTM,
                        AuroraResourceType.UTM,
                        AuroraResourceType.DWK,
                        AuroraResourceType.PWK,
                        AuroraResourceType.BTG,
                        AuroraResourceType.UTG,
                        AuroraResourceType.JRL,
                        AuroraResourceType.SAV,
                        AuroraResourceType.UTW,
                        AuroraResourceType.FourPC,
                        AuroraResourceType.SSF,
                        AuroraResourceType.HAK,
                        AuroraResourceType.NWM,
                        AuroraResourceType.BIK,
                        AuroraResourceType.NDB,
                        AuroraResourceType.PTM,
                        AuroraResourceType.PTT,
                        AuroraResourceType.NCM,
                        AuroraResourceType.MFX,
                        AuroraResourceType.MAT,
                        AuroraResourceType.MDB,
                        AuroraResourceType.SAY,
                        AuroraResourceType.TTF,
                        AuroraResourceType.TTC,
                        AuroraResourceType.CUT,
                        AuroraResourceType.KA,
                        AuroraResourceType.JPG,
                        AuroraResourceType.ICO,
                        AuroraResourceType.OGG,
                        AuroraResourceType.SPT,
                        AuroraResourceType.SPW,
                        AuroraResourceType.WFX,
                        AuroraResourceType.UGM,
                        AuroraResourceType.QDB,
                        AuroraResourceType.QST,
                        AuroraResourceType.NPC,
                        AuroraResourceType.SPN,
                        AuroraResourceType.UTX,
                        AuroraResourceType.MMD,
                        AuroraResourceType.SMM,
                        AuroraResourceType.UTA,
                        AuroraResourceType.MDE,
                        AuroraResourceType.MDV,
                        AuroraResourceType.MDA,
                        AuroraResourceType.MBA,
                        AuroraResourceType.OCT,
                        AuroraResourceType.BFX,
                        AuroraResourceType.PDB,
                        AuroraResourceType.TheWitcherSave,
                        AuroraResourceType.PVS,
                        AuroraResourceType.CFX,
                        AuroraResourceType.LUC,
                        AuroraResourceType.PRB,
                        AuroraResourceType.CAM,
                        AuroraResourceType.VDS,
                        AuroraResourceType.BIN,
                        AuroraResourceType.WOB,
                        AuroraResourceType.API,
                        AuroraResourceType.Properties,
                        AuroraResourceType.PNG,
                        AuroraResourceType.LYT,
                        AuroraResourceType.VIS,
                        AuroraResourceType.RIM,
                        AuroraResourceType.PTH,
                        AuroraResourceType.LIP,
                        AuroraResourceType.BWM,
                        AuroraResourceType.TXB,
                        AuroraResourceType.TPC,
                        AuroraResourceType.MDX,
                        AuroraResourceType.RSV,
                        AuroraResourceType.SIG,
                        AuroraResourceType.MAB,
                        AuroraResourceType.QST2,
                        AuroraResourceType.STO,
                        AuroraResourceType.HEX,
                        AuroraResourceType.MDX2,
                        AuroraResourceType.TXB2,
                        AuroraResourceType.FSM,
                        AuroraResourceType.ART,
                        AuroraResourceType.AMP,
                        AuroraResourceType.CWA,
                        AuroraResourceType.BIP,
                        AuroraResourceType.MDB2,
                        AuroraResourceType.MDA2,
                        AuroraResourceType.SPT2,
                        AuroraResourceType.GR2,
                        AuroraResourceType.FXA,
                        AuroraResourceType.FXE,
                        AuroraResourceType.JPG2,
                        AuroraResourceType.PWC,
                        AuroraResourceType.OneDA,
                        AuroraResourceType.ERF,
                        AuroraResourceType.BIF,
                        AuroraResourceType.KEY,
            };
        }

        public static string stringValue(this AuroraResourceType resourceType) {
            switch (resourceType) {
                case AuroraResourceType.UNKNOWN: return "unknown";
                case AuroraResourceType.BMP: return "bmp";
                case AuroraResourceType.TGA: return "tga";
                case AuroraResourceType.WAV: return "wav";
                case AuroraResourceType.PLT: return "plt";
                case AuroraResourceType.INI: return "ini";
                case AuroraResourceType.TXT: return "txt";
                case AuroraResourceType.MDL: return "mdl";
                case AuroraResourceType.NCS: return "ncs";
                case AuroraResourceType.ARE: return "are";
                case AuroraResourceType.SET: return "set";
                case AuroraResourceType.IFO: return "ifo";
                case AuroraResourceType.BIC: return "bic";
                case AuroraResourceType.WOK: return "wok";
                case AuroraResourceType.TwoDA: return "2da";
                case AuroraResourceType.TXI: return "txi";
                case AuroraResourceType.GIT: return "git";
                case AuroraResourceType.UTI: return "uti";
                case AuroraResourceType.UTC: return "utc";
                case AuroraResourceType.DLG: return "dlg";
                case AuroraResourceType.ITP: return "itp";
                case AuroraResourceType.UTT: return "utt";
                case AuroraResourceType.DDS: return "dds";
                case AuroraResourceType.UTS: return "uts";
                case AuroraResourceType.LTR: return "ltr";
                case AuroraResourceType.GFF: return "gff";
                case AuroraResourceType.FAC: return "fac";
                case AuroraResourceType.UTE: return "ute";
                case AuroraResourceType.UTD: return "utd";
                case AuroraResourceType.UTP: return "utp";
                case AuroraResourceType.DFT: return "dft";
                case AuroraResourceType.GIC: return "gic";
                case AuroraResourceType.GUI: return "gui";
                case AuroraResourceType.UTM: return "utm";
                case AuroraResourceType.DWK: return "dwk";
                case AuroraResourceType.PWK: return "pwk";
                case AuroraResourceType.JRL: return "jrl";
                case AuroraResourceType.UTW: return "utw";
                case AuroraResourceType.SSF: return "ssf";
                case AuroraResourceType.NDB: return "ndb";
                case AuroraResourceType.PTM: return "ptm";
                case AuroraResourceType.PTT: return "ptt";
                case AuroraResourceType.MDX: return "mdx";
                case AuroraResourceType.LYT: return "lyt";
                case AuroraResourceType.VIS: return "vis";
                case AuroraResourceType.BTC: return "btc";
                case AuroraResourceType.TPC: return "tpc";
                case AuroraResourceType.LIP: return "lip";
                case AuroraResourceType.KEY: return "key";
                case AuroraResourceType.MOD: return "mod";
                case AuroraResourceType.ERF: return "erf";
                case AuroraResourceType.SAV: return "sav";
                case AuroraResourceType.RIM: return "rim";
                case AuroraResourceType.TLK: return "tlk";
                default: return null;
            }
        }

        public static AuroraResourceType toAuroraResourceType(this string s) {
            if (AuroraResourceType.BMP.stringValue() == s) { return AuroraResourceType.BMP; }
            else if (AuroraResourceType.TGA.stringValue() == s) { return AuroraResourceType.TGA; }
            else if (AuroraResourceType.WAV.stringValue() == s) { return AuroraResourceType.WAV; }
            else if (AuroraResourceType.PLT.stringValue() == s) { return AuroraResourceType.PLT; }
            else if (AuroraResourceType.INI.stringValue() == s) { return AuroraResourceType.INI; }
            else if (AuroraResourceType.TXT.stringValue() == s) { return AuroraResourceType.TXT; }
            else if (AuroraResourceType.MDL.stringValue() == s) { return AuroraResourceType.MDL; }
            else if (AuroraResourceType.NSS.stringValue() == s) { return AuroraResourceType.NSS; }
            else if (AuroraResourceType.NCS.stringValue() == s) { return AuroraResourceType.NCS; }
            else if (AuroraResourceType.ARE.stringValue() == s) { return AuroraResourceType.ARE; }
            else if (AuroraResourceType.SET.stringValue() == s) { return AuroraResourceType.SET; }
            else if (AuroraResourceType.IFO.stringValue() == s) { return AuroraResourceType.IFO; }
            else if (AuroraResourceType.BIC.stringValue() == s) { return AuroraResourceType.BIC; }
            else if (AuroraResourceType.WOK.stringValue() == s) { return AuroraResourceType.WOK; }
            else if (AuroraResourceType.TwoDA.stringValue() == s) { return AuroraResourceType.TwoDA; }
            else if (AuroraResourceType.TXI.stringValue() == s) { return AuroraResourceType.TXI; }
            else if (AuroraResourceType.GIT.stringValue() == s) { return AuroraResourceType.GIT; }
            else if (AuroraResourceType.UTI.stringValue() == s) { return AuroraResourceType.UTI; }
            else if (AuroraResourceType.UTC.stringValue() == s) { return AuroraResourceType.UTC; }
            else if (AuroraResourceType.DLG.stringValue() == s) { return AuroraResourceType.DLG; }
            else if (AuroraResourceType.ITP.stringValue() == s) { return AuroraResourceType.ITP; }
            else if (AuroraResourceType.UTT.stringValue() == s) { return AuroraResourceType.UTT; }
            else if (AuroraResourceType.DDS.stringValue() == s) { return AuroraResourceType.DDS; }
            else if (AuroraResourceType.UTS.stringValue() == s) { return AuroraResourceType.UTS; }
            else if (AuroraResourceType.LTR.stringValue() == s) { return AuroraResourceType.LTR; }
            else if (AuroraResourceType.GFF.stringValue() == s) { return AuroraResourceType.GFF; }
            else if (AuroraResourceType.FAC.stringValue() == s) { return AuroraResourceType.FAC; }
            else if (AuroraResourceType.UTE.stringValue() == s) { return AuroraResourceType.UTE; }
            else if (AuroraResourceType.UTD.stringValue() == s) { return AuroraResourceType.UTD; }
            else if (AuroraResourceType.UTP.stringValue() == s) { return AuroraResourceType.UTP; }
            else if (AuroraResourceType.DFT.stringValue() == s) { return AuroraResourceType.DFT; }
            else if (AuroraResourceType.GIC.stringValue() == s) { return AuroraResourceType.GIC; }
            else if (AuroraResourceType.GUI.stringValue() == s) { return AuroraResourceType.GUI; }
            else if (AuroraResourceType.UTM.stringValue() == s) { return AuroraResourceType.UTM; }
            else if (AuroraResourceType.DWK.stringValue() == s) { return AuroraResourceType.DWK; }
            else if (AuroraResourceType.PWK.stringValue() == s) { return AuroraResourceType.PWK; }
            else if (AuroraResourceType.JRL.stringValue() == s) { return AuroraResourceType.JRL; }
            else if (AuroraResourceType.UTW.stringValue() == s) { return AuroraResourceType.UTW; }
            else if (AuroraResourceType.SSF.stringValue() == s) { return AuroraResourceType.SSF; }
            else if (AuroraResourceType.NDB.stringValue() == s) { return AuroraResourceType.NDB; }
            else if (AuroraResourceType.PTM.stringValue() == s) { return AuroraResourceType.PTM; }
            else if (AuroraResourceType.PTT.stringValue() == s) { return AuroraResourceType.PTT; }
            else if (AuroraResourceType.MDX.stringValue() == s) { return AuroraResourceType.MDX; }
            else if (AuroraResourceType.LYT.stringValue() == s) { return AuroraResourceType.LYT; }
            else if (AuroraResourceType.VIS.stringValue() == s) { return AuroraResourceType.VIS; }
            else if (AuroraResourceType.BTC.stringValue() == s) { return AuroraResourceType.BTC; }
            else if (AuroraResourceType.TPC.stringValue() == s) { return AuroraResourceType.TPC; }
            else if (AuroraResourceType.LIP.stringValue() == s) { return AuroraResourceType.LIP; }
            else if (AuroraResourceType.KEY.stringValue() == s) { return AuroraResourceType.KEY; }
            else if (AuroraResourceType.MOD.stringValue() == s) { return AuroraResourceType.MOD; }
            else if (AuroraResourceType.ERF.stringValue() == s) { return AuroraResourceType.ERF; }
            else if (AuroraResourceType.SAV.stringValue() == s) { return AuroraResourceType.SAV; }
            else if (AuroraResourceType.RIM.stringValue() == s) { return AuroraResourceType.RIM; }
            else if (AuroraResourceType.TLK.stringValue() == s) { return AuroraResourceType.TLK; }
            else { return AuroraResourceType.UNKNOWN; }
        }

        public static bool is2da(this AuroraResourceType fileType) {
            switch (fileType) {
                case AuroraResourceType.TwoDA:
                    return true;
                default:
                    return false;
            }
        }

        public static bool isResourceCollection(this AuroraResourceType fileType) {
            switch (fileType) {
                case AuroraResourceType.ERF:
                case AuroraResourceType.MOD:
                case AuroraResourceType.RIM:
                case AuroraResourceType.SAV:
                case AuroraResourceType.HAK:
                    return true;
                default:
                    return false;
            }
        }

        public static bool isGFF(this AuroraResourceType fileType) {
            switch (fileType) {
                case AuroraResourceType.ARE:
                case AuroraResourceType.IFO:
                case AuroraResourceType.BIC:
                case AuroraResourceType.WOK:
                case AuroraResourceType.GIT:
                case AuroraResourceType.UTI:
                case AuroraResourceType.UTC:
                case AuroraResourceType.DLG:
                case AuroraResourceType.ITP:
                case AuroraResourceType.UTT:
                case AuroraResourceType.UTS:
                case AuroraResourceType.GFF:
                case AuroraResourceType.FAC:
                case AuroraResourceType.UTE:
                case AuroraResourceType.UTD:
                case AuroraResourceType.UTP:
                case AuroraResourceType.GIC:
                case AuroraResourceType.GUI:
                case AuroraResourceType.UTM:
                case AuroraResourceType.JRL:
                case AuroraResourceType.UTW:
                case AuroraResourceType.PTM:
                case AuroraResourceType.PTT:
                    return true;
                default:
                    return false;
            }
        }

        public static bool containsAuroraResources(String directory) {
            Regex r = new Regex(String.Format("\\.((?i){0})",
                String.Join("|", compiledResources()
                .Select(res => res.stringValue())
                .Where(res => res != null)
                )));
            String[] files = Directory.GetFiles(directory, "*", SearchOption.TopDirectoryOnly)
            .Where(file => r.Matches(file).Count > 0).ToArray();
            return files.Length > 0;
        }
    }
}
