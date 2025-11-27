//using Events.HrEvents;
//using MediatR;
//using Report.Application.Contracts.Persistence.Repositories;
//using Report.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace Report.Application.Consumers.HrConsumers
//{
//    public class EmployeeContractCreatedConsumer : INotificationHandler<EmployeeContractCreatedEvent>
//    {
//        private readonly IEmployeeReportRepository _employeeReportRepository;
//        private readonly IUnitOfWork _unitOfWork;

//        public EmployeeContractCreatedConsumer(IEmployeeReportRepository employeeReportRepository
//                                               , IUnitOfWork unitOfWork)
//        {
//            _employeeReportRepository = employeeReportRepository;
//             _unitOfWork = unitOfWork;
//        }
//        public async Task Handle(EmployeeContractCreatedEvent e, CancellationToken cancellationToken)
//        {
//            var reportRow = new EmployeeReport
//            {
//                EmployeeId = e.EmployeeId,
//                FullName = e.FullName,
//                DepartmentName = e.DepartmentName,
//                JobTitle = e.JobTitle,
//                ManagerName = e.ManagerName,
//                HireDate = e.HireDate,
//                Salary = e.Salary,
//                ContractType = e.ContractType,
//                UpdatedAt = DateTime.UtcNow
//            };

//            await  _employeeReportRepository.AddAsync(reportRow);
//            await _unitOfWork.SaveChangesAsync();
//        }
//    }
//}
