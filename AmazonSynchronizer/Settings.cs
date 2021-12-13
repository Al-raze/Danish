using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AmazonSynchronizer
{
    public static class Settings
    {
        public static string SQLServerConnectionString
        {
            get
            {
                return ConfigurationSettings.AppSettings["SQLServerConnectionString"].ToString();
            }

        }

        public static string MarketPlaceList
        {
            get
            {
                return ConfigurationSettings.AppSettings["MarketPlaceList"].ToString();
            }

        }

        public static string SellerIndex
        {
            get
            {
                return ConfigurationSettings.AppSettings["SellerIndex"].ToString();
            }

        }

        public static string SellerKey
        {
            get
            {
                return ConfigurationSettings.AppSettings["SellerKey"].ToString();
            }

        }

        public static string MarketplaceId
        {
            get
            {
                return ConfigurationSettings.AppSettings["MarketplaceId"].ToString();
            }
        }

        public static string AccessKey
        {
            get
            {
                return ConfigurationSettings.AppSettings["AccessKey"].ToString();
            }
        }

        public static string AuthenticationToken
        {
            get
            {
                return ConfigurationSettings.AppSettings["AuthenticationToken"].ToString();
            }
        }


        public static string SellerID
        {
            get
            {
                return ConfigurationSettings.AppSettings["SellerID"].ToString();
            }
        }
    }
}
