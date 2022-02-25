using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AuroraIO.Models.Base {

    public struct AuroraResourceType {
        public static readonly AuroraResourceType UNKNOWN = new AuroraResourceType(AuroraResourceID.UNKNOWN, "UNKNOWN");
        public static readonly AuroraResourceType BMP = new AuroraResourceType(AuroraResourceID.BMP, "BMP");
        public static readonly AuroraResourceType MVE = new AuroraResourceType(AuroraResourceID.MVE, "MVE");
        public static readonly AuroraResourceType TGA = new AuroraResourceType(AuroraResourceID.TGA, "TGA");
        public static readonly AuroraResourceType WAV = new AuroraResourceType(AuroraResourceID.WAV, "WAV");
        public static readonly AuroraResourceType PLT = new AuroraResourceType(AuroraResourceID.PLT, "PLT");
        public static readonly AuroraResourceType INI = new AuroraResourceType(AuroraResourceID.INI, "INI");
        public static readonly AuroraResourceType BMU = new AuroraResourceType(AuroraResourceID.BMU, "BMU");
        public static readonly AuroraResourceType MPG = new AuroraResourceType(AuroraResourceID.MPG, "MPG");
        public static readonly AuroraResourceType TXT = new AuroraResourceType(AuroraResourceID.TXT, "TXT");
        public static readonly AuroraResourceType WMA = new AuroraResourceType(AuroraResourceID.WMA, "WMA");
        public static readonly AuroraResourceType WMV = new AuroraResourceType(AuroraResourceID.WMV, "WMV");
        public static readonly AuroraResourceType XMV = new AuroraResourceType(AuroraResourceID.XMV, "XMV");
        public static readonly AuroraResourceType PLH = new AuroraResourceType(AuroraResourceID.PLH, "PLH");
        public static readonly AuroraResourceType TEX = new AuroraResourceType(AuroraResourceID.TEX, "TEX");
        public static readonly AuroraResourceType MDL = new AuroraResourceType(AuroraResourceID.MDL, "MDL");
        public static readonly AuroraResourceType THG = new AuroraResourceType(AuroraResourceID.THG, "THG");
        public static readonly AuroraResourceType FNT = new AuroraResourceType(AuroraResourceID.FNT, "FNT");
        public static readonly AuroraResourceType LUA = new AuroraResourceType(AuroraResourceID.LUA, "LUA");
        public static readonly AuroraResourceType SLT = new AuroraResourceType(AuroraResourceID.SLT, "SLT");
        public static readonly AuroraResourceType NSS = new AuroraResourceType(AuroraResourceID.NSS, "NSS");
        public static readonly AuroraResourceType NCS = new AuroraResourceType(AuroraResourceID.NCS, "NCS");
        public static readonly AuroraResourceType MOD = new AuroraResourceType(AuroraResourceID.MOD, "MOD");
        public static readonly AuroraResourceType ARE = new AuroraResourceType(AuroraResourceID.ARE, "ARE");
        public static readonly AuroraResourceType SET = new AuroraResourceType(AuroraResourceID.SET, "SET");
        public static readonly AuroraResourceType IFO = new AuroraResourceType(AuroraResourceID.IFO, "IFO");
        public static readonly AuroraResourceType BIC = new AuroraResourceType(AuroraResourceID.BIC, "BIC");
        public static readonly AuroraResourceType WOK = new AuroraResourceType(AuroraResourceID.WOK, "WOK");
        public static readonly AuroraResourceType TwoDA = new AuroraResourceType(AuroraResourceID.TwoDA, "TwoDA");
        public static readonly AuroraResourceType TLK = new AuroraResourceType(AuroraResourceID.TLK, "TLK");
        public static readonly AuroraResourceType TXI = new AuroraResourceType(AuroraResourceID.TXI, "TXI");
        public static readonly AuroraResourceType GIT = new AuroraResourceType(AuroraResourceID.GIT, "GIT");
        public static readonly AuroraResourceType BTI = new AuroraResourceType(AuroraResourceID.BTI, "BTI");
        public static readonly AuroraResourceType UTI = new AuroraResourceType(AuroraResourceID.UTI, "UTI");
        public static readonly AuroraResourceType BTC = new AuroraResourceType(AuroraResourceID.BTC, "BTC");
        public static readonly AuroraResourceType UTC = new AuroraResourceType(AuroraResourceID.UTC, "UTC");
        public static readonly AuroraResourceType DLG = new AuroraResourceType(AuroraResourceID.DLG, "DLG");
        public static readonly AuroraResourceType ITP = new AuroraResourceType(AuroraResourceID.ITP, "ITP");
        public static readonly AuroraResourceType BTT = new AuroraResourceType(AuroraResourceID.BTT, "BTT");
        public static readonly AuroraResourceType UTT = new AuroraResourceType(AuroraResourceID.UTT, "UTT");
        public static readonly AuroraResourceType DDS = new AuroraResourceType(AuroraResourceID.DDS, "DDS");
        public static readonly AuroraResourceType BTS = new AuroraResourceType(AuroraResourceID.BTS, "BTS");
        public static readonly AuroraResourceType UTS = new AuroraResourceType(AuroraResourceID.UTS, "UTS");
        public static readonly AuroraResourceType LTR = new AuroraResourceType(AuroraResourceID.LTR, "LTR");
        public static readonly AuroraResourceType GFF = new AuroraResourceType(AuroraResourceID.GFF, "GFF");
        public static readonly AuroraResourceType FAC = new AuroraResourceType(AuroraResourceID.FAC, "FAC");
        public static readonly AuroraResourceType BTE = new AuroraResourceType(AuroraResourceID.BTE, "BTE");
        public static readonly AuroraResourceType UTE = new AuroraResourceType(AuroraResourceID.UTE, "UTE");
        public static readonly AuroraResourceType BTD = new AuroraResourceType(AuroraResourceID.BTD, "BTD");
        public static readonly AuroraResourceType UTD = new AuroraResourceType(AuroraResourceID.UTD, "UTD");
        public static readonly AuroraResourceType BTP = new AuroraResourceType(AuroraResourceID.BTP, "BTP");
        public static readonly AuroraResourceType UTP = new AuroraResourceType(AuroraResourceID.UTP, "UTP");
        public static readonly AuroraResourceType DFT = new AuroraResourceType(AuroraResourceID.DFT, "DFT");
        public static readonly AuroraResourceType GIC = new AuroraResourceType(AuroraResourceID.GIC, "GIC");
        public static readonly AuroraResourceType GUI = new AuroraResourceType(AuroraResourceID.GUI, "GUI");
        public static readonly AuroraResourceType CSS = new AuroraResourceType(AuroraResourceID.CSS, "CSS");
        public static readonly AuroraResourceType CCS = new AuroraResourceType(AuroraResourceID.CCS, "CCS");
        public static readonly AuroraResourceType BTM = new AuroraResourceType(AuroraResourceID.BTM, "BTM");
        public static readonly AuroraResourceType UTM = new AuroraResourceType(AuroraResourceID.UTM, "UTM");
        public static readonly AuroraResourceType DWK = new AuroraResourceType(AuroraResourceID.DWK, "DWK");
        public static readonly AuroraResourceType PWK = new AuroraResourceType(AuroraResourceID.PWK, "PWK");
        public static readonly AuroraResourceType BTG = new AuroraResourceType(AuroraResourceID.BTG, "BTG");
        public static readonly AuroraResourceType UTG = new AuroraResourceType(AuroraResourceID.UTG, "UTG");
        public static readonly AuroraResourceType JRL = new AuroraResourceType(AuroraResourceID.JRL, "JRL");
        public static readonly AuroraResourceType SAV = new AuroraResourceType(AuroraResourceID.SAV, "SAV");
        public static readonly AuroraResourceType UTW = new AuroraResourceType(AuroraResourceID.UTW, "UTW");
        public static readonly AuroraResourceType FourPC = new AuroraResourceType(AuroraResourceID.FourPC, "4PC");
        public static readonly AuroraResourceType SSF = new AuroraResourceType(AuroraResourceID.SSF, "SSF");
        public static readonly AuroraResourceType HAK = new AuroraResourceType(AuroraResourceID.HAK, "HAK");
        public static readonly AuroraResourceType NWM = new AuroraResourceType(AuroraResourceID.NWM, "NWM");
        public static readonly AuroraResourceType BIK = new AuroraResourceType(AuroraResourceID.BIK, "BIK");
        public static readonly AuroraResourceType NDB = new AuroraResourceType(AuroraResourceID.NDB, "NDB");
        public static readonly AuroraResourceType PTM = new AuroraResourceType(AuroraResourceID.PTM, "PTM");
        public static readonly AuroraResourceType PTT = new AuroraResourceType(AuroraResourceID.PTT, "PTT");
        public static readonly AuroraResourceType NCM = new AuroraResourceType(AuroraResourceID.NCM, "NCM");
        public static readonly AuroraResourceType MFX = new AuroraResourceType(AuroraResourceID.MFX, "MFX");
        public static readonly AuroraResourceType MAT = new AuroraResourceType(AuroraResourceID.MAT, "MAT");
        public static readonly AuroraResourceType MDB = new AuroraResourceType(AuroraResourceID.MDB, "MDB");
        public static readonly AuroraResourceType SAY = new AuroraResourceType(AuroraResourceID.SAY, "SAY");
        public static readonly AuroraResourceType TTF = new AuroraResourceType(AuroraResourceID.TTF, "TTF");
        public static readonly AuroraResourceType TTC = new AuroraResourceType(AuroraResourceID.TTC, "TTC");
        public static readonly AuroraResourceType CUT = new AuroraResourceType(AuroraResourceID.CUT, "CUT");
        public static readonly AuroraResourceType KA = new AuroraResourceType(AuroraResourceID.KA, "KA");
        public static readonly AuroraResourceType JPG = new AuroraResourceType(AuroraResourceID.JPG, "JPG");
        public static readonly AuroraResourceType ICO = new AuroraResourceType(AuroraResourceID.ICO, "ICO");
        public static readonly AuroraResourceType OGG = new AuroraResourceType(AuroraResourceID.OGG, "OGG");
        public static readonly AuroraResourceType SPT = new AuroraResourceType(AuroraResourceID.SPT, "SPT");
        public static readonly AuroraResourceType SPW = new AuroraResourceType(AuroraResourceID.SPW, "SPW");
        public static readonly AuroraResourceType WFX = new AuroraResourceType(AuroraResourceID.WFX, "WFX");
        public static readonly AuroraResourceType UGM = new AuroraResourceType(AuroraResourceID.UGM, "UGM");
        public static readonly AuroraResourceType QDB = new AuroraResourceType(AuroraResourceID.QDB, "QDB");
        public static readonly AuroraResourceType QST = new AuroraResourceType(AuroraResourceID.QST, "QST");
        public static readonly AuroraResourceType NPC = new AuroraResourceType(AuroraResourceID.NPC, "NPC");
        public static readonly AuroraResourceType SPN = new AuroraResourceType(AuroraResourceID.SPN, "SPN");
        public static readonly AuroraResourceType UTX = new AuroraResourceType(AuroraResourceID.UTX, "UTX");
        public static readonly AuroraResourceType MMD = new AuroraResourceType(AuroraResourceID.MMD, "MMD");
        public static readonly AuroraResourceType SMM = new AuroraResourceType(AuroraResourceID.SMM, "SMM");
        public static readonly AuroraResourceType UTA = new AuroraResourceType(AuroraResourceID.UTA, "UTA");
        public static readonly AuroraResourceType MDE = new AuroraResourceType(AuroraResourceID.MDE, "MDE");
        public static readonly AuroraResourceType MDV = new AuroraResourceType(AuroraResourceID.MDV, "MDV");
        public static readonly AuroraResourceType MDA = new AuroraResourceType(AuroraResourceID.MDA, "MDA");
        public static readonly AuroraResourceType MBA = new AuroraResourceType(AuroraResourceID.MBA, "MBA");
        public static readonly AuroraResourceType OCT = new AuroraResourceType(AuroraResourceID.OCT, "OCT");
        public static readonly AuroraResourceType BFX = new AuroraResourceType(AuroraResourceID.BFX, "BFX");
        public static readonly AuroraResourceType PDB = new AuroraResourceType(AuroraResourceID.PDB, "PDB");
        public static readonly AuroraResourceType TheWitcherSave = new AuroraResourceType(AuroraResourceID.TheWitcherSave, "TheWitcherSave");
        public static readonly AuroraResourceType PVS = new AuroraResourceType(AuroraResourceID.PVS, "PVS");
        public static readonly AuroraResourceType CFX = new AuroraResourceType(AuroraResourceID.CFX, "CFX");
        public static readonly AuroraResourceType LUC = new AuroraResourceType(AuroraResourceID.LUC, "LUC");
        public static readonly AuroraResourceType PRB = new AuroraResourceType(AuroraResourceID.PRB, "PRB");
        public static readonly AuroraResourceType CAM = new AuroraResourceType(AuroraResourceID.CAM, "CAM");
        public static readonly AuroraResourceType VDS = new AuroraResourceType(AuroraResourceID.VDS, "VDS");
        public static readonly AuroraResourceType BIN = new AuroraResourceType(AuroraResourceID.BIN, "BIN");
        public static readonly AuroraResourceType WOB = new AuroraResourceType(AuroraResourceID.WOB, "WOB");
        public static readonly AuroraResourceType API = new AuroraResourceType(AuroraResourceID.API, "API");
        public static readonly AuroraResourceType Properties = new AuroraResourceType(AuroraResourceID.Properties, "Properties");
        public static readonly AuroraResourceType PNG = new AuroraResourceType(AuroraResourceID.PNG, "PNG");
        public static readonly AuroraResourceType LYT = new AuroraResourceType(AuroraResourceID.LYT, "LYT");
        public static readonly AuroraResourceType VIS = new AuroraResourceType(AuroraResourceID.VIS, "VIS");
        public static readonly AuroraResourceType RIM = new AuroraResourceType(AuroraResourceID.RIM, "RIM");
        public static readonly AuroraResourceType PTH = new AuroraResourceType(AuroraResourceID.PTH, "PTH");
        public static readonly AuroraResourceType LIP = new AuroraResourceType(AuroraResourceID.LIP, "LIP");
        public static readonly AuroraResourceType BWM = new AuroraResourceType(AuroraResourceID.BWM, "BWM");
        public static readonly AuroraResourceType TXB = new AuroraResourceType(AuroraResourceID.TXB, "TXB");
        public static readonly AuroraResourceType TPC = new AuroraResourceType(AuroraResourceID.TPC, "TPC");
        public static readonly AuroraResourceType MDX = new AuroraResourceType(AuroraResourceID.MDX, "MDX");
        public static readonly AuroraResourceType RSV = new AuroraResourceType(AuroraResourceID.RSV, "RSV");
        public static readonly AuroraResourceType SIG = new AuroraResourceType(AuroraResourceID.SIG, "SIG");
        public static readonly AuroraResourceType MAB = new AuroraResourceType(AuroraResourceID.MAB, "MAB");
        public static readonly AuroraResourceType QST2 = new AuroraResourceType(AuroraResourceID.QST2, "QST2");
        public static readonly AuroraResourceType STO = new AuroraResourceType(AuroraResourceID.STO, "STO");
        public static readonly AuroraResourceType HEX = new AuroraResourceType(AuroraResourceID.HEX, "HEX");
        public static readonly AuroraResourceType MDX2 = new AuroraResourceType(AuroraResourceID.MDX2, "MDX2");
        public static readonly AuroraResourceType TXB2 = new AuroraResourceType(AuroraResourceID.TXB2, "TXB2");
        public static readonly AuroraResourceType FSM = new AuroraResourceType(AuroraResourceID.FSM, "FSM");
        public static readonly AuroraResourceType ART = new AuroraResourceType(AuroraResourceID.ART, "ART");
        public static readonly AuroraResourceType AMP = new AuroraResourceType(AuroraResourceID.AMP, "AMP");
        public static readonly AuroraResourceType CWA = new AuroraResourceType(AuroraResourceID.CWA, "CWA");
        public static readonly AuroraResourceType BIP = new AuroraResourceType(AuroraResourceID.BIP, "BIP");
        public static readonly AuroraResourceType MDB2 = new AuroraResourceType(AuroraResourceID.MDB2, "MDB2");
        public static readonly AuroraResourceType MDA2 = new AuroraResourceType(AuroraResourceID.MDA2, "MDA2");
        public static readonly AuroraResourceType SPT2 = new AuroraResourceType(AuroraResourceID.SPT2, "SPT2");
        public static readonly AuroraResourceType GR2 = new AuroraResourceType(AuroraResourceID.GR2, "GR2");
        public static readonly AuroraResourceType FXA = new AuroraResourceType(AuroraResourceID.FXA, "FXA");
        public static readonly AuroraResourceType FXE = new AuroraResourceType(AuroraResourceID.FXE, "FXE");
        public static readonly AuroraResourceType JPG2 = new AuroraResourceType(AuroraResourceID.JPG2, "JPG2");
        public static readonly AuroraResourceType PWC = new AuroraResourceType(AuroraResourceID.PWC, "PWC");
        public static readonly AuroraResourceType OneDA = new AuroraResourceType(AuroraResourceID.OneDA, "1DA");
        public static readonly AuroraResourceType ERF = new AuroraResourceType(AuroraResourceID.ERF, "ERF");
        public static readonly AuroraResourceType BIF = new AuroraResourceType(AuroraResourceID.BIF, "BIF");
        public static readonly AuroraResourceType KEY = new AuroraResourceType(AuroraResourceID.KEY, "KEY");


        public readonly AuroraResourceID id;
        public readonly string stringValue;

        public AuroraResourceType(AuroraResourceID id, string value) : this() {
            this.id = id;
            this.stringValue = value;
        }

        public static implicit operator AuroraResourceType(int id) {
            switch ((AuroraResourceID)id) {
                case AuroraResourceID.BMP: return BMP;
                case AuroraResourceID.MVE: return MVE;
                case AuroraResourceID.TGA: return TGA;
                case AuroraResourceID.WAV: return WAV;
                case AuroraResourceID.PLT: return PLT;
                case AuroraResourceID.INI: return INI;
                case AuroraResourceID.BMU: return BMU;
                case AuroraResourceID.MPG: return MPG;
                case AuroraResourceID.TXT: return TXT;
                case AuroraResourceID.WMA: return WMA;
                case AuroraResourceID.WMV: return WMV;
                case AuroraResourceID.XMV: return XMV;
                case AuroraResourceID.PLH: return PLH;
                case AuroraResourceID.TEX: return TEX;
                case AuroraResourceID.MDL: return MDL;
                case AuroraResourceID.THG: return THG;
                case AuroraResourceID.FNT: return FNT;
                case AuroraResourceID.LUA: return LUA;
                case AuroraResourceID.SLT: return SLT;
                case AuroraResourceID.NSS: return NSS;
                case AuroraResourceID.NCS: return NCS;
                case AuroraResourceID.MOD: return MOD;
                case AuroraResourceID.ARE: return ARE;
                case AuroraResourceID.SET: return SET;
                case AuroraResourceID.IFO: return IFO;
                case AuroraResourceID.BIC: return BIC;
                case AuroraResourceID.WOK: return WOK;
                case AuroraResourceID.TwoDA: return TwoDA;
                case AuroraResourceID.TLK: return TLK;
                case AuroraResourceID.TXI: return TXI;
                case AuroraResourceID.GIT: return GIT;
                case AuroraResourceID.BTI: return BTI;
                case AuroraResourceID.UTI: return UTI;
                case AuroraResourceID.BTC: return BTC;
                case AuroraResourceID.UTC: return UTC;
                case AuroraResourceID.DLG: return DLG;
                case AuroraResourceID.ITP: return ITP;
                case AuroraResourceID.BTT: return BTT;
                case AuroraResourceID.UTT: return UTT;
                case AuroraResourceID.DDS: return DDS;
                case AuroraResourceID.BTS: return BTS;
                case AuroraResourceID.UTS: return UTS;
                case AuroraResourceID.LTR: return LTR;
                case AuroraResourceID.GFF: return GFF;
                case AuroraResourceID.FAC: return FAC;
                case AuroraResourceID.BTE: return BTE;
                case AuroraResourceID.UTE: return UTE;
                case AuroraResourceID.BTD: return BTD;
                case AuroraResourceID.UTD: return UTD;
                case AuroraResourceID.BTP: return BTP;
                case AuroraResourceID.UTP: return UTP;
                case AuroraResourceID.DFT: return DFT;
                case AuroraResourceID.GIC: return GIC;
                case AuroraResourceID.GUI: return GUI;
                case AuroraResourceID.CSS: return CSS;
                case AuroraResourceID.CCS: return CCS;
                case AuroraResourceID.BTM: return BTM;
                case AuroraResourceID.UTM: return UTM;
                case AuroraResourceID.DWK: return DWK;
                case AuroraResourceID.PWK: return PWK;
                case AuroraResourceID.BTG: return BTG;
                case AuroraResourceID.UTG: return UTG;
                case AuroraResourceID.JRL: return JRL;
                case AuroraResourceID.SAV: return SAV;
                case AuroraResourceID.UTW: return UTW;
                case AuroraResourceID.FourPC: return FourPC;
                case AuroraResourceID.SSF: return SSF;
                case AuroraResourceID.HAK: return HAK;
                case AuroraResourceID.NWM: return NWM;
                case AuroraResourceID.BIK: return BIK;
                case AuroraResourceID.NDB: return NDB;
                case AuroraResourceID.PTM: return PTM;
                case AuroraResourceID.PTT: return PTT;
                case AuroraResourceID.NCM: return NCM;
                case AuroraResourceID.MFX: return MFX;
                case AuroraResourceID.MAT: return MAT;
                case AuroraResourceID.MDB: return MDB;
                case AuroraResourceID.SAY: return SAY;
                case AuroraResourceID.TTF: return TTF;
                case AuroraResourceID.TTC: return TTC;
                case AuroraResourceID.CUT: return CUT;
                case AuroraResourceID.KA: return KA;
                case AuroraResourceID.JPG: return JPG;
                case AuroraResourceID.ICO: return ICO;
                case AuroraResourceID.OGG: return OGG;
                case AuroraResourceID.SPT: return SPT;
                case AuroraResourceID.SPW: return SPW;
                case AuroraResourceID.WFX: return WFX;
                case AuroraResourceID.UGM: return UGM;
                case AuroraResourceID.QDB: return QDB;
                case AuroraResourceID.QST: return QST;
                case AuroraResourceID.NPC: return NPC;
                case AuroraResourceID.SPN: return SPN;
                case AuroraResourceID.UTX: return UTX;
                case AuroraResourceID.MMD: return AuroraResourceType.MMD;
                case AuroraResourceID.SMM: return SMM;
                case AuroraResourceID.UTA: return UTA;
                case AuroraResourceID.MDE: return MDE;
                case AuroraResourceID.MDV: return MDV;
                case AuroraResourceID.MDA: return MDA;
                case AuroraResourceID.MBA: return MBA;
                case AuroraResourceID.OCT: return OCT;
                case AuroraResourceID.BFX: return BFX;
                case AuroraResourceID.PDB: return PDB;
                case AuroraResourceID.TheWitcherSave: return TheWitcherSave;
                case AuroraResourceID.PVS: return PVS;
                case AuroraResourceID.CFX: return CFX;
                case AuroraResourceID.LUC: return LUC;
                case AuroraResourceID.PRB: return PRB;
                case AuroraResourceID.CAM: return CAM;
                case AuroraResourceID.VDS: return VDS;
                case AuroraResourceID.BIN: return BIN;
                case AuroraResourceID.WOB: return WOB;
                case AuroraResourceID.API: return API;
                case AuroraResourceID.Properties: return Properties;
                case AuroraResourceID.PNG: return PNG;
                case AuroraResourceID.LYT: return LYT;
                case AuroraResourceID.VIS: return VIS;
                case AuroraResourceID.RIM: return RIM;
                case AuroraResourceID.PTH: return PTH;
                case AuroraResourceID.LIP: return LIP;
                case AuroraResourceID.BWM: return BWM;
                case AuroraResourceID.TXB: return TXB;
                case AuroraResourceID.TPC: return TPC;
                case AuroraResourceID.MDX: return MDX;
                case AuroraResourceID.RSV: return RSV;
                case AuroraResourceID.SIG: return SIG;
                case AuroraResourceID.MAB: return MAB;
                case AuroraResourceID.QST2: return QST2;
                case AuroraResourceID.STO: return STO;
                case AuroraResourceID.HEX: return HEX;
                case AuroraResourceID.MDX2: return MDX2;
                case AuroraResourceID.TXB2: return TXB2;
                case AuroraResourceID.FSM: return FSM;
                case AuroraResourceID.ART: return ART;
                case AuroraResourceID.AMP: return AMP;
                case AuroraResourceID.CWA: return CWA;
                case AuroraResourceID.BIP: return BIP;
                case AuroraResourceID.MDB2: return MDB2;
                case AuroraResourceID.MDA2: return MDA2;
                case AuroraResourceID.SPT2: return SPT2;
                case AuroraResourceID.GR2: return GR2;
                case AuroraResourceID.FXA: return FXA;
                case AuroraResourceID.FXE: return FXE;
                case AuroraResourceID.JPG2: return JPG2;
                case AuroraResourceID.PWC: return PWC;
                case AuroraResourceID.OneDA: return OneDA;
                case AuroraResourceID.ERF: return ERF;
                case AuroraResourceID.BIF: return BIF;
                case AuroraResourceID.KEY: return KEY;
                default: return AuroraResourceType.UNKNOWN;
            }
        }

        public static implicit operator AuroraResourceType(string value) {
            if ("BMP" == value) return AuroraResourceType.BMP;
            if ("MVE" == value) return AuroraResourceType.MVE;
            if ("TGA" == value) return AuroraResourceType.TGA;
            if ("WAV" == value) return AuroraResourceType.WAV;
            if ("PLT" == value) return AuroraResourceType.PLT;
            if ("INI" == value) return AuroraResourceType.INI;
            if ("BMU" == value) return AuroraResourceType.BMU;
            if ("MPG" == value) return AuroraResourceType.MPG;
            if ("TXT" == value) return AuroraResourceType.TXT;
            if ("WMA" == value) return AuroraResourceType.WMA;
            if ("WMV" == value) return AuroraResourceType.WMV;
            if ("XMV" == value) return AuroraResourceType.XMV;
            if ("PLH" == value) return AuroraResourceType.PLH;
            if ("TEX" == value) return AuroraResourceType.TEX;
            if ("MDL" == value) return AuroraResourceType.MDL;
            if ("THG" == value) return AuroraResourceType.THG;
            if ("FNT" == value) return AuroraResourceType.FNT;
            if ("LUA" == value) return AuroraResourceType.LUA;
            if ("SLT" == value) return AuroraResourceType.SLT;
            if ("NSS" == value) return AuroraResourceType.NSS;
            if ("NCS" == value) return AuroraResourceType.NCS;
            if ("MOD" == value) return AuroraResourceType.MOD;
            if ("ARE" == value) return AuroraResourceType.ARE;
            if ("SET" == value) return AuroraResourceType.SET;
            if ("IFO" == value) return AuroraResourceType.IFO;
            if ("BIC" == value) return AuroraResourceType.BIC;
            if ("WOK" == value) return AuroraResourceType.WOK;
            if ("2DA" == value) return AuroraResourceType.TwoDA;
            if ("TLK" == value) return AuroraResourceType.TLK;
            if ("TXI" == value) return AuroraResourceType.TXI;
            if ("GIT" == value) return AuroraResourceType.GIT;
            if ("BTI" == value) return AuroraResourceType.BTI;
            if ("UTI" == value) return AuroraResourceType.UTI;
            if ("BTC" == value) return AuroraResourceType.BTC;
            if ("UTC" == value) return AuroraResourceType.UTC;
            if ("DLG" == value) return AuroraResourceType.DLG;
            if ("ITP" == value) return AuroraResourceType.ITP;
            if ("BTT" == value) return AuroraResourceType.BTT;
            if ("UTT" == value) return AuroraResourceType.UTT;
            if ("DDS" == value) return AuroraResourceType.DDS;
            if ("BTS" == value) return AuroraResourceType.BTS;
            if ("UTS" == value) return AuroraResourceType.UTS;
            if ("LTR" == value) return AuroraResourceType.LTR;
            if ("GFF" == value) return AuroraResourceType.GFF;
            if ("FAC" == value) return AuroraResourceType.FAC;
            if ("BTE" == value) return AuroraResourceType.BTE;
            if ("UTE" == value) return AuroraResourceType.UTE;
            if ("BTD" == value) return AuroraResourceType.BTD;
            if ("UTD" == value) return AuroraResourceType.UTD;
            if ("BTP" == value) return AuroraResourceType.BTP;
            if ("UTP" == value) return AuroraResourceType.UTP;
            if ("DFT" == value) return AuroraResourceType.DFT;
            if ("GIC" == value) return AuroraResourceType.GIC;
            if ("GUI" == value) return AuroraResourceType.GUI;
            if ("CSS" == value) return AuroraResourceType.CSS;
            if ("CCS" == value) return AuroraResourceType.CCS;
            if ("BTM" == value) return AuroraResourceType.BTM;
            if ("UTM" == value) return AuroraResourceType.UTM;
            if ("DWK" == value) return AuroraResourceType.DWK;
            if ("PWK" == value) return AuroraResourceType.PWK;
            if ("BTG" == value) return AuroraResourceType.BTG;
            if ("UTG" == value) return AuroraResourceType.UTG;
            if ("JRL" == value) return AuroraResourceType.JRL;
            if ("SAV" == value) return AuroraResourceType.SAV;
            if ("UTW" == value) return AuroraResourceType.UTW;
            if ("4PC" == value) return AuroraResourceType.FourPC;
            if ("SSF" == value) return AuroraResourceType.SSF;
            if ("HAK" == value) return AuroraResourceType.HAK;
            if ("NWM" == value) return AuroraResourceType.NWM;
            if ("BIK" == value) return AuroraResourceType.BIK;
            if ("NDB" == value) return AuroraResourceType.NDB;
            if ("PTM" == value) return AuroraResourceType.PTM;
            if ("PTT" == value) return AuroraResourceType.PTT;
            if ("NCM" == value) return AuroraResourceType.NCM;
            if ("MFX" == value) return AuroraResourceType.MFX;
            if ("MAT" == value) return AuroraResourceType.MAT;
            if ("MDB" == value) return AuroraResourceType.MDB;
            if ("SAY" == value) return AuroraResourceType.SAY;
            if ("TTF" == value) return AuroraResourceType.TTF;
            if ("TTC" == value) return AuroraResourceType.TTC;
            if ("CUT" == value) return AuroraResourceType.CUT;
            if ("KA" == value) return AuroraResourceType.KA;
            if ("JPG" == value) return AuroraResourceType.JPG;
            if ("ICO" == value) return AuroraResourceType.ICO;
            if ("OGG" == value) return AuroraResourceType.OGG;
            if ("SPT" == value) return AuroraResourceType.SPT;
            if ("SPW" == value) return AuroraResourceType.SPW;
            if ("WFX" == value) return AuroraResourceType.WFX;
            if ("UGM" == value) return AuroraResourceType.UGM;
            if ("QDB" == value) return AuroraResourceType.QDB;
            if ("QST" == value) return AuroraResourceType.QST;
            if ("NPC" == value) return AuroraResourceType.NPC;
            if ("SPN" == value) return AuroraResourceType.SPN;
            if ("UTX" == value) return AuroraResourceType.UTX;
            if ("MMD" == value) return AuroraResourceType.MMD;
            if ("SMM" == value) return AuroraResourceType.SMM;
            if ("UTA" == value) return AuroraResourceType.UTA;
            if ("MDE" == value) return AuroraResourceType.MDE;
            if ("MDV" == value) return AuroraResourceType.MDV;
            if ("MDA" == value) return AuroraResourceType.MDA;
            if ("MBA" == value) return AuroraResourceType.MBA;
            if ("OCT" == value) return AuroraResourceType.OCT;
            if ("BFX" == value) return AuroraResourceType.BFX;
            if ("PDB" == value) return AuroraResourceType.PDB;
            if ("TheWitcherSave" == value) return AuroraResourceType.TheWitcherSave;
            if ("PVS" == value) return AuroraResourceType.PVS;
            if ("CFX" == value) return AuroraResourceType.CFX;
            if ("LUC" == value) return AuroraResourceType.LUC;
            if ("PRB" == value) return AuroraResourceType.PRB;
            if ("CAM" == value) return AuroraResourceType.CAM;
            if ("VDS" == value) return AuroraResourceType.VDS;
            if ("BIN" == value) return AuroraResourceType.BIN;
            if ("WOB" == value) return AuroraResourceType.WOB;
            if ("API" == value) return AuroraResourceType.API;
            if ("Properties" == value) return AuroraResourceType.Properties;
            if ("PNG" == value) return AuroraResourceType.PNG;
            if ("LYT" == value) return AuroraResourceType.LYT;
            if ("VIS" == value) return AuroraResourceType.VIS;
            if ("RIM" == value) return AuroraResourceType.RIM;
            if ("PTH" == value) return AuroraResourceType.PTH;
            if ("LIP" == value) return AuroraResourceType.LIP;
            if ("BWM" == value) return AuroraResourceType.BWM;
            if ("TXB" == value) return AuroraResourceType.TXB;
            if ("TPC" == value) return AuroraResourceType.TPC;
            if ("MDX" == value) return AuroraResourceType.MDX;
            if ("RSV" == value) return AuroraResourceType.RSV;
            if ("SIG" == value) return AuroraResourceType.SIG;
            if ("MAB" == value) return AuroraResourceType.MAB;
            if ("QST2" == value) return AuroraResourceType.QST2;
            if ("STO" == value) return AuroraResourceType.STO;
            if ("HEX" == value) return AuroraResourceType.HEX;
            if ("MDX2" == value) return AuroraResourceType.MDX2;
            if ("TXB2" == value) return AuroraResourceType.TXB2;
            if ("FSM" == value) return AuroraResourceType.FSM;
            if ("ART" == value) return AuroraResourceType.ART;
            if ("AMP" == value) return AuroraResourceType.AMP;
            if ("CWA" == value) return AuroraResourceType.CWA;
            if ("BIP" == value) return AuroraResourceType.BIP;
            if ("MDB2" == value) return AuroraResourceType.MDB2;
            if ("MDA2" == value) return AuroraResourceType.MDA2;
            if ("SPT2" == value) return AuroraResourceType.SPT2;
            if ("GR2" == value) return AuroraResourceType.GR2;
            if ("FXA" == value) return AuroraResourceType.FXA;
            if ("FXE" == value) return AuroraResourceType.FXE;
            if ("JPG2" == value) return AuroraResourceType.JPG2;
            if ("PWC" == value) return AuroraResourceType.PWC;
            if ("1DA" == value) return AuroraResourceType.OneDA;
            if ("ERF" == value) return AuroraResourceType.ERF;
            if ("BIF" == value) return AuroraResourceType.BIF;
            if ("KEY" == value) return AuroraResourceType.KEY;
            else return AuroraResourceType.UNKNOWN;
        }
    }

    public enum AuroraResourceID {
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
        KEY = 9999
    }

    public static class AuroraResourceTypeExtensions { 

        public static AuroraResourceID[] compiledResources() {
            return new AuroraResourceID[] {
                        AuroraResourceID.BMP,
                        AuroraResourceID.MVE,
                        AuroraResourceID.TGA,
                        AuroraResourceID.WAV,
                        AuroraResourceID.PLT,
                        AuroraResourceID.INI,
                        AuroraResourceID.BMU,
                        AuroraResourceID.MPG,
                        AuroraResourceID.WMA,
                        AuroraResourceID.WMV,
                        AuroraResourceID.XMV,
                        AuroraResourceID.PLH,
                        AuroraResourceID.TEX,
                        AuroraResourceID.MDL,
                        AuroraResourceID.THG,
                        AuroraResourceID.FNT,
                        AuroraResourceID.LUA,
                        AuroraResourceID.SLT,
                        AuroraResourceID.NSS,
                        AuroraResourceID.NCS,
                        AuroraResourceID.MOD,
                        AuroraResourceID.ARE,
                        AuroraResourceID.SET,
                        AuroraResourceID.IFO,
                        AuroraResourceID.BIC,
                        AuroraResourceID.WOK,
                        AuroraResourceID.TwoDA,
                        AuroraResourceID.TLK,
                        AuroraResourceID.TXI,
                        AuroraResourceID.GIT,
                        AuroraResourceID.BTI,
                        AuroraResourceID.UTI,
                        AuroraResourceID.BTC,
                        AuroraResourceID.UTC,
                        AuroraResourceID.DLG,
                        AuroraResourceID.ITP,
                        AuroraResourceID.BTT,
                        AuroraResourceID.UTT,
                        AuroraResourceID.DDS,
                        AuroraResourceID.BTS,
                        AuroraResourceID.UTS,
                        AuroraResourceID.LTR,
                        AuroraResourceID.GFF,
                        AuroraResourceID.FAC,
                        AuroraResourceID.BTE,
                        AuroraResourceID.UTE,
                        AuroraResourceID.BTD,
                        AuroraResourceID.UTD,
                        AuroraResourceID.BTP,
                        AuroraResourceID.UTP,
                        AuroraResourceID.DFT,
                        AuroraResourceID.GIC,
                        AuroraResourceID.GUI,
                        AuroraResourceID.CSS,
                        AuroraResourceID.CCS,
                        AuroraResourceID.BTM,
                        AuroraResourceID.UTM,
                        AuroraResourceID.DWK,
                        AuroraResourceID.PWK,
                        AuroraResourceID.BTG,
                        AuroraResourceID.UTG,
                        AuroraResourceID.JRL,
                        AuroraResourceID.SAV,
                        AuroraResourceID.UTW,
                        AuroraResourceID.FourPC,
                        AuroraResourceID.SSF,
                        AuroraResourceID.HAK,
                        AuroraResourceID.NWM,
                        AuroraResourceID.BIK,
                        AuroraResourceID.NDB,
                        AuroraResourceID.PTM,
                        AuroraResourceID.PTT,
                        AuroraResourceID.NCM,
                        AuroraResourceID.MFX,
                        AuroraResourceID.MAT,
                        AuroraResourceID.MDB,
                        AuroraResourceID.SAY,
                        AuroraResourceID.TTF,
                        AuroraResourceID.TTC,
                        AuroraResourceID.CUT,
                        AuroraResourceID.KA,
                        AuroraResourceID.JPG,
                        AuroraResourceID.ICO,
                        AuroraResourceID.OGG,
                        AuroraResourceID.SPT,
                        AuroraResourceID.SPW,
                        AuroraResourceID.WFX,
                        AuroraResourceID.UGM,
                        AuroraResourceID.QDB,
                        AuroraResourceID.QST,
                        AuroraResourceID.NPC,
                        AuroraResourceID.SPN,
                        AuroraResourceID.UTX,
                        AuroraResourceID.MMD,
                        AuroraResourceID.SMM,
                        AuroraResourceID.UTA,
                        AuroraResourceID.MDE,
                        AuroraResourceID.MDV,
                        AuroraResourceID.MDA,
                        AuroraResourceID.MBA,
                        AuroraResourceID.OCT,
                        AuroraResourceID.BFX,
                        AuroraResourceID.PDB,
                        AuroraResourceID.TheWitcherSave,
                        AuroraResourceID.PVS,
                        AuroraResourceID.CFX,
                        AuroraResourceID.LUC,
                        AuroraResourceID.PRB,
                        AuroraResourceID.CAM,
                        AuroraResourceID.VDS,
                        AuroraResourceID.BIN,
                        AuroraResourceID.WOB,
                        AuroraResourceID.API,
                        AuroraResourceID.Properties,
                        AuroraResourceID.PNG,
                        AuroraResourceID.LYT,
                        AuroraResourceID.VIS,
                        AuroraResourceID.RIM,
                        AuroraResourceID.PTH,
                        AuroraResourceID.LIP,
                        AuroraResourceID.BWM,
                        AuroraResourceID.TXB,
                        AuroraResourceID.TPC,
                        AuroraResourceID.MDX,
                        AuroraResourceID.RSV,
                        AuroraResourceID.SIG,
                        AuroraResourceID.MAB,
                        AuroraResourceID.QST2,
                        AuroraResourceID.STO,
                        AuroraResourceID.HEX,
                        AuroraResourceID.MDX2,
                        AuroraResourceID.TXB2,
                        AuroraResourceID.FSM,
                        AuroraResourceID.ART,
                        AuroraResourceID.AMP,
                        AuroraResourceID.CWA,
                        AuroraResourceID.BIP,
                        AuroraResourceID.MDB2,
                        AuroraResourceID.MDA2,
                        AuroraResourceID.SPT2,
                        AuroraResourceID.GR2,
                        AuroraResourceID.FXA,
                        AuroraResourceID.FXE,
                        AuroraResourceID.JPG2,
                        AuroraResourceID.PWC,
                        AuroraResourceID.OneDA,
                        AuroraResourceID.ERF,
                        AuroraResourceID.BIF,
                        AuroraResourceID.KEY,
            };
        }

        public static bool is2da(this AuroraResourceType fileType) {
            switch (fileType.id) {
                case AuroraResourceID.TwoDA:
                    return true;
                default:
                    return false;
            }
        }

        public static bool isResourceCollection(this AuroraResourceType fileType) {
            switch (fileType.id) {
                case AuroraResourceID.ERF:
                case AuroraResourceID.MOD:
                case AuroraResourceID.RIM:
                case AuroraResourceID.SAV:
                case AuroraResourceID.HAK:
                    return true;
                default:
                    return false;
            }
        }

        public static bool isGFF(this AuroraResourceType fileType) {
            switch (fileType.id) {
                case AuroraResourceID.ARE:
                case AuroraResourceID.IFO:
                case AuroraResourceID.BIC:
                case AuroraResourceID.WOK:
                case AuroraResourceID.GIT:
                case AuroraResourceID.UTI:
                case AuroraResourceID.UTC:
                case AuroraResourceID.DLG:
                case AuroraResourceID.ITP:
                case AuroraResourceID.UTT:
                case AuroraResourceID.UTS:
                case AuroraResourceID.GFF:
                case AuroraResourceID.FAC:
                case AuroraResourceID.UTE:
                case AuroraResourceID.UTD:
                case AuroraResourceID.UTP:
                case AuroraResourceID.GIC:
                case AuroraResourceID.GUI:
                case AuroraResourceID.UTM:
                case AuroraResourceID.JRL:
                case AuroraResourceID.UTW:
                case AuroraResourceID.PTM:
                case AuroraResourceID.PTT:
                    return true;
                default:
                    return false;
            }
        }
    }
}
