/*using Microsoft.Extensions.Logging;
using Repositories;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Services.Semester
{
    public class SemesterService : ISemesterService
    {
        private readonly ModelMapper _modelMapper;
        private readonly ISemesterRepository _semesterRepository;
        private readonly IClassRepository _classRepository;
        private readonly ILogger<SemesterService> _logger;

        public SemesterService(
            ModelMapper modelMapper,
            ISemesterRepository semesterRepository,
            IClassRepository classRepository,
            ILogger<SemesterService> logger)
        {
            _modelMapper = modelMapper;
            _semesterRepository = semesterRepository;
            _classRepository = classRepository;
            _logger = logger;
        }

        public Response<Void> AddSemester(SemesterDTO semesterDto)
        {
            _logger.LogInformation("AddSemester(semesterDto: {semesterDto})", semesterDto);

            if (semesterDto.Code == null || semesterDto.StartDate == null || semesterDto.EndDate == null)
            {
                _logger.LogWarning("Create semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new Response<Void>(ServiceStatusCode.BAD_REQUEST_STATUS, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (semesterDto.EndDate < semesterDto.StartDate)
            {
                _logger.LogWarning("Create semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new Response<Void>(ServiceStatusCode.BAD_REQUEST_STATUS, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            var semester = _modelMapper.Map<Semester>(semesterDto);
            _semesterRepository.Save(semester);

            _logger.LogInformation("Create semester success");
            return new Response<Void>(ServiceStatusCode.OK_STATUS, ServiceMessage.SUCCESS_MESSAGE);
        }

        public Response<HashSet<SemesterDTO>> GetSemester(string code)
        {
            _logger.LogInformation("GetSemester(code: {code})", code);

            if (code == null)
                code = "";

            var semesterSet = _semesterRepository.GetSemester("%" + code + "%");
            var semesterDtoSet = semesterSet.Select(semesterEntity => _modelMapper.Map<SemesterDTO>(semesterEntity)).ToHashSet();

            _logger.LogInformation("Get semester success");
            return new Response<HashSet<SemesterDTO>>(ServiceStatusCode.OK_STATUS, ServiceMessage.SUCCESS_MESSAGE, semesterDtoSet);
        }

        public Response<Void> UpdateSemester(SemesterDTO semesterDto)
        {
            _logger.LogInformation("UpdateSemester(semesterDto: {semesterDto})", semesterDto);

            if (semesterDto.Code == null || semesterDto.StartDate == null || semesterDto.EndDate == null)
            {
                _logger.LogWarning("Update semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new Response<Void>(ServiceStatusCode.BAD_REQUEST_STATUS, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (!_semesterRepository.ExistsById(semesterDto.Code))
            {
                _logger.LogWarning("Update semester: {0}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return new Response<Void>(ServiceStatusCode.BAD_REQUEST_STATUS, ServiceMessage.ID_NOT_EXIST_MESSAGE);
            }

            if (semesterDto.EndDate < semesterDto.StartDate)
            {
                _logger.LogWarning("Update semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new Response<Void>(ServiceStatusCode.BAD_REQUEST_STATUS, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            var semester = _modelMapper.Map<Semester>(semesterDto);
            _semesterRepository.Save(semester);

            _logger.LogInformation("Update semester success");
            return new Response<Void>(ServiceStatusCode.OK_STATUS, ServiceMessage.SUCCESS_MESSAGE);
        }

        public Response<Void> DeleteSemester(string code)
        {
            _logger.LogInformation("DeleteSemester(code: {code})", code);

            if (code == null)
            {
                _logger.LogWarning("Delete semester: {0}", ServiceMessage.INVALID_ARGUMENT_MESSAGE);
                return new Response<Void>(ServiceStatusCode.BAD_REQUEST_STATUS, ServiceMessage.INVALID_ARGUMENT_MESSAGE);
            }

            if (!_semesterRepository.ExistsById(code))
            {
                _logger.LogWarning("Delete semester: {0}", ServiceMessage.ID_NOT_EXIST_MESSAGE);
                return new Response<Void>(ServiceStatusCode.BAD_REQUEST_STATUS, ServiceMessage.ID_NOT_EXIST_MESSAGE);
            }

            var classId = FindClassBySemester(code).GetAwaiter().GetResult();
            if (classId > 0)
            {
                _logger.LogWarning("Delete semester: {0}", ServiceMessage.SEMESTER_HAS_ASSOCIATED_CLASSES_MESSAGE);
                return new Response<Void>(ServiceStatusCode.BAD_REQUEST_STATUS, ServiceMessage.SEMESTER_HAS_ASSOCIATED_CLASSES_MESSAGE);
            }

            _semesterRepository.Delete(code);

            _logger.LogInformation("Delete semester success");
            return new Response<Void>(ServiceStatusCode.OK_STATUS, ServiceMessage.SUCCESS_MESSAGE);
        }

        private async Task<int> FindClassBySemester(string semesterCode)
        {
            return await _classRepository.FindClassBySemesterAsync(semesterCode);
        }
    }
}
*/