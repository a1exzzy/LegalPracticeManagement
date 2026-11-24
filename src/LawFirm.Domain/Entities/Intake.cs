using System;
using LawFirm.Domain.Enums;

namespace LawFirm.Domain.Entities;

public class Intake
{
    public int Id { get; private set; }
    public string IntakeNumber { get; set; }  = string.Empty;
    public string ProspectiveClientName { get; set; } = string.Empty; //required
    public int PracticeAreaId { get; set; }//required
    public IntakeStatus Status = IntakeStatus.New;//required, default New

    public ClientType ClientTypeGuess { get; set; } //optional
    public string? PrimaryEmail { get; set; }
    public string? PrimatyPhone { get; set; }
    public string? Urgency { get; set; } //???optional but recommended. not sure if it's a string or enum
    public int AssignedReviewerId { get; set; } //optional at create, but recommended
    public string? SourceOfEnquiry { get; set; } //phone / email / referral / walk-in / website etc.
    public string? OpposingParty { get; set; } //optional
    public DateTime ConsultationDate { get; set; }
    public string? Summary { get; set; } = string.Empty;
    public string? Notes { get; set; } = string.Empty;

}
