using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AbobusMobile.BLL.Services.Abstractions.Error
{
    public interface IErrorHandlingService
    {
        Task AddGlobalError();
    }
}
