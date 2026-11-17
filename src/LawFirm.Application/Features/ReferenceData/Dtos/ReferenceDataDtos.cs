using System;
using LawFirm.Domain.Enums;

namespace LawFirm.Application.Features.ReferenceData.Dtos;

// 1. 查出来展示给前端（比如前端拉取“实践领域”下拉菜单）
public class ReferenceDataDto
{
    public int Id { get; set; }
    public ReferenceDataType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}

// 2. 新增时前端传过来的数据
public class CreateReferenceDataCommand
{
    public ReferenceDataType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int DisplayOrder { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

// 3. 修改时前端传过来的数据
public class UpdateReferenceDataCommand
{
    public ReferenceDataType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Code { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; }
}