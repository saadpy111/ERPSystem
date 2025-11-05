using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Inventory.Application.Dtos.AttachmentDtos
{
    public static class AttachmentExtensions
    {
        public static AttachmentDto ToDto(this Attachment entity)
        {
            return new AttachmentDto
            {
                Id = entity.Id,
                FileName = entity.FileName,
                FileUrl = entity.FileUrl,
                ContentType = entity.ContentType,
                FileSize = entity.FileSize,
                Description = entity.Description,
                UploadedAt = entity.UploadedAt
            };
        }

        public static List<AttachmentDto> ToDtoList(this List<Attachment> entities)
        {
            return entities?.Select(e => e.ToDto()).ToList() ?? new List<AttachmentDto>();
        }
    }
}
