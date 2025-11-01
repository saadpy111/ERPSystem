using Hr.Domain.Entities;

namespace Hr.Application.Contracts.Persistence.Repositories
{
    public interface IApplicantRepository
    {
        Task<Applicant> AddAsync(Applicant applicant);
        Task<Applicant?> GetByIdAsync(int id);
        Task<IEnumerable<Applicant>> GetAllAsync();
        Task<IEnumerable<Applicant>> GetByJobIdAsync(int jobId);
        void Update(Applicant applicant);
        void Delete(Applicant applicant);
    }
}
