using System;
using System.Collections.Generic;
using System.Linq;
using DOrderPurchase.Features.Auth.Dto;
using OrderPurchase.WebApi.Features.Users.Entities;
using OrderPurchase.WebApi.Features.Common.Entities;
using OrderPurchase.WebApi.Infraestructure;
using Microsoft.Extensions.Configuration;
using OrderPurchase.WebApi.Helpers;
using Sap.Data.Hana;
using OrderPurchase.WebApi.Features.ServiceLayer;
using OrderPurchase.WebApi.Features.ServiceLayer.Dto;

namespace OrderPurchase.WebApi.Features.Users
{
    public class UserService
    {
        private readonly OrderPurchaseDbContext _OrderPurchaseDbContext;
        private readonly IConfiguration _configuration;
        private readonly HanaDbContext _hanaDbContext;
        private readonly AuthSapServices _authSapService;
        private readonly OrderPurchaseServices _orderPurchaseServices;

        public UserService(OrderPurchaseDbContext OrderPurchaseDbContext, IConfiguration configuration, HanaDbContext hanaDbContext, AuthSapServices authSapService, OrderPurchaseServices orderPurchaseServices)
        {
            _OrderPurchaseDbContext = OrderPurchaseDbContext;
            _configuration = configuration;
            _hanaDbContext = hanaDbContext;
            _authSapService = authSapService;
            _orderPurchaseServices = orderPurchaseServices;
        }

        public List<UserDto> Get()
        {
            var users = _OrderPurchaseDbContext.User.ToList();
            var themes = _OrderPurchaseDbContext.Theme.ToList();
            var roles = _OrderPurchaseDbContext.Role.ToList();

            var result = (from u in users
                          join r in roles on u.RoleId equals r.RoleId into userRole
                          from r in userRole.DefaultIfEmpty()
                          join t in themes on u.ThemeId equals t.ThemeId into themeUser
                          from t in themeUser.DefaultIfEmpty()
                          select new UserDto
                          {
                              Active = u.Active,
                              Email = u.Email,
                              Name = u.Name,
                              Password = null,
                              RoleId = u.RoleId,
                              ThemeId = u.ThemeId,
                              UserId = u.UserId,
                              UserName = u.UserName,
                              Role = r?.Description ??"ROL NO ASIGNADO",
                              Theme = t?.Description ?? "TEMA NO ASIGNADO"
                          }
                          ).ToList();
            return result;
        }

        public List<UserDto> Add(User user)
        {
            user.IsValid();
            if (string.IsNullOrEmpty(user.Password)) throw new Exception("Debe ingresar una contraseña");
            if (user.Password.Length <8) throw new Exception("Debe ingresar una contraseña que contenga al menos 8 caracteres");
            user.Active = true;
            user.Password = Helper.EncryptPassword(user.Password.Trim(), _configuration);
            user.UserName = user.UserName.Trim().ToLower();
            _OrderPurchaseDbContext.User.Add(user);
            _OrderPurchaseDbContext.SaveChanges();
            return Get();
        }


        public List<UserDto> Edit(User user)
        {
            user.IsValid();
            if (!string.IsNullOrEmpty(user.Password))
            {
                if (user.Password.Length < 8) throw new Exception("Debe ingresar una contraseña que contenga al menos 8 caracteres");
                user.Password = Helper.EncryptPassword(user.Password.Trim(), _configuration);
            }
            var currentUser = _OrderPurchaseDbContext.User.Where(x => x.UserId == user.UserId).FirstOrDefault();
            currentUser.Name = user.Name;
            currentUser.Email = user.Email;
            currentUser.RoleId = user.RoleId;
            currentUser.ThemeId = user.ThemeId;
            currentUser.Active = user.Active;

            _OrderPurchaseDbContext.User.Update(currentUser);
            currentUser.Password = user.Password;
            _OrderPurchaseDbContext.SaveChanges();
            return Get();
        }

        public List<Theme> GetThemes()
        {
            var themes = _OrderPurchaseDbContext.Theme.ToList();
            return themes;
        }

        public List<string> GetSellersSAP()
        {
            List<string> result = new List<string>();
            _hanaDbContext.Conn.Open();
            string query = $@"SELECT ""SlpName"" FROM ""TEST_CHAMER"".""OSLP"" ";
            HanaCommand selectCmd = new HanaCommand(query, _hanaDbContext.Conn);
            HanaDataReader dr = selectCmd.ExecuteReader();
            while (dr.Read())
            {
                string slpName = dr.GetString(0);
                result.Add(slpName);
            }
            dr.Close();
            _hanaDbContext.Conn.Close();
            return result;
        }

 

    }
}
