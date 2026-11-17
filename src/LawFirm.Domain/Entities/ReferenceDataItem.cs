using System;
using System.ComponentModel.DataAnnotations;
using LawFirm.Domain.Enums;

namespace LawFirm.Domain.Entities;

public class ReferenceDataItem
{
    public int Id { get; private set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; } //Allow nullable
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
    public ReferenceDataType Type { get; set; }


    public static ReferenceDataItem Create(string name, string code, int displayOrder, bool isActive, ReferenceDataType type)
    {
        var referenceData = new ReferenceDataItem
        {
            Name = name.Trim(),
            Code = code.Trim(),
            DisplayOrder = displayOrder,
            IsActive = isActive,
            Type = type

        };
        return referenceData;
    }
}
