using Website.Domain.Entities;
using Website.Domain.Enums;
using Website.Domain.ValueObjects;
using Website.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Website.Persistence.Seeders
{
    public static class ThemeSeeder
    {
        public static async Task SeedAsync(WebsiteDbContext context)
        {
            if (await context.Themes.AnyAsync())
                return;

            var themes = GetInitialThemes();

            await context.Themes.AddRangeAsync(themes);
            await context.SaveChangesAsync();
        }

        public static List<Theme> GetInitialThemes()
        {
            return new()
            {
                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_furniture_01",
                    Name = "أثاث عصري",
                    PreviewImage = "https://erpsystem.runasp.net/themes/tpl_furniture_01/preview/c43426ed-c969-41a0-8802-fdda9ef57866.jpeg",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#000000",
                            Secondary = "#535929",
                            Background = "white",
                            Text = "#c1ffe4",
                            FontFamily = "Neo Sans Arabic"
                        },

                        Hero = new HeroSection
                        {
                            Title = Text("أثاث راقي… يصنع الفرق في كل زاوية."),
                            Subtitle = Text("تصميمات حديثة، ألوان هادئة، وجودة تعيش سنين—حوّل كل غرفة لفرصة جديدة للراحة والجمال."),
                            ButtonText = Text("تسوق الأن"),
                            BackgroundImage = Image("https://i.ibb.co/Vcsnc5jX/Desktop-13-1.png")
                        },

                        ContactUsImages = new ContactUsImages
                        {
                            ContactUsImg = Image("https://example.com/contact.png"),
                            ClientOImg   = Image("https://example.com/client.png")
                        },

                        Sections = new()
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "products", Enabled = true, Order = 1 },
                            new() { Id = "about", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                },

                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_fashion_02",
                    Name = "أزياء وموضة",
                    PreviewImage = "https://erpsystem.runasp.net/themes/tpl_fashion_02/preview/585bc864-6836-4120-ab9c-59ece5e88191.jpeg",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#6dcaff",
                            Secondary = "#000000",
                            Background = "white",
                            Text = "#6dcaff",
                            FontFamily = "Neo Sans Arabic"
                        },

                        Hero = new HeroSection
                        {
                            Title = Text("موضة بتكمّل شخصيتك."),
                            Subtitle = Text("مصممة بعناية لتناسب كل تفاصيل يومك إطلالات مرنة تلائمك في جميع المناسبات."),
                            ButtonText = Text("تسوق الأن"),
                            BackgroundImage = Image("https://i.ibb.co/LDjWD1Z7/clothes.png")
                        },

                        ContactUsImages = new ContactUsImages
                        {
                            ContactUsImg = Image("https://example.com/contact.png"),
                            ClientOImg   = Image("https://example.com/client.png")
                        },

                        Sections = new()
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "new-arrivals", Enabled = true, Order = 1 },
                            new() { Id = "trending", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                },

                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_agency_05",
                    Name = "متجر الكترونات",
                    PreviewImage = "https://erpsystem.runasp.net/themes/tpl_agency_05/preview/92d4b987-94f8-405d-8f8f-61268e4fb111.jpeg",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#1e2a60",
                            Secondary = "#3e4ea3",
                            Background = "white",
                            Text = "#3E4EA3",
                            FontFamily = "Neo Sans Arabic"
                        },

                        Hero = new HeroSection
                        {
                            Title = Text("تكنولوجيا المستقبل… تحت إيدك دلوقتى."),
                            Subtitle = Text("اختار من أحدث الأجهزة اللي بتتعلم منك مع الوقت، وتطوّر أدائها حسب استخدامك، وتقدم لك تجربة أسرع وأقوى من أي جهاز تقليدي."),
                            ButtonText = Text("تسوق الأن"),
                            BackgroundImage = Image("https://i.ibb.co/67Jx2n12/1.png")
                        },

                        ContactUsImages = new ContactUsImages
                        {
                            ContactUsImg = Image("https://example.com/contact.png"),
                            ClientOImg   = Image("https://example.com/client.png")
                        },

                        Sections = new()
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "products", Enabled = true, Order = 1 },
                            new() { Id = "specs", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                },

                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_tech_03",
                    Name = "متجر للاغذية",
                    PreviewImage = "https://erpsystem.runasp.net/themes/tpl_tech_03/preview/035d5a86-a8b8-4593-bc7e-f4eaf2734a05.jpeg",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#2e0d76",
                            Secondary = "#001ec0",
                            Background = "white",
                            Text = "#001ec0",
                            FontFamily = "Neo Sans Arabic"
                        },

                        Hero = new HeroSection
                        {
                            Title = Text("طلباتك كلها هتوصل لباب بيتك … أسرع وأوفر"),
                            Subtitle = Text("أكتر من 5000 منتج متوفرين جاهزين للطلب اختار اللي تحتاجه وهيوصل لحد باب بيتك بسرعة وجودة مضمونة."),
                            ButtonText = Text("تسوق الأن"),
                            BackgroundImage = Image("https://i.ibb.co/Z6d04N07/image.png")
                        },

                        ContactUsImages = new ContactUsImages
                        {
                            ContactUsImg = Image("https://example.com/contact.png"),
                            ClientOImg   = Image("https://example.com/client.png")
                        },

                        Sections = new()
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "categories", Enabled = true, Order = 1 },
                            new() { Id = "offers", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                },

                new Theme
                {
                    Id = Guid.NewGuid(),
                    Code = "tpl_agency_04",
                    Name = "معرض سيارات",
                    PreviewImage = "https://erpsystem.runasp.net/themes/tpl_agency_04/preview/0e8cb5c4-f623-4531-b867-b9fcd9b3a589.jpeg",
                    IsActive = true,
                    Config = new ThemeConfig
                    {
                        Colors = new ThemeColors
                        {
                            Primary = "#72A1FF",
                            Secondary = "#33025e",
                            Background = "white",
                            Text = "#72A1FF",
                            FontFamily = "Neo Sans Arabic"
                        },

                        Hero = new HeroSection
                        {
                            Title = Text("مستقبل السيارات… بين يديك."),
                            Subtitle = Text("استكشف أحدث السيارات الكهربائية والتقنيات الذكية داخل معرض مصمم بعناية ليعرض لك الجيل الجديد من القيادة."),
                            ButtonText = Text("تسوق الأن"),
                            BackgroundImage = Image("https://i.ibb.co/HpfsfXjF/1.png")
                        },

                        ContactUsImages = new ContactUsImages
                        {
                            ContactUsImg = Image("https://example.com/contact.png"),
                            ClientOImg   = Image("https://example.com/client.png")
                        },

                        Sections = new()
                        {
                            new() { Id = "hero", Enabled = true, Order = 0 },
                            new() { Id = "featured-cars", Enabled = true, Order = 1 },
                            new() { Id = "services", Enabled = true, Order = 2 },
                            new() { Id = "footer", Enabled = true, Order = 3 }
                        }
                    }
                }
            };
        }

        private static TextContent Text(string value)
        {
            return new TextContent
            {
                Text = value,
                Style = new TextStyle
                {
                    FontSize = 16,
                    FontWeight = FontWeight.Normal,
                    Color = "#000000",
                    Alignment = TextAlign.Left,
                    HorizontalSpacing = 0,
                    VerticalSpacing = 0
                }
            };
        }

        private static ImageContent Image(string url)
        {
            return new ImageContent
            {
                Url = url,
                Style = new ImageStyle
                {
                    BorderRadius = 6,
                    OverlayColor = "#FFFFFF",
                    OverlayOpacity = 40
                }
            };
        }
    }
}