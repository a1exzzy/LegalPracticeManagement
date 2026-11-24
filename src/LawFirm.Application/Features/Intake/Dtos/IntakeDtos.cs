using System;
using LawFirm.Domain.Entities;
using LawFirm.Domain.Enums;

namespace LawFirm.Application.Features.Intake;
public record IntakeListDto(
    int Id,
    string IntakeNumber,
    string ProspectiveClientName,
    int PracticeAreaId,
    string PracticeAreaName,
    int AssignedReviewerId,
    string? AssignedReviewerName,
    IntakeStatus Status,
    string? Urgency,
    DateTime ConsultationDate
);

public record IntakeDetailDto(
    int Id,
    string IntakeNumber,
    string ProspectiveClientName,
    ClientType ClientTypeGuess,
    string? PrimaryEmail,
    string? PrimatyPhone,
    int PracticeAreaId,
    string PracticeAreaName,
    string? Summary,
    int AssignedReviewerId,
    string? AssignedReviewerName,
    string? SourceOfEnquiry,
    string? OpposingParty,
    IntakeStatus Status,
    string? Urgency,
    DateTime ConsultationDate
);

public record CreateIntakeRequest(
    string ProspectiveClientName,
    ClientType ClientTypeGuess,
    string? PrimaryEmail,
    string? PrimatyPhone,
    int PracticeAreaId,
    string PracticeAreaName,
    string? Summary,
    int AssignedReviewerId,
    string? AssignedReviewerName,
    string? SourceOfEnquiry,
    string? OpposingParty,
    IntakeStatus Status,
    string? Urgency,
    DateTime ConsultationDate
);

public record UpdateIntakeRequest(
    IntakeStatus Status,
    int AssignedReviewerId,
    string? AssignedReviewerName,
    int PracticeAreaId,
    string PracticeAreaName,
    string? Summary,
    string? Urgency,
    DateTime ConsultationDate,
    string Notes    
);

public record ConvertIntakeRequest(
    bool CreateNewClient,
    int ExistingClientId,
    string? MatterTitle,
    int MatterTypeId,
    int ResponsibleLawyerId,
    //MatterStatus MatterStatus,
    string? MatterSummary 
);
