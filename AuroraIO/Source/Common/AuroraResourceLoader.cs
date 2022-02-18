using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AuroraIO.Models;
using System.Text.RegularExpressions;
using AuroraIO.Collections;

namespace AuroraIO {

    internal class AuroraResourceLoader {

        internal static AuroraResource loadFile(AuroraResourceInfo resInfo, byte[] fileArray) {
            switch (resInfo.resourceType) {
                case AuroraResourceType.TwoDA:
                //TODO: this needs to be reinstated
                //return new Array2D();//(resInfo.resref, fileArray);
                case AuroraResourceType.UTI:
                    //return new GFFObjectCoder().decode(fileArray);
                case AuroraResourceType.ARE:
                case AuroraResourceType.IFO:
                case AuroraResourceType.BIC:
                case AuroraResourceType.GIT:
                
                case AuroraResourceType.UTC:
                case AuroraResourceType.DLG:
                case AuroraResourceType.ITP:
                case AuroraResourceType.UTT:
                case AuroraResourceType.UTS:
                case AuroraResourceType.GFF:
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
                    //return new GFFObjectCoder().decode(fileArray);
                case AuroraResourceType.UNKNOWN:
                case AuroraResourceType.BMP:
                case AuroraResourceType.MVE:
                case AuroraResourceType.TGA:
                case AuroraResourceType.WAV:
                case AuroraResourceType.PLT:
                    return new AuroraBinaryObject(resInfo, fileArray);
                case AuroraResourceType.INI:
                case AuroraResourceType.TXT:
                case AuroraResourceType.NSS:
                case AuroraResourceType.SET:
                case AuroraResourceType.TXI:
                case AuroraResourceType.DFT:
                case AuroraResourceType.LYT:
                case AuroraResourceType.VIS:
                    return new AuroraTextObject(resInfo, fileArray);
                case AuroraResourceType.BMU:
                    break;
                case AuroraResourceType.MPG:
                    break;
                case AuroraResourceType.WMA:
                    break;
                case AuroraResourceType.WMV:
                    break;
                case AuroraResourceType.XMV:
                    break;
                case AuroraResourceType.PLH:
                    break;
                case AuroraResourceType.TEX:
                    break;
                case AuroraResourceType.MDL:
                    break;
                case AuroraResourceType.THG:
                    break;
                case AuroraResourceType.FNT:
                    break;
                case AuroraResourceType.LUA:
                    break;
                case AuroraResourceType.SLT:
                    break;
                case AuroraResourceType.NCS:
                    break;
                case AuroraResourceType.MOD:
                    break;
                case AuroraResourceType.WOK:
                    break;
                case AuroraResourceType.TLK:
                    break;
                case AuroraResourceType.BTI:
                    break;
                case AuroraResourceType.BTC:
                    break;
                case AuroraResourceType.BTT:
                    break;
                case AuroraResourceType.DDS:
                    break;
                case AuroraResourceType.BTS:
                    break;
                case AuroraResourceType.LTR:
                    break;
                case AuroraResourceType.FAC:
                    break;
                case AuroraResourceType.BTE:
                    break;
                case AuroraResourceType.BTD:
                    break;
                case AuroraResourceType.BTP:
                    break;
                case AuroraResourceType.CSS:
                    break;
                case AuroraResourceType.CCS:
                    break;
                case AuroraResourceType.BTM:
                    break;
                case AuroraResourceType.DWK:
                    break;
                case AuroraResourceType.PWK:
                    break;
                case AuroraResourceType.BTG:
                    break;
                case AuroraResourceType.UTG:
                    break;
                case AuroraResourceType.SAV:
                    return new ERFArchive(fileArray);
                case AuroraResourceType.FourPC:
                    break;
                case AuroraResourceType.SSF:
                    break;
                case AuroraResourceType.HAK:
                    break;
                case AuroraResourceType.NWM:
                    break;
                case AuroraResourceType.BIK:
                    break;
                case AuroraResourceType.NDB:
                    break;
                case AuroraResourceType.NCM:
                    break;
                case AuroraResourceType.MFX:
                    break;
                case AuroraResourceType.MAT:
                    break;
                case AuroraResourceType.MDB:
                    break;
                case AuroraResourceType.SAY:
                    break;
                case AuroraResourceType.TTF:
                    break;
                case AuroraResourceType.TTC:
                    break;
                case AuroraResourceType.CUT:
                    break;
                case AuroraResourceType.KA:
                    break;
                case AuroraResourceType.JPG:
                    break;
                case AuroraResourceType.ICO:
                    break;
                case AuroraResourceType.OGG:
                    break;
                case AuroraResourceType.SPT:
                    break;
                case AuroraResourceType.SPW:
                    break;
                case AuroraResourceType.WFX:
                    break;
                case AuroraResourceType.UGM:
                    break;
                case AuroraResourceType.QDB:
                    break;
                case AuroraResourceType.QST:
                    break;
                case AuroraResourceType.NPC:
                    break;
                case AuroraResourceType.SPN:
                    break;
                case AuroraResourceType.UTX:
                    break;
                case AuroraResourceType.MMD:
                    break;
                case AuroraResourceType.SMM:
                    break;
                case AuroraResourceType.UTA:
                    break;
                case AuroraResourceType.MDE:
                    break;
                case AuroraResourceType.MDV:
                    break;
                case AuroraResourceType.MDA:
                    break;
                case AuroraResourceType.MBA:
                    break;
                case AuroraResourceType.OCT:
                    break;
                case AuroraResourceType.BFX:
                    break;
                case AuroraResourceType.PDB:
                    break;
                case AuroraResourceType.TheWitcherSave:
                    break;
                case AuroraResourceType.PVS:
                    break;
                case AuroraResourceType.CFX:
                    break;
                case AuroraResourceType.LUC:
                    break;
                case AuroraResourceType.PRB:
                    break;
                case AuroraResourceType.CAM:
                    break;
                case AuroraResourceType.VDS:
                    break;
                case AuroraResourceType.BIN:
                    break;
                case AuroraResourceType.WOB:
                    break;
                case AuroraResourceType.API:
                    break;
                case AuroraResourceType.Properties:
                    break;
                case AuroraResourceType.PNG:
                    break;
                case AuroraResourceType.RIM:
                    break;
                case AuroraResourceType.PTH:
                    break;
                case AuroraResourceType.LIP:
                    break;
                case AuroraResourceType.BWM:
                    break;
                case AuroraResourceType.TXB:
                    break;
                case AuroraResourceType.TPC:
                    break;
                case AuroraResourceType.MDX:
                    break;
                case AuroraResourceType.RSV:
                    break;
                case AuroraResourceType.SIG:
                    break;
                case AuroraResourceType.MAB:
                    break;
                case AuroraResourceType.QST2:
                    break;
                case AuroraResourceType.STO:
                    break;
                case AuroraResourceType.HEX:
                    break;
                case AuroraResourceType.MDX2:
                    break;
                case AuroraResourceType.TXB2:
                    break;
                case AuroraResourceType.FSM:
                    break;
                case AuroraResourceType.ART:
                    break;
                case AuroraResourceType.AMP:
                    break;
                case AuroraResourceType.CWA:
                    break;
                case AuroraResourceType.BIP:
                    break;
                case AuroraResourceType.MDB2:
                    break;
                case AuroraResourceType.MDA2:
                    break;
                case AuroraResourceType.SPT2:
                    break;
                case AuroraResourceType.GR2:
                    break;
                case AuroraResourceType.FXA:
                    break;
                case AuroraResourceType.FXE:
                    break;
                case AuroraResourceType.JPG2:
                    break;
                case AuroraResourceType.PWC:
                    break;
                case AuroraResourceType.OneDA:
                    break;
                case AuroraResourceType.ERF:
                    break;
                case AuroraResourceType.BIF:
                    break;
                case AuroraResourceType.KEY:
                    break;
                default:
                    break;
            }
            return new AuroraBinaryObject(resInfo, fileArray);
        }

        private static AuroraResource loadFile(String path) {
            String fullFileName = Regex.Match(path, "[^\\\\]*$").Value.ToLower();
            String resref = Regex.Match(fullFileName, "^[^.]*").Value.ToLower();
            String extension = Regex.Match(fullFileName, "[^.]*$").Value;
            AuroraResourceType resourceType = extension.toAuroraResourceType();
            return AuroraResourceLoader.loadFile(new AuroraResourceInfo(resref, resourceType), File.ReadAllBytes(path));
        }
    }
}
