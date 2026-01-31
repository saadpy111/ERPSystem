using MediatR;
using Website.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Website.Application.Features.Themes.Commands.CreateTheme
{
    public class CreateThemeCommand : IRequest<CreateThemeResponse>
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        
        // Uploaded Images
        public IFormFile? PreviewImageFile { get; set; }
        public IFormFile? HeroBackgroundImageFile { get; set; }
        
        // Flattened Config - Colors
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public string? BackgroundColor { get; set; }
        public string? TextColor { get; set; }
        
        // Flattened Config - Hero Text
        public string? HeroTitle { get; set; }
        public string? HeroSubtitle { get; set; }
        public string? HeroButtonText { get; set; }
        
        // Sections
        public List<SectionItem>? Sections { get; set; }
    }

    public class CreateThemeResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
        public Guid? ThemeId { get; set; }
    }
}
