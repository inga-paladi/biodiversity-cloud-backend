using AutoMapper;
using BiodiversityCloudApp.DTOs.ObservationRecords;
using BiodiversityCloudApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.Controllers
{
    [ApiController]
    [Route("api/observations/{observationId}/records")]
    public class ObservationRecordsController(IObservationRecordRepository observationRecordRepository, IMapper mapper) : ControllerBase
    {
        private readonly IObservationRecordRepository _observationRecordRepository = observationRecordRepository;
        private readonly IMapper _mapper = mapper;

        // GET: /api/observations/{observationId}/records
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObservationRecordDto>>> List(Guid observationId)
        {
            var record = await _observationRecordRepository.GetAllRecordsAsync(observationId);
            return Ok(_mapper.Map<IEnumerable<ObservationRecordDto>>(record));
        }

        // GET: /api/observations/{observationId}/records/{recordId}
        [HttpGet("{recordId}")]
        public async Task<ActionResult<ObservationRecordDto>> Get(Guid observationId, Guid recordId)
        {
            var record = await _observationRecordRepository.GetRecordAsync(recordId);
            if (record == null)
                return NotFound(new { message = "Observation record not found" });

            return Ok(_mapper.Map<ObservationRecordDto>(record));
        }

        // POST: /api/observations/{observationId}/records
        [HttpPost]
        public async Task<ActionResult<Guid>> Create(Guid observationId, [FromBody] CreateObservationRecordDto createObservationRecordDto)
        {
            var record = _mapper.Map<ObservationRecord>(createObservationRecordDto);
            record.ObservationId = observationId;

            await _observationRecordRepository.AddAsync(record);
            await _observationRecordRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { observationId, recordId = record.Id }, new { id = record.Id });
        }

        // PATCH: /api/observations/{observationId}/records/{recordId}
        [HttpPatch("{recordId}")]
        public async Task<IActionResult> Update(Guid observationId, Guid recordId, [FromBody] UpdateObservationRecordDto updateObservationRecordDto)
        {
            var record = await _observationRecordRepository.GetRecordAsync(recordId);
            if (record == null)
                return NotFound(new { message = "Observation record not found" });

            // Mapper is configured to ignore null values, so only non-null properties will be updated
            _mapper.Map(updateObservationRecordDto, record);
            await _observationRecordRepository.UpdateAsync(record);
            await _observationRecordRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /api/observations/{observationId}/records/{recordId}
        [HttpDelete("{recordId}")]
        public async Task<IActionResult> Delete(Guid observationId, Guid recordId)
        {
            var record = await _observationRecordRepository.GetRecordAsync(recordId);
            if (record == null)
                return NotFound(new { message = "Observation record not found" });

            await _observationRecordRepository.DeleteAsync(record);
            await _observationRecordRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
