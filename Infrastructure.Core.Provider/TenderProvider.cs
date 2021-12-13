using Infrastructure.Core.Models;
using Infrastructure.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Core.Providers
{
    public class TenderProvider : ITenderProvider 
    {
        ITenderRepository _tenderRepository;
		//add new comment added
        public TenderProvider(ITenderRepository tenderRepository)
        {
            _tenderRepository = tenderRepository;
        }

        //Tender
        public List<Tender> GetTenderDetails(int StatusID)
        {
            return _tenderRepository.GetTenderDetails(StatusID);
        }

        public ModifyTender GetTenderByID(int tenderID)
        {
            return _tenderRepository.GetTenderByID(tenderID);
        }

        public bool AddTender(AddTender tender)
        {
            return _tenderRepository.AddTender(tender);
        }

        public bool UpdateTender(ModifyTender tender)
        {
            return _tenderRepository.UpdateTender(tender);
        }

        public bool DeleteTender(int tenderID)
        {
            return _tenderRepository.DeleteTender(tenderID);
        } 
       
        //TenderStatus
        public List<Tender> GetTenderStatus(Tender tender)
        {
            return _tenderRepository.GetTenderStatus(tender);
        }

        //get CompanyName
        public List<Company> GetCompanyDetails()
        {
            return _tenderRepository.GetCompanyDetails();
        }

        //Get Bank Name
        public List<Tender> GetBankNameDetails()
        {
            return _tenderRepository.GetBankNameDetails();
        }

        //Rival
        public List<Rival> GetRivalDetails(int tenderID)
        {
            return _tenderRepository.GetRivalDetails(tenderID);
        }

        public Rival GetRivalByID(int rivalID)
        {
            return _tenderRepository.GetRivalByID(rivalID);
        }

        public bool CreateRival(Rival rival)
        {
            return _tenderRepository.CreateRival(rival);
        }

        public bool UpdateRivalDetails(Rival rival)
        {
            return _tenderRepository.UpdateRivalDetails(rival);
        }


        public List<Rival> GetTenderName()
        {
            return _tenderRepository.GetTenderName();
        }

        public List<Rival> GetRivalName()
        {
            return _tenderRepository.GetRivalName();
        }


        public List<Tender> GetTenderStatusName()
        {
            return _tenderRepository.GetTenderStatusName();
        }


        public List<Tender> GetUploadDocumentFile(int tenderID)
        {
            return _tenderRepository.GetUploadDocumentFile(tenderID);
        }


        public bool Uploadfile(string fileName, string filePath,int tenderID)
        {
            return _tenderRepository.Uploadfile(fileName, filePath, tenderID);
        }


        public bool DeleteUploadDoc(int FileID)
        {
            return _tenderRepository.DeleteUploadDoc(FileID);
        }

        public bool DeleteRivals(int id)
        {
            return _tenderRepository.DeleteRivals(id);
        }

        public bool AddColor(int[] rivalID, string colorCode)
        {
            return _tenderRepository.AddColor(rivalID, colorCode);
        }
    }
}
