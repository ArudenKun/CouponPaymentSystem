using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using CouponPaymentSystem.Core.UploadTransactions;

namespace CouponPaymentSystem.Application.UploadTransactions.Dto;

[AutoMapFrom(typeof(UploadTransaction))]
public class UploadTransactionDto : EntityDto { }
