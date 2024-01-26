namespace TGS.SmsService.ViewModels;

public class SmsViewModel
{
    public required string Destination { get; set; }

    public required string Message { get; set; }

    public required Guid BrandId { get; set; }

    public string? AreaCode { get; set; }

    public string? Phone { get; set; }
}
