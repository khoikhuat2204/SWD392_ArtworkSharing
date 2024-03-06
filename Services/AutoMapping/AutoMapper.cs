using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.Models;

namespace Services.AutoMapping;

public class AutoMapper: Profile
{
    public AutoMapper()
    {
        MapArtwork();
        MapAccount();
    }

    private void MapArtwork()
    {
        CreateMap<Artwork, UploadArtworkDTO>().ReverseMap();
    }

    private void MapAccount()
    {
        CreateMap<User, LoginDTO>().ReverseMap();
    }
}