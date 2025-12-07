using System.ComponentModel.DataAnnotations;

namespace Sqordia.Contracts.Requests.BusinessPlan;

public class CreatePublicShareRequest
{
    [Range(0, 2, ErrorMessage = "Permission must be 0 (ReadOnly), 1 (Edit), or 2 (FullAccess)")]
    public int Permission { get; set; } = 0; // 0 = ReadOnly, 1 = Edit, 2 = FullAccess
    
    public DateTime? ExpiresAt { get; set; }
}

