using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project.Models;

public partial class Account 
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required.")]
    [StringLength(20, MinimumLength = 2)]
    public string? Username { get; set; } = string.Empty;

    [Required]
    [StringLength(30, MinimumLength = 4)]
    public string? Password { get; set; } = string.Empty;
 
    public string? Address { get; set; } = string.Empty;
    public string? Avatar { get; set; } = string.Empty;
 
    [MinLength(1)]
    [MaxLength(10)]
    public string? IdentityNumber { get; set; } = string.Empty;
   
    public bool Gender { get; set; }
	[Required]
    [StringLength(20, MinimumLength = 2)]
    public string Fullname { get; set; } = string.Empty;
	public bool IsActive { get; set; }
	public DateTime CreateDate { get; set; } = DateTime.Now;
	public DateTime Dob { get; set; }
	[NotMapped]
	public IFormFile? ImageFile { get; set; } 
	[Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    [MinLength(1)]
    [MaxLength(20)]
    public string? PhoneNumber { get; set; } = string.Empty;

    public int SkyMiles { get; set; } = 0;
    public int RoleId {  get; set; }
    public Role? Role { get; set; }

	public ICollection<Cart> Carts = new List<Cart>();

    public ICollection<Booking> Bookings = new List<Booking>();


}
