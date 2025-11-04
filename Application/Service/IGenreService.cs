using Contracts.Genre.Request;
using Contracts.Genre.Response;

namespace Application.Service
{
    public interface IGenreService
    {
        List<GenreResponse> GetGenres();
        GenreResponse CreateGenre(CreateGenreRequest request);
        GenreResponse UpdateGenre(UpdateGenreRequest request);
        bool DeleteGenre(int id);
    }
}
