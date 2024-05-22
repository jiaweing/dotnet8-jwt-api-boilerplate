using Api.Application.Models.Base;

namespace Api.Application.Models;

public partial class User : BaseModel
{
    public string UserId { get; set; } = null!;

	public string Name { get; set; } = null!;

    public string EmailAddress { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<UserRole> UserRoles { get; set; }
}
