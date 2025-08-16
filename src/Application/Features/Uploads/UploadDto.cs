using Abp.Application.Services.Dto;
using CouponPaymentSystem.Domain.Entities;
using Facet;

namespace CouponPaymentSystem.Application.Features.Uploads;

[Facet(typeof(Upload), exclude: [nameof(Upload.JobId)])]
public partial class UploadDto : EntityDto { }
