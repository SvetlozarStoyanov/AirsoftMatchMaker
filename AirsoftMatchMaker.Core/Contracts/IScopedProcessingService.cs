using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirsoftMatchMaker.Core.Contracts
{
    public interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }
}
