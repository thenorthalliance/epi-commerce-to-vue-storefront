using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPiServer.Vsf.DataAccess.Model
{
    public class AspNetUser
    {
        public string Id { get; set; }

        [StringLength(256)]
        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        public string PasswordHash { get; set; }

        public string SecurityStamp { get; set; }

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public DateTime? LockoutEndDateUtc { get; set; }

        public bool LockoutEnabled { get; set; }

        public int AccessFailedCount { get; set; }

        [Required]
        [StringLength(256)]
        public string UserName { get; set; }

        public bool NewsLetter { get; set; }

        public bool IsApproved { get; set; }

        public bool IsLockedOut { get; set; }

        public string Comment { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime CreationDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastLoginDate { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? LastLockoutDate { get; set; }

        public virtual RefreshToken RefreshToken { get; set; }
    }
}
