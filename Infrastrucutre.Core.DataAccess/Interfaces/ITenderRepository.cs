using Infrastructure.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.DataAccess
{
    public interface ITenderRepository
    {
        
        #region Tender Operation Here
        List<Tender> GetTenderDetails(int StatusID);

        ModifyTender GetTenderByID(int tenderID);

        bool AddTender(AddTender tender);

        bool UpdateTender(ModifyTender tender);

        bool DeleteTender(int tenderID);

        List<Rival> GetTenderName();

        #endregion

        #region  Company/Bank Operation Here

        List<Company> GetCompanyDetails();

        List<Tender> GetBankNameDetails();

        #endregion


        #region TebnderStatus Operation Here

        List<Tender> GetTenderStatus(Tender tender);

        List<Tender> GetTenderStatusName();

        #endregion

        #region Rivals Operation Here

        List<Rival> GetRivalDetails(int tenderID);

        Rival GetRivalByID(int rivalID);

        bool CreateRival(Rival rival);

        bool UpdateRivalDetails(Rival rival);

        List<Rival> GetRivalName();

        bool DeleteRivals(int id);

        bool AddColor(int[] rivalID, string colorCode);

        #endregion

        #region DocumentUpload Operation Here

        List<Tender> GetUploadDocumentFile(int tenderID);

        bool Uploadfile(string fileName, string filePath,int tenderID);

        bool DeleteUploadDoc(int FileID);
        #endregion


        
    }
}
