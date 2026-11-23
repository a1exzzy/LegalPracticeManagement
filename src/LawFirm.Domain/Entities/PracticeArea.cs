using System;

namespace LawFirm.Domain.Entities;

public class PracticeArea
{
    public int Id { get; private set; } 
    public string Name { get; set; } = string.Empty;
}
