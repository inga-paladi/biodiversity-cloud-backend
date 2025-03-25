using AutoMapper;
using BiodiversityCloudApp.DTOs;
using BiodiversityCloudApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MicroObservationsController : ControllerBase
    {
        private readonly IMicroObservationRepository _microObservationRepository;
        private readonly IMapper _mapper;

        public MicroObservationsController(IMicroObservationRepository microObservationRepository, IMapper mapper)
        {
            _microObservationRepository = microObservationRepository;
            _mapper = mapper;
        }

        // GET: api/MicroObservations/observation/{observationId}
        [HttpGet("observation/{observationId}")]
        public async Task<ActionResult<IEnumerable<MicroObservationDto>>> GetByObservationId(Guid observationId)
        {
            var microObservations = await _microObservationRepository.GetByObservationIdAsync(observationId);
            return Ok(_mapper.Map<IEnumerable<MicroObservationDto>>(microObservations));
        }

        // GET: api/MicroObservations/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<MicroObservationDto>> GetById(Guid id)
        {
            var microObservation = await _microObservationRepository.GetByIdAsync(id);
            if (microObservation == null)
                return NotFound(new { message = "MicroObservation not found" });

            return Ok(_mapper.Map<MicroObservationDto>(microObservation));
        }

        // POST: api/MicroObservations
        [HttpPost]
        public async Task<ActionResult<MicroObservationDto>> Create(MicroObservationDto dto)
        {
            var microObservation = _mapper.Map<MicroObservation>(dto);
            microObservation.Id = Guid.NewGuid();
            microObservation.Timestamp = DateTime.UtcNow;

            await _microObservationRepository.AddAsync(microObservation);
            await _microObservationRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = microObservation.Id }, _mapper.Map<MicroObservationDto>(microObservation));
        }

        // PUT: api/MicroObservations/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, MicroObservationDto dto)
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            var existing = await _microObservationRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "MicroObservation not found" });

            _mapper.Map(dto, existing);
            await _microObservationRepository.UpdateAsync(existing);
            await _microObservationRepository.SaveChangesAsync();

            return Ok(_mapper.Map<MicroObservationDto>(existing));
        }

        // DELETE: api/MicroObservations/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var existing = await _microObservationRepository.GetByIdAsync(id);
            if (existing == null)
                return NotFound(new { message = "MicroObservation not found" });

            await _microObservationRepository.DeleteAsync(existing);
            await _microObservationRepository.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/MicroObservations/sync
        [HttpPost("sync")]
        public async Task<IActionResult> SyncMicroObservations([FromBody] IEnumerable<MicroObservationDto> microDtos)
        {
            foreach (var dto in microDtos)
            {
                var existing = await _microObservationRepository.GetByIdAsync(dto.Id);
                if (existing == null)
                {
                    var newMicro = _mapper.Map<MicroObservation>(dto);
                    await _microObservationRepository.AddAsync(newMicro);
                }
                else
                {
                    if (dto.Timestamp > existing.Timestamp)
                    {
                        _mapper.Map(dto, existing);
                        await _microObservationRepository.UpdateAsync(existing);
                    }
                }
            }

            await _microObservationRepository.SaveChangesAsync();
            return Ok(new { message = "MicroObservations sync completed." });
        }
    }
}
