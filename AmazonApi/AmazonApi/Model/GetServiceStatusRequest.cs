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
 * Get Service Status Request
 * API Version: com.amazon.maws.coral
 * Library Version: 2013-09-01
 * Generated: Fri Jun 06 15:20:51 UTC 2014
 */


using System;
using System.Xml;
using MWSClientCsRuntime;

namespace MarketplaceWebServiceOrders.Model
{
    public class GetServiceStatusRequest : AbstractMwsObject
    {

        private string _sellerId;

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
        public GetServiceStatusRequest WithSellerId(string sellerId)
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


        public override void ReadFragmentFrom(IMwsReader reader)
        {
            _sellerId = reader.Read<string>("SellerId");
        }

        public override void WriteFragmentTo(IMwsWriter writer)
        {
            writer.Write("SellerId", _sellerId);
        }

        public override void WriteTo(IMwsWriter writer)
        {
            writer.Write("http://internal.amazon.com/coral/com.amazon.maws.coral/", "GetServiceStatusRequest", this);
        }

        public GetServiceStatusRequest() : base()
        {
        }
    }
}
