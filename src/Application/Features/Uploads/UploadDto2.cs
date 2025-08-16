using Abp.Application.Services.Dto;
using CouponPaymentSystem.Domain.Enums;

namespace CouponPaymentSystem.Application.Features.Uploads;

public class UploadDto2 : EntityDto
{
    public string FileName { get; set; }
    public Currency Currency { get; set; }
}
