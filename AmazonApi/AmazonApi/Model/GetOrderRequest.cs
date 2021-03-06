/*******************************************************************************
 * Copyright 2009-2014 Amazon Services. All Rights Reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 *
 * You may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License.
 *******************************************************************************
 * Get Order Request
 * API Version: 2013-09-01
 * Library Version: 2013-09-01
 * Generated: Fri Jun 06 15:20:51 UTC 2014
 */


using System;
using System.Xml;
using System.Collections.Generic;
using MWSClientCsRuntime;

namespace MarketplaceWebServiceOrders.Model
{
    public class GetOrderRequest : AbstractMwsObject
    {

        private string _sellerId;
        private List<string> _amazonOrderId;

        /// <summary>
        /// Gets and sets the SellerId property.
        /// </summary>
        public string SellerId
        {
            get { return this._sellerId; }
            set { this._sellerId = value; }
        }

        /// <summary>
        /// Sets the SellerId property.
        /// </summary>
        /// <param name="sellerId">SellerId property.</param>
        /// <returns>this instance.</returns>
        public GetOrderRequest WithSellerId(string sellerId)
        {
            this._sellerId = sellerId;
            return this;
        }

        /// <summary>
        /// Checks if SellerId property is set.
        /// </summary>
        /// <returns>true if SellerId property is set.</returns>
        public bool IsSetSellerId()
        {
            return this._sellerId != null;
        }

        /// <summary>
        /// Gets and sets the AmazonOrderId property.
        /// </summary>
        public List<string> AmazonOrderId
        {
            get
            {
                if(this._amazonOrderId == null)
                {
                    this._amazonOrderId = new List<string>();
                }
                return this._amazonOrderId;
            }
            set { this._amazonOrderId = value; }
        }

        /// <summary>
        /// Sets the AmazonOrderId property.
        /// </summary>
        /// <param name="amazonOrderId">AmazonOrderId property.</param>
        /// <returns>this instance.</returns>
        public GetOrderRequest WithAmazonOrderId(string[] amazonOrderId)
        {
            this._amazonOrderId.AddRange(amazonOrderId);
            return this;
        }

        /// <summary>
        /// Checks if AmazonOrderId property is set.
        /// </summary>
        /// <returns>true if AmazonOrderId property is set.</returns>
        public bool IsSetAmazonOrderId()
        {
            return this.AmazonOrderId.Count > 0;
        }


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _sellerId = reader.Read<string>("SellerId");
            _amazonOrderId = reader.ReadList<string>("AmazonOrderId", "Id");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("SellerId", _sellerId);
            writer.WriteList("AmazonOrderId", "Id", _amazonOrderId);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("https://mws.amazonservices.com/Orders/2013-09-01", "GetOrderRequest", this);
        }

        public GetOrderRequest() : base()
        {
        }
    }
}
