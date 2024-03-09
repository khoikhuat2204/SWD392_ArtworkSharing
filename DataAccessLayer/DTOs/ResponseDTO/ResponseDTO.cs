namespace DataAccessLayer.DTOs.RequestDTO;

public class ResponseDTO<T> 
{ 
        public int statusCode { get; set; } = 200; 
        public string? message { get; set; } 
        public T? payload { get; set; } 
}

