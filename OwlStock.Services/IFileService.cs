﻿using OwlStock.Domain.Entities;

namespace OwlStock.Services
{
    public interface IFileService
    { 
        bool CreatePhotoFile(PhotoBase photo);
    }
}
