using AutoMapper;
using DataAccessLayer.DTOs.RequestDTO;
using DataAccessLayer.DTOs.ResponseDTO;
using DataAccessLayer.Models;

namespace Services.AutoMapping;

public class AutoMapper: Profile
{
    public AutoMapper()
    {
        MapArtwork();
        MapAccount();
        MapReservation();
        MapReport();
    }
    
    private void MapReport()
    {
        CreateMap<Report, ReportResponseDTO>().ReverseMap();
        CreateMap<Report, ReportRequestDTO>().ReverseMap();
    }

    private void MapReservation()
    {
        CreateMap<Reservation, ReservationResponseDTO>().ReverseMap();
        CreateMap<Reservation, ReservationRequestDTO>().ReverseMap();
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