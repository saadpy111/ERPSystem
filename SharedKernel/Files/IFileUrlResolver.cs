using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedKernel.Core.Files
{
    public interface IFileUrlResolver
    {
        string? Resolve(string? relativePath);
    }
}
