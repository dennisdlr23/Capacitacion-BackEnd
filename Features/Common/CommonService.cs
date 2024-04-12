using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OrderPurchase.WebApi.Features.Common.Dto;
using OrderPurchase.WebApi.Features.Common.Entities;
using OrderPurchase.WebApi.Helpers;
using OrderPurchase.WebApi.Infraestructure;
using Microsoft.AspNetCore.Http;

namespace OrderPurchase.WebApi.Features.Common
{
    public class CommonService
    {
        private readonly OrderPurchaseDbContext _OrderPurchaseDbContext;
        public CommonService(OrderPurchaseDbContext logisticaBtdDbContext)
        {
            _OrderPurchaseDbContext = logisticaBtdDbContext;
        }
 
  
     

       
       
        
       
        

    }
}
