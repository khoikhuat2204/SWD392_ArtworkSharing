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
        MapPackage();
        MapActiveSubscription();
        MapRegister();
        MapArtworkType();
        MapRating();
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
        CreateMap<Artwork, ArtworkDTO>().ReverseMap();
        CreateMap<Artwork, UpdateArtworkDTO>().ReverseMap();
        CreateMap<Artwork, ArtworkDetailDTO>().ReverseMap();
    }

    private void MapAccount()
    {
        CreateMap<User, LoginDTO>().ReverseMap();
    }

    private void MapPackage()
    {
        CreateMap<Package, PackageDTO>().ReverseMap();
    }
    
    private void MapActiveSubscription()
    {
        CreateMap<ActiveSubscription, ActiveSubscriptionDTO>().ReverseMap();
        CreateMap<ActiveSubscription, CreateSubscriptionDTO>().ReverseMap();
    }

    private void MapRegister()
    {
        CreateMap<User, RegisterDTO>().ReverseMap();
    }

    private void MapArtworkType()
    {
        CreateMap<ArtworkType, ArtworkTypeDTO>().ReverseMap();
    }
    
    private void MapRating()
    {
        CreateMap<Rating, CreateRatingDTO>().ReverseMap();
        CreateMap<Rating, RatingResponseDTO>().ReverseMap();
    }
}