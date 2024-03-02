using AutoMapper;
using DataAccessLayer.Models;
using Services.RequestDTO;

namespace Services.AutoMapping;

public class AutoMapper: Profile
{
    public AutoMapper()
    {
        MapArtwork();
    }

    private void MapArtwork()
    {
        CreateMap<Artwork, UploadArtworkDTO>().ReverseMap();
    }
}