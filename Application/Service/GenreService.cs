using Application.Abstraction;
using Contracts.Genre.Request;
using Contracts.Genre.Response;
using Domain.Entities;

namespace Application.Service
{
    public class GenreService : IGenreService
    {
        private readonly IGenreRepository _genreRepo;

        public GenreService(IGenreRepository genreRepo)
        {
            _genreRepo = genreRepo;
        }
        public GenreResponse CreateGenre(CreateGenreRequest request)
        {
            
            var genre = new Genre
            {
                Name = request.Name
            };
            bool success = _genreRepo.Create(genre);

            if (!success)
            {
                throw new ApplicationException("Error al crear el género en la base de datos.");
            }
            var response = new GenreResponse
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return response;
        }

        public List<GenreResponse> GetGenres()
        {
            var genres = _genreRepo.GetAll();

            var responseList = genres.Select(g => new GenreResponse
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();

            return responseList;
        }
        public GenreResponse UpdateGenre(UpdateGenreRequest request)
        {
            var existingGenre = _genreRepo.GetById(request.Id, trackChanges: true);
            if (existingGenre == null)
            {
                throw new Exception($"Género con ID {request.Id} no encontrado.");
            }
            existingGenre.Name = request.NewName;
            bool success = _genreRepo.Update(existingGenre);
            if (!success)
            {
                throw new ApplicationException("Error al actualizar el género en la base de datos.");
            }
            return new GenreResponse
            {
                Id = existingGenre.Id,
                Name = existingGenre.Name
            };
            }
            public bool DeleteGenre(int id)
        { 
            var genreToDelete = _genreRepo.GetById(id);

            if (genreToDelete == null)
            {
                return true;
            }

            return _genreRepo.Delete(genreToDelete);
        }
    }
    }

