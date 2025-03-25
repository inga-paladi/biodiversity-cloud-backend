using AutoMapper;
using BiodiversityCloudApp.DTOs;
using BiodiversityCloudApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiodiversityCloudApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObservationsController : ControllerBase
    {
        private readonly IObservationRepository _observationRepository;
        private readonly IMapper _mapper;
        private readonly IPhotoRepository _photoRepository;
        private readonly ICommentRepository _commentRepository;

        public ObservationsController(IObservationRepository observationRepository, IMapper mapper, IPhotoRepository photoRepository, ICommentRepository commentRepository)
        {
            _observationRepository = observationRepository;
            _mapper = mapper;
            _photoRepository = photoRepository;
            _commentRepository = commentRepository;
        }

        [HttpPost]
        public async Task<ActionResult<ObservationDto>> CreateObservation(ObservationDto observationDto)
        {
            var existingObservation = await _observationRepository.GetByIdAsync(observationDto.Id);
            if (existingObservation != null)
            {
                // If observation exists and incoming is newer, update it
                if (observationDto.UpdatedAt > existingObservation.UpdatedAt)
                {
                    existingObservation.UpdatedAt = DateTime.UtcNow;
                    _mapper.Map(observationDto, existingObservation);
                    await _observationRepository.UpdateAsync(existingObservation);
                    await _observationRepository.SaveChangesAsync();
                    return Ok(_mapper.Map<ObservationDto>(existingObservation));
                }

                return Conflict(new { message = "Observation already exists and is newer or same." });
            }

            var observation = _mapper.Map<Observation>(observationDto);
            observation.Id = observationDto.Id == Guid.Empty ? Guid.NewGuid() : observationDto.Id;
            observation.CreatedAt = DateTime.UtcNow;
            observation.UpdatedAt = DateTime.UtcNow;

            await _observationRepository.AddAsync(observation);
            await _observationRepository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetObservationById), new { id = observation.Id }, _mapper.Map<ObservationDto>(observation));
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncObservations([FromBody] IEnumerable<ObservationDto> observationsDto)
        {
            foreach (var dto in observationsDto)
            {
                var existing = await _observationRepository.GetByIdAsync(dto.Id);
                if (existing == null)
                {
                    var newObservation = _mapper.Map<Observation>(dto);
                    await _observationRepository.AddAsync(newObservation);
                }
                else
                {
                    if (dto.UpdatedAt > existing.UpdatedAt)
                    {
                        existing.UpdatedAt = DateTime.UtcNow;
                        _mapper.Map(dto, existing);
                        await _observationRepository.UpdateAsync(existing);
                    }
                }
            }

            await _observationRepository.SaveChangesAsync();
            return Ok(new { message = "Sync completed." });
        }

        [HttpGet("sync")]
        public async Task<IActionResult> GetObservationsSince([FromQuery] DateTime since)
        {
            var updatedObservations = await _observationRepository.GetUpdatedSinceAsync(since);
            return Ok(_mapper.Map<IEnumerable<ObservationDto>>(updatedObservations));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ObservationDto>>> GetObservation()
        {
            var observations = await _observationRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<ObservationDto>>(observations));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetObservationById(Guid id)
        {
            var observation = await _observationRepository.GetByIdAsync(id);
            if (observation == null)
                return NotFound(new { message = "Observation not found" });

            observation.Photos = (await _photoRepository.GetByObservationIdAsync(id)).ToList();
            observation.Comments = (await _commentRepository.GetByObservationIdAsync(id)).ToList();

            return Ok(_mapper.Map<ObservationDto>(observation));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateObservation(Guid id, ObservationDto observationDto)
        {
            if (id != observationDto.Id)
                return BadRequest(new { message = "Observation ID mismatch" });

            var existingObservation = await _observationRepository.GetByIdAsync(id);
            if (existingObservation == null)
                return NotFound(new { message = "Observation not found" });

            existingObservation.UpdatedAt = DateTime.UtcNow;
            _mapper.Map(observationDto, existingObservation);
            await _observationRepository.UpdateAsync(existingObservation);
            await _observationRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteObservation(Guid id)
        {
            var observation = await _observationRepository.GetByIdAsync(id);
            if (observation == null)
                return NotFound(new { message = "Observation not found" });

            await _observationRepository.DeleteAsync(observation);
            await _observationRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
