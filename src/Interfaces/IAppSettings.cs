using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Darixidor.Blazor.Settings.Interfaces
{
    public interface IAppSettings<T>
    {
        Task<T> GetSettings();

        Task<string> GetEnviroment();
    }
}
