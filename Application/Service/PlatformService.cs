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
            if (request == null)
                throw new ArgumentException("Solicitud inválida.");

            var normalizedName = (request.Name ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(normalizedName))
                throw new ArgumentException("El nombre de la plataforma no puede estar vacío.");

            var exists = _platformRepo.GetAll()
                .Any(p => p.Name.Equals(normalizedName, StringComparison.OrdinalIgnoreCase));

            if (exists)
                throw new InvalidOperationException($"Ya existe una plataforma con el nombre '{normalizedName}'.");

            var platform = new Platform
            {
                Name = normalizedName
            };
            bool success = _platformRepo.Create(platform);

            if (!success)
            {
                throw new ApplicationException("Error al crear la plataforma en la base de datos.");
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
            if (request == null)
                throw new ArgumentException("Solicitud inválida.");

            var newName = (request.NewName ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(newName))
                throw new ArgumentException("El nuevo nombre de la plataforma no puede estar vacío.");

            var existingPlatform = _platformRepo.GetById(request.Id, trackChanges: true);
            if (existingPlatform == null)
            {
                throw new Exception($"Plataforma con ID {request.Id} no encontrado.");
            }

            var conflict = _platformRepo.GetAll()
                .Any(p => p.Id != request.Id && p.Name.Equals(newName, StringComparison.OrdinalIgnoreCase));

            if (conflict)
                throw new InvalidOperationException($"Ya existe otra plataforma con el nombre '{newName}'.");

            existingPlatform.Name = newName;
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