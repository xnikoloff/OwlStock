﻿using Microsoft.AspNetCore.Identity;
using OwlStock.Domain.Enumerations;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OwlStock.Domain.Entities
{
    public class PhotoShoot
    {
        public PhotoShoot()
        {
            this.PhotoShootFiles = new HashSet<PhotoShootFile>();
        }

        [Key]
        public int Id { get; set; }
        public string? PersonFirstName { get; set; }
        public string? PersonLastName { get; set; }
        public string? PersonFullName { get; set; }
        public string? PersonEmail { get; set; }
        public string? PersonPhone { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public PhotoShootType PhotoShootType { get; set; }
        public string? PhotoShootTypeDescription { get; set; }

        [ForeignKey(nameof(IdentityUser))]
        public string? IdentityUserId { get; set; }
        public IdentityUser? IdentityUser { get; set; }

        public ICollection<PhotoShootFile> PhotoShootFiles { get; set; }
    }
}
