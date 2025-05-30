using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_BASE.Application.Interfaces.UserExternal
{
    public interface ICurrentUserService
    {
        string? UserName { get; }
    }
}
