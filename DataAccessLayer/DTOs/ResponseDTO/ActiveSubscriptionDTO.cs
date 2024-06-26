﻿using DataAccessLayer.Models;
using Newtonsoft.Json;

namespace DataAccessLayer.DTOs.ResponseDTO;

public class ActiveSubscriptionDTO
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int PackageId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string UserName { get; set; }
    public string PackageName { get; set; }

}