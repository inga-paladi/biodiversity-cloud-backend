using AutoMapper;
using BiodiversityCloudApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BiodiversityCloudApp.DTOs;

namespace BiodiversityCloudApp.Controllers
{
    [Route("api/observations")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        private readonly IObservationRepository _observationRepository;
        private readonly IMapper _mapper;   
        private readonly IPhotoRepository _photoRepository;

        public ObservationsController(IObservationRepository observationRepository, IMapper mapper, IPhotoRepository photoRepository)
        {
            _observationRepository = observationRepository;
            _mapper = mapper;
            _photoRepository = photoRepository;
        }

        // POST: api/Observations
        [HttpPost]
        public async Task<ActionResult<ObservationDto>> CreateObservation(ObservationDto observationDto)
        {
            var observation = _mapper.Map<Observation>(observationDto);
            observation.Id = Guid.NewGuid();

            await _observationRepository.AddAsync(observation);
            await _observationRepository.SaveChangesAsync();

            var createdObservationDto = _mapper.Map<ObservationDto>(observation);


            return CreatedAtAction(nameof(GetObservation), new { id = observationDto.Id }, observationDto);
        }


        // GET: api/Observations
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObservationDto>>> GetObservation()
        {
            var observations = await _observationRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ObservationDto>>(observations));
        }

        // GET: api/Observations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult> GetObservationById(Guid id)
        {
            var observation = await _observationRepository.GetByIdAsync(id);
            if (observation == null)
            {
                return NotFound(new { message = "Observation not found" });
            }

            observation.Photos = (await _photoRepository.GetByObservationIdAsync(id)).ToList();
            return Ok(_mapper.Map<ObservationDto>(observation));
        }

        // PUT: api/Observations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObservation(Guid id, ObservationDto observationDto)
        {
            if (id != observationDto.Id)
            {
                return BadRequest(new { message = "Observation ID mismatch" });
            }

            var existingObservation = await _observationRepository.GetByIdAsync(id);
            if (existingObservation == null)
            {
                return NotFound(new { message = "Observation not found" });
            }

            _mapper.Map(observationDto, existingObservation);
            await _observationRepository.UpdateAsync(existingObservation);
            await _observationRepository.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Observations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObservation(Guid id)
        {
            var observation = await _observationRepository.GetByIdAsync(id);
            if (observation == null)
            {
               return NotFound(new { message = "Observation not found" });
            }
            await _observationRepository.DeleteAsync(observation);
            await _observationRepository.SaveChangesAsync();
            return NoContent();

        }   
    }
}