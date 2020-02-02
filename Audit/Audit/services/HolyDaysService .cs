using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AutoMapper;

using Audit.interfaces;
using audit.db;
using Audit.models;
using Audit.exceptions;
using Audit.objects;
using Audit.Helper;

namespace Audit.services
{
    public class HolyDaysService : IHolyDaysService
    {
        private DBDictinaryContext _dbcontext;
        private IHttpContextAccessor _httpContextAccessor;

        public HolyDaysService(DBDictinaryContext dbcontext, IHttpContextAccessor httpContextAccessor) {
            _dbcontext = dbcontext;
            _httpContextAccessor = httpContextAccessor;
        }

        public IEnumerable<DateTime> HolyWorkDays(DateTime DFrom)
        {
            return _dbcontext.Holidays.Where(d => d.Data.Date >= DFrom && d.Hours == 8).Select(l => l.Data).AsEnumerable();
        }

        public IEnumerable<DateTime> HolyDays(DateTime DFrom)
        {
            return _dbcontext.Holidays.Where(d => d.Data.Date >= DFrom && d.Hours == 0).Select(l => l.Data).AsEnumerable();
        }

    }
}
