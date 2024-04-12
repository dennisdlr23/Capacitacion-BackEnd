using OrderPurchase.WebApi.Features.Orders.Dto;
using OrderPurchase.WebApi.Features.Orders.Entitie;
using OrderPurchase.WebApi.Features.ServiceLayer;
using OrderPurchase.WebApi.Features.ServiceLayer.Dto;
using OrderPurchase.WebApi.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderPurchase.WebApi.Features.Orders.Service
{
    public class OrderServices
    {
        private readonly OrderPurchaseDbContext _context;
        private readonly OrderPurchaseServices _serviceSap;

        public OrderServices(OrderPurchaseDbContext context, OrderPurchaseServices serviceSap)
        {
            _context = context;
            _serviceSap = serviceSap;
        }

        public List<OrderDto> GetOrder()
        {
            var result = (from o in _context.Order
                          join u in _context.User on o.CreatedBy equals u.UserId
                          select new OrderDto
                          {
                              Id = o.Id,
                              DocNum = o.DocNum,
                              DocEntry = o.DocEntry,
                              CardCode = o.CardCode,
                              DocDate = o.DocDate,
                              Reference = o.Reference,
                              CreatedBy = o.CreatedBy,
                              CreatedByName = u.Name,
                              Detail = (_context.OrderDetail.Where(x => x.IdOrder == o.Id).ToList())
                          }).ToList();
            return result;
        }

        public List<OrderDto> GetOrderByDate(DateTime fro, DateTime to)
        {
            var result = (from o in _context.Order
                          join u in _context.User on o.CreatedBy equals u.UserId
                          where o.DocDate.Date >= fro.Date && o.DocDate <= to.Date
                          select new OrderDto
                          {
                              Id = o.Id,
                              DocNum = o.DocNum,
                              DocEntry = o.DocEntry,
                              CardCode = o.CardCode,
                              DocDate = o.DocDate,
                              Reference = o.Reference,
                              CreatedBy = o.CreatedBy,
                              CreatedByName = u.Name,
                              Detail = (_context.OrderDetail.Where(x => x.IdOrder == o.Id).ToList())
                          }).ToList();
            return result;
        }

        public List<OrderDto> AddOrder(Order request)
        {
            try
            {
                _context.Database.BeginTransaction();
                request.DocTotal = request.Detail.Sum(x => x.Quantity * x.Price);
                request.DocDate = DateTime.Now;
                _context.Order.Add(request);
                _context.SaveChanges();
                //Creamos el detalle
                var documentLines = new List<DocumentDetailDto>();
                foreach (var detail in request.Detail)
                {
                    var documentDetail = new DocumentDetailDto
                    {
                        ItemCode = detail.ItemCode,
                        Quantity = detail.Quantity,
                        TaxCode = "EXE",
                        UnitPrice = detail.Price
                    };
                    documentLines.Add(documentDetail);
                }
                //Creamos la cabecera
                var order = new DocumentDto
                {
                    CardCode = request.CardCode,
                    DocDate = request.DocDate,
                    Serie = 15,
                    U_FIS = "Y",
                    DocumentLines = documentLines
                };
                //Mandamos a crear a SAP
                var result1 = _serviceSap.CreatePurchaseOrder(order);
                //Validamos el resultado
                if (result1.Item1 != 0)
                {
                    request.DocEntry = result1.Item1;
                    request.DocNum = result1.Item2;                   
                    _context.SaveChanges();
                    _context.Database.CommitTransaction();
                    return GetOrder();
                }             
            }
            catch
            {
                _context.Database.RollbackTransaction();
                throw;
            }          
            return GetOrder();
        }
    
    }
}
