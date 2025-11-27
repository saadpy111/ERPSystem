﻿using AutoMapper;
using Hr.Application.Contracts.Persistence;
using Hr.Application.Contracts.Persistence.Repositories;
using MediatR;

namespace Hr.Application.Features.JobFeatures.UpdateJob
{
    public class UpdateJobHandler : IRequestHandler<UpdateJobRequest, UpdateJobResponse>
    {
        private readonly IJobRepository _jobRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateJobHandler(IJobRepository jobRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _jobRepository = jobRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UpdateJobResponse> Handle(UpdateJobRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var job = await _jobRepository.GetByIdAsync(request.Id);
                if (job == null)
                {
                    return new UpdateJobResponse
                    {
                        Success = false,
                        Message = "الوظيفة غير موجودة"
                    };
                }

                job.Title = request.Title;
                job.DepartmentId = request.DepartmentId;
                job.WorkType = request.WorkType;
                job.PublishedDate = request.PublishedDate;
                job.Status = request.Status;
                job.Responsibilities = request.Responsibilities;
                job.RequiredSkills = request.RequiredSkills;
                job.RequiredExperience = request.RequiredExperience;
                job.RequiredQualification = request.RequiredQualification;

                _jobRepository.Update(job);
                await _unitOfWork.SaveChangesAsync();

                var jobDto = _mapper.Map<DTOs.JobDto>(job);

                return new UpdateJobResponse
                {
                    Success = true,
                    Message = "تم تحديث الوظيفة بنجاح",
                    Job = jobDto
                };
            }
            catch (Exception ex)
            {
                return new UpdateJobResponse
                {
                    Success = false,
                    Message = "حدث خطأ أثناء تحديث الوظيفة"
                };
            }
        }
    }
}