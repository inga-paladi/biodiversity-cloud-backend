using AutoMapper;
using BiodiversityCloudApp.DTOs.Observations;
using BiodiversityCloudApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using BiodiversityCloudApp.Models;

namespace BiodiversityCloudApp.Controllers
{
    [Route("api/observations")]
    [ApiController]
    public class ObservationsController(IObservationRepository observationRepository, IMapper mapper, IPhotoRepository photoRepository) : ControllerBase
    {
        private readonly IObservationRepository _observationRepository = observationRepository;
        private readonly IMapper _mapper = mapper;
        private readonly IPhotoRepository _photoRepository = photoRepository;
        private readonly string _hackUserId = "00000000-0000-0000-0000-000000000001"; // just a fix.

        // GET: /api/observations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObservationDto>>> List()
        {
            var observations = await _observationRepository.GetObservationsAsync(Guid.Parse(_hackUserId));
            return Ok(_mapper.Map<IEnumerable<ObservationDto>>(observations));
        }

        // GET: /api/observations/{observationId}
        [HttpGet("{observationId}")]
        public async Task<ActionResult<ObservationDto>> Get(Guid observationId)
        {
            var observation = await _observationRepository.GetObservationAsync(observationId);
            if (observation == null)
                return NotFound(new { message = "Observation not found" });

            return Ok(_mapper.Map<ObservationDto>(observation));
        }

        // POST: /api/observations
        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateObservationDto createObservationDto)
        {
            var observation = _mapper.Map<Observation>(createObservationDto);
            observation.UserId = Guid.Parse(_hackUserId); // just a fix.

            await _observationRepository.AddAsync(observation);
            await _observationRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { observationId = observation.Id }, new { id = observation.Id });
        }

        // PATCH: /api/observations/{observationId}
        [HttpPatch("{observationId}")]
        public async Task<IActionResult> Update(Guid observationId, [FromBody] UpdateObservationDto updateObservationDto)
        {
            var observation = await _observationRepository.GetObservationAsync(observationId);
            if (observation == null)
                return NotFound(new { message = "Observation not found" });

            observation.UpdatedAt = DateTime.UtcNow;
            // Mapper is configured to ignore null values, so only non-null properties will be updated
            _mapper.Map(updateObservationDto, observation);
            await _observationRepository.UpdateAsync(observation);
            await _observationRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: /api/observations/{observationId}
        [HttpDelete("{observationId}")]
        public async Task<IActionResult> Delete(Guid observationId)
        {
            var observation = await _observationRepository.GetObservationAsync(observationId);
            if (observation == null)
                return NotFound(new { message = "Observation not found" });

            await _observationRepository.DeleteAsync(observation);
            await _observationRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
