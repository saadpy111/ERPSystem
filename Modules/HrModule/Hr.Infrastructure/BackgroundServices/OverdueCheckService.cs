using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using Hr.Domain.Enums;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hr.Infrastructure.BackgroundServices
{
    public class OverdueCheckService : BackgroundService
    {
        private readonly ILoanInstallmentRepository _repo;
        private readonly IUnitOfWork _unitOfWork;

        public OverdueCheckService(ILoanInstallmentRepository repo, IUnitOfWork unitOfWork)
        {
            _repo = repo;
            _unitOfWork = unitOfWork;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var pending = await _repo.GetAllPendingAsync();
                foreach (var inst in pending.Where(i => i.DueDate < DateTime.UtcNow))
                {
                    inst.Status = InstallmentStatus.Overdue;
                    _repo.Update(inst);
                }
                await _unitOfWork.SaveChangesAsync();
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }

}
