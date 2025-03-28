using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentShopVT.Models
{
    class ModificaRedeSocialModel
    {
        private readonly HttpClient _httpclient;
        public ModificaRedeSocialModel()
        {
            _httpclient = new HttpClient()
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

    }
}
