using Api.Dto.Shared;
using Api.Dto.Shared.plms.ManagementService.Model.DTO;
using Api.Services.Semesters;
using BusinessObjects.Models;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Semesters
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassRepository _classRepository;
        private readonly ILogger<SemesterService> _logger;
        private static readonly String SEMESTER_HAS_ASSOCIATED_CLASSES_MESSAGE = "Some classes created in this semester.";

        public SemesterService(
            ISemesterRepository semesterRepository,
            IClassRepository classRepository,
            ILogger<SemesterService> logger)
        {
            _semesterRepository = semesterRepository;
            _classRepository = classRepository;
            _logger = logger;
        }

        public async Task AddSemester(SemesterDTO semesterDto)
        {
            _logger.LogInformation("AddSemester(semesterDto: {semesterDto})", semesterDto);

            if (semesterDto.Code == null || semesterDto.StartDate == null || semesterDto.EndDate == null)
            {
                _logger.LogWarning("Create semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (semesterDto.EndDate < semesterDto.StartDate)
            {
                _logger.LogWarning("Create semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            var semester = MapToSemesterEntity(semesterDto);
            await _semesterRepository.SaveAsync(semester);

            _logger.LogInformation("Create semester success");
        }

        public async Task<HashSet<SemesterDTO>> GetSemester(string code)
        {
            _logger.LogInformation("GetSemester(code: {code})", code);

            if (code == null)
                code = "";

            var semesterSet = await _semesterRepository.GetSemester("%" + code + "%");
            var semesterDtoSet = semesterSet.Select(semesterEntity => MapToSemesterDTO(semesterEntity)).ToHashSet();

            _logger.LogInformation("Get semester success");
            return semesterDtoSet;
        }

        public async Task UpdateSemester(SemesterDTO semesterDto)
        {
            _logger.LogInformation("UpdateSemester(semesterDto: {semesterDto})", semesterDto);

            if (semesterDto.Code == null || semesterDto.StartDate == null || semesterDto.EndDate == null)
            {
                _logger.LogWarning("Update semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (!await _semesterRepository.ExistsByIdAsync(semesterDto.Code))
            {
                _logger.LogWarning("Update semester: {0}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                throw new ArgumentException(ServiceMessage.ID_NOT_EXIST_MESSAGE);
            }

            if (semesterDto.EndDate < semesterDto.StartDate)
            {
                _logger.LogWarning("Update semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            var semester = MapToSemesterEntity(semesterDto);
            await _semesterRepository.SaveAsync(semester);

            _logger.LogInformation("Update semester success");
        }

        public async Task DeleteSemester(string code)
        {
            _logger.LogInformation("DeleteSemester(code: {code})", code);

            if (code == null)
            {
                _logger.LogWarning("Delete semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (!await _semesterRepository.ExistsByIdAsync(code))
            {
                _logger.LogWarning("Delete semester: {0}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                throw new ArgumentException(ServiceMessage.ID_NOT_EXIST_MESSAGE);
            }

            var classId = await FindClassBySemester(code);
            if (classId > 0)
            {
                _logger.LogWarning("Delete semester: {0}", SEMESTER_HAS_ASSOCIATED_CLASSES_MESSAGE);
                throw new ArgumentException(SEMESTER_HAS_ASSOCIATED_CLASSES_MESSAGE);
            }

            await _semesterRepository.DeleteAsync(code);

            _logger.LogInformation("Delete semester success");
        }

        private async Task<int> FindClassBySemester(string semesterCode)
        {
            return await _classRepository.FindClassBySemesterAsync(semesterCode);
        }

        private Semester MapToSemesterEntity(SemesterDTO semesterDto)
        {
            return new Semester
            {
                Code = semesterDto.Code,
                StartDate = semesterDto.StartDate,
                EndDate = semesterDto.EndDate
            };
        }

        private SemesterDTO MapToSemesterDTO(Semester semesterEntity)
        {
            return new SemesterDTO
            {
                Code = semesterEntity.Code,
                StartDate = (DateTime)semesterEntity.StartDate,
                EndDate = (DateTime)semesterEntity.EndDate
            };
        }
    }
}
