using Infrastrucutre.Core.DataAccess;
using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Core.Provider
{
    public class ListingProvider : IListingProvider
    {

        IListingRepository _listingRepository;

        //-------------------------------------------Load Lists------------------------------------------------------//
        public List<ItemMaster> GetItemsList(string ItemName = "") //Get Item List 15-10-2020 # Danish
        {
            return _listingRepository.GetItemsList(ItemName);
        }

        public List<ItemMaster> GetSKUList(string SKU = "") //Get Item List 24-10-2020 # Danish
        {
            return _listingRepository.GetSKUList(SKU);
        }


        public List<ItemMaster> GetAsinsList(string SKU = "") //Get Asin No List 24-10-2020 # Danish
        {
            return _listingRepository.GetAsinsList(SKU);
        }
        public List<ItemMaster> GetUserItemByID(string UserID = "") //Get User Item List 24-10-2020 # Danish
        {
            return _listingRepository.GetUserItemByID(UserID);
        }

        public List<ItemMaster> GetAsinByID(string ItemMasterID = "") //Get Asin No List 24-10-2020 # Danish
        {
            return _listingRepository.GetAsinByID(ItemMasterID);
        }
        public List<ItemMaster> GetSKUByID(string ListingItemNo = "") //Get Asin No List 24-10-2020 # Danish
        {
            return _listingRepository.GetSKUByID(ListingItemNo);
        }

        public List<UserInformation> GetUser(string UserID = "", bool? sync = false) //Get Asin No List 24-10-2020 # Danish
        {
            return _listingRepository.GetUser(UserID, sync);
        }

        public List<UserInformation> GetGroupUser(string UserID = "", bool? sync = false) //Get Group List 02-12-2021 # Danish
        {
            return _listingRepository.GetGroupUser(UserID, sync);
        }

        public List<ItemDetails> GetFNSKUList(string FNSKU = "") //Get FNSKU List 24-10-2020 # Danish
        {
            return _listingRepository.GetFNSKUList(FNSKU);
        }
        public List<FBALocations> GetFBARootList(string FBARootID = "", bool? sync = false) //Get FNSKU List 24-10-2020 # Danish
        {
            return _listingRepository.GetFBARootList(FBARootID, sync);
        }

        //-------------------------------------------28-10-2020---------------------------------------------------------//
        public ListingProvider(IListingRepository listingRepository)
        {
            this._listingRepository = listingRepository;
        }

        public SellerItemLink InsertLink(SellerItemLink link)
        {
            return _listingRepository.InsertLink(link);
        }

        public bool UpdateLink(SellerItemLink link)
        {
            return _listingRepository.UpdateLink(link);
        }

        public bool DeleteLink(int ItemLinkId) //Delete Sku 
        {
            return _listingRepository.DeleteLink(ItemLinkId);
        }

        public bool RefreshOrderItems(string orderReferenceNo, List<OrderItem> items)
        {
            return _listingRepository.RefreshOrderItems(orderReferenceNo, items);
        }

        public List<AutoCompleteItem> GetSellerList()
        {
            return _listingRepository.GetSellerList();
        }

        public List<AutoCompleteItem> GetPostageList() // Get Postage List For SKU New Recorde
        {
            return _listingRepository.GetPostageList();
        }

        public bool AddListing(ListingRequest request, out int listingRequestID)
        {
            return _listingRepository.AddListing(request, out listingRequestID);
        }

        public bool AddFBARequest(FbaRequest request, out int FRequestID) //FBA Request Form 04-03-2021
        {
            return _listingRepository.FBARequest(request, out FRequestID);
        }

        public List<ListingRequest> GetListingRequests(int requestID = 0)
        {
            return _listingRepository.GetListingRequests(requestID);

        }
        //_______________FBA Req Updating____________________//

        public List<FbaRequest> GetFbaRequest(out int totalCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            return _listingRepository.GetFbaRequest(out totalCount, jtStartIndex, jtPageSize);

        }
        public bool UpdateFbaRequest(FbaRequest request)
        {
            return _listingRepository.UpdateFbaRequest(request);
        }

        public List<FbaRequest> GetFbaPendingRequest(int jtStartIndex = 0, int jtPageSize = 0, int SellerIndex = 0, int FBARootID = 0)
        {
            return _listingRepository.GetFbaPendingRequest(jtStartIndex, jtPageSize, SellerIndex, FBARootID);

        }

        public bool UpdateFbaProcesingByID(int FBARequestID = 0) //Accept FBA Processing Form
        {
            return _listingRepository.UpdateFbaProcesingByID(FBARequestID);

        }
        public bool RejectFbaProcesingByID(int FBARequestID = 0) //Accept FBA Processing Form
        {
            return _listingRepository.RejectFbaProcesingByID(FBARequestID);

        }
        //________Get Fba Sorted List 15/03/2021________//


        public List<FbaRequest> GetFbaSortedList(int jtStartIndex = 0, int jtPageSize = 0, int SellerIndex = 0, int FBARootID = 0)
        {
            return _listingRepository.GetFbaSortedList(jtStartIndex, jtPageSize, SellerIndex, FBARootID);

        }

        public bool AddShipment(string ShipmentID, string ShipmentMethod, string Destination, DateTime ProceedTime, decimal[] FinalQty, decimal[] FBABoxQty, int[] OrderIDs)
        {
            return _listingRepository.AddShipment(ShipmentID, ShipmentMethod, Destination, ProceedTime,FinalQty, FBABoxQty, OrderIDs);
        }

        public List<ShipmentDetails> GetShipmentDetails(out int totalCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            return _listingRepository.GetShipmentDetails(out totalCount,jtStartIndex,jtPageSize);

        }
        public List<FbaRequest> GetShipmentHistory(string id, out int totalCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            return _listingRepository.GetShipmentHistory(id, out totalCount, jtStartIndex, jtPageSize);

        }
        public List<FbaRequest> GetFbaRequestHistory(string id, out int totalCount, int jtStartIndex = 0, int jtPageSize = 0)
        {
            return _listingRepository.GetFbaRequestHistory(id, out totalCount, jtStartIndex, jtPageSize);

        }

        public bool DeleteShipment(string ShipmentID = "") //Accept FBA Processing Form
        {
            return _listingRepository.DeleteShipment(ShipmentID);

        }
        public List<ShipmentDetails> GetTotalFNSKU(string ShipmentID ="") ///Get Total Fnsku
        {
            return _listingRepository.GetTotalFNSKU(ShipmentID);
        }

        //_______________________________________//

        public bool UpdateListing(ListingRequest request)
        {
            return _listingRepository.UpdateListing(request);
        }

        public bool SubmitListing(ListingSubmission submission)
        {
            return _listingRepository.SubmitListing(submission);
        }


        public ListingRequest GetListingRequestByID(int requestID)
        {
            return _listingRepository.GetListingRequestByID(requestID);
        }

        public FbaRequest GetFbaRequestByID(int FbarequestID)
        {
            return _listingRepository.GetFbaRequestByID(FbarequestID);
        }


        public List<ListingRequestFilter> GetUnAllocatedListingRequestNo()
        {
            return _listingRepository.GetUnAllocatedListingRequestNo();
        }


        public List<ListingStatus> GetListingStatus()
        {
            return _listingRepository.GetListingStatus();
        }


        public List<ListingChannel> GetListingChannels()
        {
            return _listingRepository.GetListingChannels();
        }


        public ListingSubmission GetListingSubmissionByID(int submissionID)
        {
            return _listingRepository.GetListingSubmissionByID(submissionID);
        }


        public bool UpdateSubmission(ListingSubmission submission)
        {
            return _listingRepository.UpdateSubmission(submission);
        }


        public List<Submission> GetListingSubmissions(int itemMasterID = 0)
        {
            return _listingRepository.GetListingSubmissions(itemMasterID);
        }


        public bool InsertOrders(List<Order> orders)
        {
            return _listingRepository.InsertOrders(orders);
        }

        public bool SynchronizeSKU()
        {
            return _listingRepository.SynchronizeSKU();
        }

        public List<ListingDocument> GetListingRequestImages(int requestID)
        {
            return _listingRepository.GetListingRequestImages(requestID);
        }


        public bool Uploadfile(string fileName, string filePath, int requestID)
        {
            return _listingRepository.Uploadfile(fileName, filePath, requestID);
        }

        public List<SellerItemLink> GetSellerItemLink(out int rowCount, int id, string filter = "", string filterText = "", int jtStartIndex = 0, int jtPageSize = 0)
        {
            return _listingRepository.GetSellerItemLink(out rowCount, id, filter, filterText, jtStartIndex, jtPageSize);
        }


        public bool DeleteFile(int requestID, int fileID)
        {
            return _listingRepository.DeleteFile(requestID, fileID);
        }


        public List<Order> GetListingOrders(int[] orderIDs = null, string startDate = null, string endDate = null, string sellerId = null, string dispatchStatus = null, string orderNumber = null, bool? specialDelivery = null, string jtSorting = null)
        {
            return _listingRepository.GetListingOrders(orderIDs, startDate, endDate, sellerId, dispatchStatus, orderNumber: orderNumber, specialDelivery: specialDelivery, jtSorting: jtSorting);
        }


        public bool CreateListingItems(List<BulkInsert> ItemsInserted)
        {
            return _listingRepository.CreateListingItems(ItemsInserted);
        }


        public List<SellerAccount> GetSeller(string sellerID = "", bool? sync = false)
        {
            return _listingRepository.GetSeller(sellerID, sync);
        }


        public bool UpdateCarrier(int PostalCarrierID, DateTime ProceedTime, int[] OrderIDs)
        {
            return _listingRepository.UpdateCarrier(PostalCarrierID, ProceedTime, OrderIDs);
        }
        public bool updateReactiveDeletedOrders(DateTime ProceedTime, int[] OrderIDs)
        {
            return _listingRepository.updateReactiveDeletedOrders(ProceedTime, OrderIDs);
        }
        public bool UpdateCarrierRef(string carrierRef, DateTime ProceedTime, int[] OrderIDs)
        {
            return _listingRepository.UpdateCarrierRef(carrierRef, ProceedTime, OrderIDs);
        }

        public bool UpdateOrderCarriers(List<Order> orders)
        {
            return _listingRepository.UpdateOrderCarriers(orders);
        }


        public bool CheckExistingOrder(string orderRefNo)
        {
            return _listingRepository.CheckExistingOrder(orderRefNo);
        }

        public List<ItemImage> GetItemCodes(string sellerID)
        {
            return _listingRepository.GetItemCodes(sellerID);
        }

        public bool InsertImages(List<ItemImage> images)
        {
            return _listingRepository.InsertImages(images);
        }

        public bool UpdateOrders(List<OrderItem> orderItems)
        {
            return _listingRepository.UpdateOrders(orderItems);
        }

        public bool InsertSalesCommission(SaleOrder sale, out string errorMessage)
        {
            return _listingRepository.InsertSalesCommission(sale, out errorMessage);
        }

        public SellerItemLink UpdateSellerItemLink(SellerItemLink link)
        {
            return _listingRepository.UpdateSellerItemLink(link);
        }

        public List<SellerItemLink> GetItemSellerLink(int id)
        {
            return _listingRepository.GetItemSellerLink(id);
        }

        public List<SellerItemLink> GetListingLinkSubmission(int id)
        {
            return _listingRepository.GetListingLinkSubmission(id);
        }

        public bool UpdateSellerAccount(SellerAccount account)
        {
            return _listingRepository.UpdateSellerAccount(account);
        }
        public SellerAccount InsertSellerAccount(SellerAccount account)
        {
            return _listingRepository.InsertSellerAccount(account);
        }

        public List<SellerAccount> GetSellerAccounts()
        {
            return _listingRepository.GetSellerAccounts();
        }

        public bool UpdateSellerAuth(SellerAccount account)
        {
            return _listingRepository.UpdateSellerAuth(account);
        }

        public List<SellerItemLink> GetStockErrors()
        {
            return _listingRepository.GetStockErrors();
        }

        public SellerItemLink InsertAndFix(SellerItemLink link)
        {
            return _listingRepository.InsertAndFix(link);
        }

        public bool DeleteOrder(int orderID)
        {
            return _listingRepository.DeleteOrder(orderID);
        }
        public bool UpdateAdditionalNotes(string addiotionalNotes, int[] OrderIDs)
        {
            return _listingRepository.UpdateAdditionalNotes(addiotionalNotes, OrderIDs);
        }

        public bool AddGroup(int GID, DateTime ProceedTime, int[] OrderIDs)
        {
            return _listingRepository.AddGroup(GID, ProceedTime, OrderIDs);
        }

    }
}
