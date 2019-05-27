using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EPiServer.Vsf.DataAccess.Model
{
    public class RefreshToken
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }

        public string Value { get; set; }

        public virtual AspNetUser User { get; set; }
    }
}