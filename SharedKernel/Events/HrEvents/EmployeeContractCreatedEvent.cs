using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.HrEvents
{
    public class EmployeeContractCreatedEvent : INotification
    {
        public int EmployeeId { get; set; }

        // Employee Info
        public string FullName { get; set; } = string.Empty;
        public string DepartmentName { get; set; } = string.Empty;
        public string JobTitle { get; set; } = string.Empty;
        public string ManagerName { get; set; } = string.Empty;
        public DateTime HireDate { get; set; }

        // Contract Info
        public decimal Salary { get; set; }
        public string ContractType { get; set; } = string.Empty;
    }
}
