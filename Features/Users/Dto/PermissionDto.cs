using System;
using OrderPurchase.WebApi.Features.Users.Entities;

namespace OrderPurchase.WebApi.Features.Users.Dto
{
    public class PermissionDto: Permission
    {
        public int RoleId { get; set; }
    }
}
