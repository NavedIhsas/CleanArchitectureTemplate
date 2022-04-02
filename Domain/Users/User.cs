using Domain.Attributes;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Users
{
    [Auditable]   
    public class User:IdentityUser
    {
        public Guid Id { get; set; }
        public string Fullname {  get; set; }
    }
}
