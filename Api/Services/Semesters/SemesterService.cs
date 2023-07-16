using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Dto.Shared.plms.ManagementService.Model.DTO;
using Api.Services.Constant;
using BusinessObjects.Models;
using FPLMS.Api.Dto;
using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;

namespace Api.Services.Semesters
{
    public class SemesterService : ISemesterService
    {
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassRepository _classRepository;
        private readonly ILogger<SemesterService> _logger;
        private const string SEMESTER_HAS_ASSOCIATED_CLASSES_MESSAGE = "Semester still has class";
        public SemesterService(
            ILogger<SemesterService> logger)
        {
            _semesterRepository = SemesterRepository.Instance;
            _classRepository = ClassRepository.Instance;
            _logger = logger;
        }

        public async Task<ResponseDto<List<SemesterDTO>>> GetSemester(string code)
        {
            _logger.LogInformation("GetSemester(code: {code})", code);

            if (code == null)
                code = "";

            var semesterSet = await _semesterRepository.GetSemester("%" + code + "%");
            var semesterDtoSet = semesterSet.Select(semesterEntity => MapToSemesterDTO(semesterEntity)).ToList();

            _logger.LogInformation("Get semester success");
            return new ResponseDto<List<SemesterDTO>>
            {
                code = 200,
                message = "Success",
                data = semesterDtoSet
            };
        }

        public async Task<ResponseDto<object>> AddSemester(SemesterDTO semesterDto)
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

            return new ResponseDto<object>
            {
                code = 200,
                message = "Success",
                data = null
            };
        }

        public async Task<ResponseDto<object>> UpdateSemester(SemesterDTO semesterDto)
        {
            _logger.LogInformation("UpdateSemester(semesterDto: {semesterDto})", semesterDto);

            if (semesterDto.Code == null || semesterDto.StartDate == null || semesterDto.EndDate == null)
            {
                _logger.LogWarning("Update semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (!await _semesterRepository.ExistsById(semesterDto.Code))
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

            return new ResponseDto<object>
            {
                code = 200,
                message = "Success",
                data = null
            };
        }

        public async Task<ResponseDto<object>> DeleteSemester(string code)
        {
            _logger.LogInformation("DeleteSemester(code: {code})", code);

            if (code == null)
            {
                _logger.LogWarning("Delete semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                throw new ArgumentException(ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (!await _semesterRepository.ExistsById(code))
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

            return new ResponseDto<object>
            {
                code = 200,
                message = "Success",
                data = null
            };
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
