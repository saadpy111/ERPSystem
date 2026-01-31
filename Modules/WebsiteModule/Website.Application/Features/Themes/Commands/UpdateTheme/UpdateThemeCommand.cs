using MediatR;
using Website.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Website.Application.Features.Themes.Commands.UpdateTheme
{
    public class UpdateThemeCommand : IRequest<UpdateThemeResponse>
    {
        public Guid ThemeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        
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

    public class UpdateThemeResponse
    {
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}
