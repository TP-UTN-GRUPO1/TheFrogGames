using Application.Abstraction;
using Contracts.Platform.Request;
using Contracts.Platform.Response;
using Domain.Entities;

namespace Application.Service
{
    public class PlatformService : IPlatformService
    {
        private readonly IPlatformRepository _platformRepo;

        public PlatformService(IPlatformRepository platformRepo)
        {
            _platformRepo = platformRepo;
        }
        public PlatformResponse CreatePlatform(CreatePlatformRequest request)
        {

            var platform = new Platform
            {
                Name = request.Name
            };
            bool success = _platformRepo.Create(platform);

            if (!success)
            {
                throw new ApplicationException("Error al crear el género en la base de datos.");
            }
            var response = new PlatformResponse
            {
                Id = platform.Id,
                Name = platform.Name
            };

            return response;
        }

        public List<PlatformResponse> GetPlatform()
        {
            var platform = _platformRepo.GetAll();

            var responseList = platform.Select(p => new PlatformResponse
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();

            return responseList;
        }
        public PlatformResponse UpdatePlatform(UpdatePlatformRequest request)
        {
            var existingPlatform = _platformRepo.GetById(request.Id, trackChanges: true);
            if (existingPlatform == null)
            {
                throw new Exception($"Plataforma con ID {request.Id} no encontrado.");
            }
            existingPlatform.Name = request.NewName;
            bool success = _platformRepo.Update(existingPlatform);
            if (!success)
            {
                throw new ApplicationException("Error al actualizar la plataforma en la base de datos.");
            }
            return new PlatformResponse
            {
                Id = existingPlatform.Id,
                Name = existingPlatform.Name
            };
        }
        public bool DeletePlatform(int id)
        {
            var platformToDelete = _platformRepo.GetById(id);

            if (platformToDelete == null)
            {
                return true;
            }

            return _platformRepo.Delete(platformToDelete);
        }
    }
}