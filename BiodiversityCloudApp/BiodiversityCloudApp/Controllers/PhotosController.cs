using AutoMapper;
using BiodiversityCloudApp.DTOs;
using BiodiversityCloudApp.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace BiodiversityCloudApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository;
        private readonly IObservationRepository _observationRepository;
        private readonly IMapper _mapper;

        public PhotosController(IPhotoRepository photoRepository, IObservationRepository observationRepository, IMapper mapper)
        {
            _photoRepository = photoRepository;
            _observationRepository = observationRepository;
            _mapper = mapper;
        }

        [HttpPost("upload/{observationId}")]
        public async Task<IActionResult> UploadPhoto(Guid observationId, IFormFile file, [FromForm] string description, [FromForm] string fileUrl)
        {
            var observation = await _observationRepository.GetByIdAsync(observationId);
            if (observation == null)
                return NotFound("Observation not found.");

            if (string.IsNullOrEmpty(fileUrl))
                return BadRequest("File URL is required.");

            var photo = new Photo
            {
                Id = Guid.NewGuid(),
                ObservationId = observationId,
                Url = fileUrl,
                Description = description
            };

            await _photoRepository.AddAsync(photo);

            // Check if photo was actually saved
            var savedPhoto = await _photoRepository.GetByObservationIdAsync(observationId);
            if (!savedPhoto.Any())
            {
                return StatusCode(500, "Photo was not saved in the database.");
            }

            return Ok(new { message = "Photo uploaded successfully", photo = _mapper.Map<PhotoDto>(photo) });
        }

        [HttpGet("{observationId}")]
        public async Task<IActionResult> GetPhotos(Guid observationId)
        {
            var photos = await _photoRepository.GetByObservationIdAsync(observationId);
            if (photos == null || !photos.Any())
                return NotFound("No photos found for this observation.");

            return Ok(_mapper.Map<IEnumerable<PhotoDto>>(photos));
        }

        [HttpGet("photo/{photoId}")]
        public async Task<ActionResult<PhotoDto>> GetPhotoById(Guid photoId)
        {
            var photo = await _photoRepository.GetByIdAsync(photoId);
            if (photo == null)
                return NotFound(new { message = "Photo not found" });

            return Ok(_mapper.Map<PhotoDto>(photo));
        }

        [HttpDelete("{photoId}")]
        public async Task<IActionResult> DeletePhoto(Guid photoId)
        {
            var photo = await _photoRepository.GetByIdAsync(photoId);
            if (photo == null)
                return NotFound(new { message = "Photo not found" });

            await _photoRepository.DeleteAsync(photo);
            return Ok(new { message = "Photo deleted successfully" });
        }

        [HttpPut("{photoId}")]
        public async Task<IActionResult> UpdatePhoto(Guid photoId, [FromBody] PhotoDto photoDto)
        {
            var photo = await _photoRepository.GetByIdAsync(photoId);
            if (photo == null)
                return NotFound(new { message = "Photo not found" });

            // Update fields
            photo.Url = photoDto.Url;
            photo.Description = photoDto.Description;

            await _photoRepository.UpdateAsync(photo);
            return Ok(new { message = "Photo updated successfully", photo = _mapper.Map<PhotoDto>(photo) });
        }
    }
}

