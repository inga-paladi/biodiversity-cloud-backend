using AutoMapper;
using BiodiversityCloudApp.DTOs;
using BiodiversityCloudApp.Repositories;
using Microsoft.AspNetCore.Mvc;
using BiodiversityCloudApp.Models;
using BiodiversityCloudApp.Common;

namespace BiodiversityCloudApp.Controllers
{
    // TODO: When making some changes to photos, check that the photo is really part
    // of the observation record and that the record is part of the observation.
    // This is important for the integrity of the data and security of the application.
    // This should be handled in the middleware.
    [Route("api/observations/{observationId}/records/{recordId}/photos")]
    [ApiController]
    public class PhotosController(IPhotoRepository photoRepository, IObservationRecordRepository recordRepository, IMapper mapper) : ControllerBase
    {
        private readonly IPhotoRepository _photoRepository = photoRepository;
        private readonly IObservationRecordRepository _recordRepository = recordRepository;
        private readonly IMapper _mapper = mapper;

        // POST: /api/observations/{observationId}/records/{recordId}/photos
        [HttpPost]
        public async Task<IActionResult> Add(Guid observationId, Guid recordId, IFormFile photo)
        {
            // This check should be done in the middleware.
            var record = await _recordRepository.GetByIdAsync(recordId);
            if (record == null)
                return NotFound("Record not found.");

            if (photo == null || photo.Length == 0)
                return BadRequest("Invalid photo file.");

            var mimeType = photo.ContentType;
            if (!PhotoMimeType.SupportedMimeTypes.Contains(mimeType))
                return BadRequest("Unsupported photo file type.");

            var photoModel = new Photo
            {
                RecordId = recordId,
                Record = record,
                FileType = mimeType,
            };

            using (var stream = new FileStream(Path.Combine(photoModel.Path, photoModel.Id.ToString()), FileMode.Create))
            {
                await photo.CopyToAsync(stream);
            }

            await _photoRepository.AddAsync(photoModel);

            return Ok(new { photoId = photoModel.Id });
        }

        // GET: /api/observations/{observationId}/records/{recordId}/photos/{photoId}
        [HttpGet("{photoId}")]
        public async Task<IActionResult> Get(Guid observationId, Guid recordId, Guid photoId)
        {
            var photo = await _photoRepository.GetByIdAsync(photoId);
            if (photo == null)
                return NotFound();

            var photoPath = Path.Combine(Directory.GetCurrentDirectory(), photo.Path, photo.Id.ToString());
            if (!System.IO.File.Exists(photoPath))
                return NotFound();

            return PhysicalFile(photoPath, photo.FileType);
        }

        // DELETE: /api/observations/{observationId}/records/{recordId}/photos/{photoId}
        [HttpDelete("{photoId}")]
        public async Task<IActionResult> Delete(Guid observationId, Guid recordId, Guid photoId)
        {
            var photo = await _photoRepository.GetByIdAsync(photoId);
            if (photo == null)
                return NotFound();

            var photoPath = Path.Combine(photo.Path, photo.Id.ToString());
            if (!System.IO.File.Exists(photoPath))
                return NotFound();

            await _photoRepository.DeleteAsync(photo);
            System.IO.File.Delete(photoPath);
            return Ok();
        }
    }
}

