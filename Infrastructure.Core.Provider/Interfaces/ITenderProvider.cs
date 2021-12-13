using Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Providers
{
    public interface ITenderProvider
    {
        List<Tender> GetTenderDetails(int StatusID);
        ModifyTender GetTenderByID(int tenderID);
        bool AddTender(AddTender tender);
        bool UpdateTender(ModifyTender tender);
        bool DeleteTender(int tenderID);

        List<Tender> GetTenderStatus(Tender tender);
        
        List<Company> GetCompanyDetails();
        List<Tender> GetBankNameDetails();

        List<Rival> GetRivalDetails(int tenderID);
        Rival GetRivalByID(int rivalID);
        bool CreateRival(Rival rival);
        bool UpdateRivalDetails(Rival rival);

        List<Rival> GetTenderName();
        List<Rival> GetRivalName();

        List<Tender> GetTenderStatusName();
        List<Tender> GetUploadDocumentFile(int tenderID);
        bool Uploadfile(string fileName, string filePath,int tenderID);
        bool DeleteUploadDoc(int FileID);

        bool DeleteRivals(int id);

        bool AddColor(int[] rivalID ,string colorCode);
}
}
