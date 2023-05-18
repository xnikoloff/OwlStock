﻿using Microsoft.AspNetCore.Identity;
using OwlStock.Domain.Entities;
using OwlStock.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OwlStock.Services.DTOs.PhotoShoot
{
    public class PhotoShootByIdDTO
    {
        public int Id { get; set; }
        public string? PersonFullName { get; set; }
        public DateTime ReservationDate { get; set; }
        public DateTime CreatedOn { get; set; }
        public PhotoShootType PhotoShootType { get; set; }
        public string? PhotoShootTypeDescription { get; set; }
        public string? IdentityUserId { get; set; }
        public List<PhotoShootFile>? PhotoShootFiles { get; set; }
    }
}