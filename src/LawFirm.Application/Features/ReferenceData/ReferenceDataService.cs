using System;
using LawFirm.Infrastructure.Persistence;

namespace LawFirm.Application.Features.ReferenceData;

public class ReferenceDataService : IReferenceDataService
{// 直接用 DbContext 查询，按显示顺序排序，只查激活的
    private LawFirmDbContext _dbContext;
    public ReferenceDataService(LawFirmDbContext lawFirmDbContext)
    {
        _dbContext = lawFirmDbContext;
    }

    public 


}
