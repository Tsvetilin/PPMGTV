using AutoMapper;
using Common.Constants;
using Data.Models;
using Services.Contracts.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Web.Areas.Admin.Models.Users
{
    public class UserPermissionsModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        [DisplayName("Администратор")]
        public bool IsAdmin { get; set; }

        [DisplayName("Редактор")]
        public bool IsEditor { get; set; }
    }
}
