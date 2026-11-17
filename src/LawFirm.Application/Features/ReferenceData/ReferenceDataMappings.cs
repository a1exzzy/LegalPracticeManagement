using System;
using LawFirm.Application.Features.ReferenceData.Dtos;
using LawFirm.Domain.Entities;
namespace LawFirm.Application.Features.ReferenceData;

public static class ReferenceDataMappings
{
    //将实体映射为展示的dto
    public static ReferenceDataDto ToDto(this ReferenceDataItem entity) => new()
    {
        Id = entity.Id,
        Name = entity.Name,
        Code = entity.Code,
        DisplayOrder = entity.DisplayOrder,
        IsActive = entity.IsActive,
        Type = entity.Type
    };
}
