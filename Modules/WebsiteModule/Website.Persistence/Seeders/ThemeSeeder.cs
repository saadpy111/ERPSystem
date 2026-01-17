using Website.Domain.Entities;
using Website.Domain.ValueObjects;
using Website.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Website.Persistence.Seeders
{
    /// <summary>
    /// Seeds initial themes into the database.
    /// Themes are stored in DB and are editable via admin APIs.
    /// </summary>
    public static class ThemeSeeder
    {
        public static async Task SeedAsync(WebsiteDbContext context)
        {
            // Only seed if no themes exist
            if (await context.Themes.AnyAsync())
                return;

            var themes = GetInitialThemes();
            await context.Themes.AddRangeAsync(themes);
            await context.SaveChangesAsync();
        }

        public static List<Theme> GetInitialThemes()
        {
            return new List<Theme>
            {
                // ===============================
                // Furniture Theme
                // ===============================
                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_furniture_01",
                    Name = "أثاث عصري",
                    PreviewImage = "/id1.png",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#000000",
                            Secondary = "#535929",
                            Background = "white",
                            Text = "#c1ffe4"
                        },
                        Hero = new HeroSection
                        {
                            Title = "أثاث راقي… يصنع الفرق في كل زاوية.",
                            Subtitle = "تصميمات حديثة، ألوان هادئة، وجودة تعيش سنين—حوّل كل غرفة لفرصة جديدة للراحة والجمال.",
                            ButtonText = "تسوق الأن",
                            BackgroundImage = "https://i.ibb.co/Vcsnc5jX/Desktop-13-1.png"
                        },
                        Sections = new List<SectionItem>
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "products", Enabled = true, Order = 1 },
                            new() { Id = "about", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                },

                // ===============================
                // Food Store Theme
                // ===============================
                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_tech_03",
                    Name = "متجر للاغذية",
                    PreviewImage = "/id3.png",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#2e0d76",
                            Secondary = "#001ec0",
                            Background = "white",
                            Text = "#001ec0"
                        },
                        Hero = new HeroSection
                        {
                            Title = "طلباتك كلها هتوصل لباب بيتك … أسرع وأوفر",
                            Subtitle = "أكتر من 5000 منتج متوفرين جاهزين للطلب اختار اللي تحتاجه وهيوصل لحد باب بيتك بسرعة وجودة مضمونة.",
                            ButtonText = "تسوق الأن",
                            BackgroundImage = "https://i.ibb.co/Z6d04N07/image.png"
                        },
                        Sections = new List<SectionItem>
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "categories", Enabled = true, Order = 1 },
                            new() { Id = "offers", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                },

                // ===============================
                // Fashion Theme
                // ===============================
                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_fashion_02",
                    Name = "أزياء وموضة",
                    PreviewImage = "/id2.png",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#6dcaff",
                            Secondary = "#000000",
                            Background = "white",
                            Text = "#6dcaff"
                        },
                        Hero = new HeroSection
                        {
                            Title = "موضة بتكمّل شخصيتك.",
                            Subtitle = "مصممة بعناية لتناسب كل تفاصيل يومك إطلالات مرنة تلائمك في جميع المناسبات.",
                            ButtonText = "تسوق الأن",
                            BackgroundImage = "https://i.ibb.co/LDjWD1Z7/clothes.png"
                        },
                        Sections = new List<SectionItem>
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "new-arrivals", Enabled = true, Order = 1 },
                            new() { Id = "trending", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                },

                // ===============================
                // Car Showroom Theme
                // ===============================
                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_agency_04",
                    Name = "معرض سيارات",
                    PreviewImage = "/id4.png",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#72A1FF",
                            Secondary = "#33025e",
                            Background = "white",
                            Text = "#72A1FF"
                        },
                        Hero = new HeroSection
                        {
                            Title = "مستقبل السيارات… بين يديك.",
                            Subtitle = "استكشف أحدث السيارات الكهربائية والتقنيات الذكية داخل معرض مصمم بعناية ليعرض لك الجيل الجديد من القيادة.",
                            ButtonText = "تسوق الأن",
                            BackgroundImage = "https://i.ibb.co/HpfsfXjF/1.png"
                        },
                        Sections = new List<SectionItem>
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "featured-cars", Enabled = true, Order = 1 },
                            new() { Id = "services", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                },

                // ===============================
                // Electronics Store Theme
                // ===============================
                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_agency_05",
                    Name = "متجر الكترونات",
                    PreviewImage = "/id5.png",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#1e2a60",
                            Secondary = "#3e4ea3",
                            Background = "white",
                            Text = "#3E4EA3"
                        },
                        Hero = new HeroSection
                        {
                            Title = "تكنولوجيا المستقبل… تحت إيدك دلوقتى.",
                            Subtitle = "اختار من أحدث الأجهزة اللي بتتعلم منك مع الوقت، وتطوّر أدائها حسب استخدامك، وتقدم لك تجربة أسرع وأقوى من أي جهاز تقليدي.",
                            ButtonText = "تسوق الأن",
                            BackgroundImage = "https://i.ibb.co/67Jx2n12/1.png"
                        },
                        Sections = new List<SectionItem>
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "products", Enabled = true, Order = 1 },
                            new() { Id = "specs", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                }
            };
        }
    }


}
