using Souq.Core.Entites.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Souq.Core.Service.Contarct
{
   public interface ITokenService
    {

        Task<string> CreateTokenAsyn(AppUser user);
    }
}
