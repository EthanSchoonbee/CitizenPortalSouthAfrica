using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitizenPortalSouthAfrica.Resources
{
    internal interface IFileManagementService
    {
        Task<(List<byte[]> Files, List<string> FileNames)> AttachFilesAsync();
    }
}
