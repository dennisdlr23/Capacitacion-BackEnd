using DOrderPurchase.Features.Auth.Dto;
using OrderPurchase.WebApi.Features.TypeDocuments.Dto;
using OrderPurchase.WebApi.Features.TypeDocuments.Entities;
using OrderPurchase.WebApi.Features.Users.Entities;
using OrderPurchase.WebApi.Infraestructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderPurchase.WebApi.Features.TypeDocuments.Services
{
    public class TypeDocumentServices
    {
        private readonly OrderPurchaseDbContext _context;

        public TypeDocumentServices(OrderPurchaseDbContext context)
        {
            _context = context;
        }

        public List<TypeDocumentDto> GetTypeDocument()
        {
            var result = (from t in _context.TypeDocument
                          join u in _context.User on t.CreatedBy equals u.UserId
                          join uUpdate in _context.User on t.UpdateBy equals uUpdate.UserId into updateUser
                          from uUpdate in updateUser.DefaultIfEmpty() 
                          select new TypeDocumentDto
                          {
                              Id = t.Id,
                              Name = t.Name,
                              CreatedDate = t.CreatedDate,
                              CreatedBy = t.CreatedBy,
                              CreatedByName = u.Name,
                              UpdateDate = t.UpdateDate,
                              UpdateBy = t.UpdateBy,
                              UpdatedByName = uUpdate != null ? uUpdate.Name : null 
                          }).ToList();
            return result;
        }

        public List<TypeDocumentDto> GetTypeDocumentById(int id)
        {
            var result = (from t in _context.TypeDocument
                          join u in _context.User on t.CreatedBy equals u.UserId
                          join uUpdate in _context.User on t.UpdateBy equals uUpdate.UserId into updateUser
                          from uUpdate in updateUser.DefaultIfEmpty() 
                          where t.Id == id
                          select new TypeDocumentDto
                          {
                              Id = t.Id,
                              Name = t.Name,
                              CreatedDate = t.CreatedDate,
                              CreatedBy = t.CreatedBy,
                              CreatedByName = u.Name,
                              UpdateDate = t.UpdateDate,
                              UpdateBy = t.UpdateBy,
                              UpdatedByName = uUpdate != null ? uUpdate.Name : null 
                          }).ToList();
            return result;
        }

        public List<TypeDocumentDto> Add(TypeDocument request)
        {
            request.IsValid();
            request.CreatedDate = DateTime.Now;
            _context.TypeDocument.Add(request);
            _context.SaveChanges();
            return GetTypeDocument();
        }

        public List<TypeDocumentDto> Edit(TypeDocument request, int userId)
        {
            request.IsValid();
            var existingDocument = _context.TypeDocument.FirstOrDefault(x => x.Id == request.Id);
            if (existingDocument == null)
            {
                throw new Exception("El documento seleccionado no existe");
            }

            existingDocument.Name = request.Name;
            existingDocument.UpdateDate = DateTime.Now;
   
            existingDocument.UpdateBy = userId;

            _context.SaveChanges();

            return GetTypeDocument();
        }
        public List<TypeDocumentDto> Delete(int id)
        {
            var documentToDelete = _context.TypeDocument.FirstOrDefault(x => x.Id == id);

            
            if (documentToDelete == null)
            {
                throw new Exception("El documento seleccionado no existe");
            }

           
            _context.TypeDocument.Remove(documentToDelete);
            _context.SaveChanges();

            return GetTypeDocument();
        }

    }
}
