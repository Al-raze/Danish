using Infrastrucutre.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Core.Provider
{
    public interface IListingProvider
    {
        bool AddListing(ListingRequest request, out int listingRequestID);
        //_____________ FBA Request Form _____________//
        bool AddFBARequest(FbaRequest request, out int FRequestID);

        List<ListingRequest> GetListingRequests(int requestID=0);

        List<FbaRequest> GetFbaRequest(out int totalCount, int jtStartIndex = 0, int jtPageSize = 0);// Get Request List 
        List<FbaRequest> GetFbaPendingRequest(int jtStartIndex = 0, int jtPageSize = 0, int SellerIndex = 0,int FBARootID = 0);//Get Fba Pending Request
        List<FbaRequest> GetFbaSortedList(int jtStartIndex = 0, int jtPageSize = 0,int SellerIndex = 0, int FBARootID = 0);//Get FbaSorted List 15-03-2021
        bool UpdateFbaProcesingByID(int FBARequestID = 0); //Accept Fba Processing form
        bool RejectFbaProcesingByID(int FBARequestID = 0); // Reject FBA PROCESSING FORM
        List<ShipmentDetails> GetShipmentDetails(out int totalCount, int jtStartIndex = 0, int jtPageSize = 0);//Shipment Details 
        List<ShipmentDetails> GetTotalFNSKU(string ShipmentID = "");
        bool UpdateListing(ListingRequest request);
        bool UpdateFbaRequest(FbaRequest request);//Update Fba Request List

        bool SubmitListing(ListingSubmission submission);

        ListingRequest GetListingRequestByID(int requestID);
        FbaRequest GetFbaRequestByID(int FBARequestID);

        List<ListingRequestFilter> GetUnAllocatedListingRequestNo();

        List<ListingStatus> GetListingStatus();

        List<ListingChannel> GetListingChannels();

        ListingSubmission GetListingSubmissionByID(int submissionID);

        bool UpdateSubmission(ListingSubmission submission);

        List<Submission> GetListingSubmissions(int itemMasterID=0);

        bool InsertOrders(List<Order> orders);

        bool DeleteOrder(int orderID);

        List<ListingDocument> GetListingRequestImages(int requestID);

        bool Uploadfile(string fileName, string filePath, int requestID);

        bool DeleteFile(int requestID, int fileID);

        List<Order> GetListingOrders(int[] orderIDs = null, string startDate = null, string endDate = null, string sellerId = null, string dispatchStatus = null, string orderNumber = null, bool? specialDelivery = null, string jtSorting = null);

        bool CreateListingItems(List<BulkInsert> ItemsInserted);

        List<SellerAccount> GetSeller(string sellerID = "",bool ?sync = false);

        List<UserInformation> GetUser(string UserID = "", bool? sync = false); // Get User For Group Asssign 03-02-2021
        List<UserInformation> GetGroupUser(string UserID = "", bool? sync = false); // Get User For Group User List 03-12-2021

        bool UpdateCarrier(int PostalCarrierID,DateTime ProceedTime, int[] OrderIDs);
        bool updateReactiveDeletedOrders(DateTime ProceedTime, int[] OrderIDs);

        bool UpdateCarrierRef(string carrierRef, DateTime proceedDateTime, int[] orderIDs);

        bool CheckExistingOrder(string orderRefNo);

        List<ItemImage> GetItemCodes(string sellerID);

        bool InsertImages(List<ItemImage> images);

        bool UpdateOrders(List<OrderItem> orderItems);

        bool UpdateOrderCarriers(List<Order> orders);

        bool InsertSalesCommission(SaleOrder sale, out string errorMessage);

        bool SynchronizeSKU();

        List<AutoCompleteItem> GetSellerList();

        List<AutoCompleteItem> GetPostageList(); // Get Postage List For SKU Add New Recorde 24-02-2021

        SellerItemLink UpdateSellerItemLink(SellerItemLink link);

        List<SellerItemLink> GetItemSellerLink(int id);

        List<SellerItemLink> GetSellerItemLink(out int rowCount, int id, string filter = "", string filterText = "", int jtStartIndex = 0, int jtPageSize = 0);

        List<SellerItemLink> GetListingLinkSubmission(int id);

        SellerItemLink InsertLink(SellerItemLink link);

        bool UpdateLink(SellerItemLink link);
        bool DeleteLink(int ItemLinkId);//Delete Sku 26-02-2021

        bool UpdateSellerAccount(SellerAccount account);

        SellerAccount InsertSellerAccount(SellerAccount account);

        List<SellerAccount> GetSellerAccounts();

        List<SellerItemLink> GetStockErrors();

        bool UpdateSellerAuth(SellerAccount account);

        SellerItemLink InsertAndFix(SellerItemLink link);

        bool RefreshOrderItems(string orderReferenceNo,List<OrderItem> items);
        bool UpdateAdditionalNotes(string AditionalNotes, int[] orderIDs);

        //-----------------------------------------------------------------------------------------------------------------//
        List<ItemMaster> GetItemsList(string ItemMasterID = "");// Get Item List 15-10-2020 # Danish
        List<ItemMaster> GetSKUList(string ItemMasterID = "");// Get Item List 15-10-2020 # Danish
        List<ItemDetails> GetFNSKUList(string ItemMasterID = "");
        List<FBALocations> GetFBARootList(string FBARootID ="",bool? sync = false);
        List<ItemMaster> GetAsinsList(string ItemMasterID = "");// Get Asin List 15-10-2020 # Danish
        // List<ItemStock> GetSKUByID(string ItemMasterID ="",bool? sync=false);
        List<ItemMaster> GetUserItemByID(string UserID ="");
        List<ItemMaster> GetAsinByID(string ItemMasterID = "");
        List<ItemMaster> GetSKUByID(string ListItemNo = "");

        //-----------------------------------------------------------------------------------------------------------------//
        bool AddGroup(int GID, DateTime ProceedTime, int[] OrderIDs); //Group Update 10-02-2021
        bool AddShipment(string ShipmentID, string ShipmentMethod, string Destination, DateTime ProceedTime,decimal[] FinalQty,decimal[] FBABoxQty, int[] OrderIDs);
        List<FbaRequest> GetShipmentHistory(string id, out int rowCount, int jtStartIndex = 0, int jtPageSize = 0);
        bool DeleteShipment(string ShipmentID);//Delete Shipment
        List<FbaRequest> GetFbaRequestHistory(string id, out int rowCount, int jtStartIndex = 0, int jtPageSize = 0);


    }
}
