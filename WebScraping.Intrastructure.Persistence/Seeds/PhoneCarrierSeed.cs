using WebScraping.Core.Domain.Entities;
using Enums = WebScraping.Core.Application.Enums;

namespace WebScraping.Infrastructure.Persistence.Seeds
{
    public static class PhoneCarrierSeed
    {

        public static List<PhoneCarrier> data { get; set; } = new List<PhoneCarrier>()
        {
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.UNK,
                Name = "Unknown",
                ShortName = Enums.PhoneCarrier.UNK.ToString()
            },
                new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.ALL,
                Name = "All Carriers",
                ShortName = Enums.PhoneCarrier.ALL.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.ATT,
                Name = "AT&T",
                ShortName = Enums.PhoneCarrier.ATT.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.VZW,
                Name = "Verizon",
                ShortName = Enums.PhoneCarrier.VZW.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.TMB,
                Name = "T-Mobile",
                ShortName = Enums.PhoneCarrier.TMB.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.SPR,
                Name = "Sprint",
                ShortName = Enums.PhoneCarrier.SPR.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.USC,
                Name = "U.S. Cellular",
                ShortName = Enums.PhoneCarrier.USC.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.CTL,
                Name = "CenturyLink",
                ShortName = Enums.PhoneCarrier.CTL.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.SPC,
                Name = "Spectrum",
                ShortName = Enums.PhoneCarrier.SPC.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.XFN,
                Name = "Xfinity",
                ShortName = Enums.PhoneCarrier.XFN.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.CKT,
                Name = "Cricket",
                ShortName = Enums.PhoneCarrier.CKT.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.MPCS,
                Name = "Metro | MetroPCS",
                ShortName = Enums.PhoneCarrier.MPCS.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.TFN,
                Name = "TracFone",
                ShortName = Enums.PhoneCarrier.TFN.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.BST,
                Name = "Boost Mobile",
                ShortName = Enums.PhoneCarrier.BST.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.QLK,
                Name = "Q Link Wireless",
                ShortName = Enums.PhoneCarrier.QLK.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.RPW,
                Name = "Republic Wireless",
                ShortName = Enums.PhoneCarrier.RPW.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.STK,
                Name = "Straight Talk",
                ShortName = Enums.PhoneCarrier.STK.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.VMU,
                Name = "Virgin Mobile USA",
                ShortName = Enums.PhoneCarrier.VMU.ToString(),
            },new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.TWL,
                Name = "Total Wireless",
                ShortName = Enums.PhoneCarrier.TWL.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.GFI,
                Name = "Google Fi",
                ShortName = Enums.PhoneCarrier.GFI.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.MNT,
                Name = "Mint Mobile",
                ShortName = Enums.PhoneCarrier.MNT.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.TNG,
                Name = "Ting",
                ShortName = Enums.PhoneCarrier.TNG.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.CCU,
                Name = "Consumer Cellular",
                ShortName = Enums.PhoneCarrier.CCU.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.CRD,
                Name = "Credo Mobile",
                ShortName = Enums.PhoneCarrier.CRD.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.FDM,
                Name = "FreedomPop",
                ShortName = Enums.PhoneCarrier.FDM.ToString(),
            },
            new PhoneCarrier
            {
                Id = (int)Enums.PhoneCarrier.NTW,
                Name = "Net10 Wireless",
                ShortName = Enums.PhoneCarrier.NTW.ToString(),
            }
        };
    }
}