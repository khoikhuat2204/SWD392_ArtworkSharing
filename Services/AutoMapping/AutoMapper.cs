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
        CreateMap<UploadArtworkDTO, Artwork>().ReverseMap();
        CreateMap<Artwork, ArtworkDTO>().ReverseMap();
        CreateMap<ArtworkDTO, Artwork>().ReverseMap();
        CreateMap<Artwork, UpdateArtworkDTO>().ReverseMap();
        CreateMap<UpdateArtworkDTO, Artwork>().ReverseMap();
    }

    private void MapAccount()
    {
        CreateMap<User, LoginDTO>().ReverseMap();
    }
}