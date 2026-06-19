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

        await context.Themes.AddRangeAsync(GetThemes());
            await context.SaveChangesAsync();
        }

        private static List<Theme> GetThemes()
        {
            return new()
        {
            Pharmacy(),
            Furniture(),
            Food(),
            Electronics(),
            Cars(),
            Fashion()
        };
        }

        // ===================== THEMES =====================

        private static Theme Pharmacy() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_pharmacy_01",
            Name = "أدوية",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_pharmacy_01/preview/bebb7172-8075-43c8-9f9f-0364b7811ff9.png",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#2BB673", "#00AEEF", "#F7FFFB", "#2E2E2E", "Cairo"),
                Hero = new HeroSection
                {
                    Title = Txt("صحتك أولويتنا… دوائك لحد باب بيتك", 54, FontWeight.Bold, "#FFFFFF"),
                    Subtitle = Txt("اطلب أدويتك بسهولة وأمان...", 26, FontWeight.Normal, "#f1f3f5"),
                    ButtonText = Txt("اطلب الآن", 20, FontWeight.Bold, "#56A3CF"),
                    BackgroundImage = Img("https://erpsystem.runasp.net/themes/tpl_pharmacy_01/hero/883d8aac-898e-49f9-becf-291ef5ecbbf9.png", 0)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_pharmacy_01/contactus/c447f238-197e-42a1-b36c-cf07a2977926.png", 6, 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_pharmacy_01/clientoverview/32d4383b-a0d1-4594-8719-29495c976388.png", 6, 40)
                },
                Sections = Sections()
            }
        };

        private static Theme Furniture() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_furniture_01",
            Name = "اثاث",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_furniture_01/preview/c43426ed-c969-41a0-8802-fdda9ef57866.jpeg",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#000", "#535929", "#fbfbfb", "#6b6b6b", "El Messiri"),
                Hero = new HeroSection
                {
                    Title = Txt("أثاث راقي…", 56, FontWeight.Bold, "#FFF"),
                    Subtitle = Txt("تصميمات حديثة...", 28, FontWeight.Normal, "#C1FFE4"),
                    ButtonText = Txt("تسوق الآن", 20, FontWeight.Bold, "#000"),
                    BackgroundImage = Img("https://i.ibb.co/Vcsnc5jX/Desktop-13-1.png", 0)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_furniture_01/contactus/70bea904-9558-4c75-bfdd-3741c663b6e4.png", 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_furniture_01/clientoverview/8cde7579-730b-4f11-84e1-d03cd8fc7431.png", 0)
                },
                Sections = Sections()
            }
        };

        private static Theme Food() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_tech_03",
            Name = "اغذية",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_tech_03/preview/26305696-ee9c-4334-8ca4-95dd3afca0ed.png",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#2E0D76", "#7F59D2", "#ffffff", "#6b6b6b", "Neo Sans Arabic"),
                Hero = new HeroSection
                {
                    Title = Txt("طلباتك كلها هتوصل...", 48, FontWeight.Bold, "#2E0D76"),
                    Subtitle = Txt("أكتر من 5000 منتج...", 28, FontWeight.Normal, "#001EC0"),
                    ButtonText = Txt("تسوق الآن", 20, FontWeight.Bold, "#2E0D76"),
                    BackgroundImage = Img("https://i.ibb.co/Z6d04N07/image.png", 6, 40)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_tech_03/contactus/fb265cbb-75bd-4f60-8d8d-fb214567659a.png", 6, 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_tech_03/clientoverview/5ff24b0c-dff8-4eaf-8a63-944b037583ce.jpg", 6, 40)
                },
                Sections = Sections()
            }
        };

        private static Theme Electronics() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_agency_05",
            Name = "الكترونيات",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_agency_05/preview/790b376c-a28e-4b8d-81d8-031ff3c1da4c.png",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#1E2A60", "#3E4EA3", "#ffffff", "#6b6b6b", "Droid Arabic Kufi"),
                Hero = new HeroSection
                {
                    Title = Txt("تكنولوجيا المستقبل…", 56, FontWeight.Bold, "#1E2A60"),
                    Subtitle = Txt("اختار من أحدث الأجهزة...", 28, FontWeight.Normal, "#3E4EA3"),
                    ButtonText = Txt("تسوق الآن", 20, FontWeight.Bold, "#1E2A60"),
                    BackgroundImage = Img("https://i.ibb.co/67Jx2n12/1.png", 6, 40)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_agency_05/contactus/e1d340c9-58c5-4e3f-9a54-b6bc5074a197.png", 6, 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_agency_05/clientoverview/b37f711f-6e8a-4ba1-b4e6-407dcc7100f0.png", 6, 40)
                },
                Sections = Sections()
            }
        };

        private static Theme Cars() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_agency_04",
            Name = "سيارات",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_agency_04/preview/c1c97947-37de-4884-b636-2ba5fd0a9f79.png",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#0c0929", "#33025e", "#ffffff", "#3e4ea3", "Neo Sans Arabic"),
                Hero = new HeroSection
                {
                    Title = Txt("مستقبل السيارات…", 60, FontWeight.Medium, "#FFF"),
                    Subtitle = Txt("استكشف أحدث السيارات...", 28, FontWeight.Normal, "#72A1FF"),
                    ButtonText = Txt("تسوق الآن", 24, FontWeight.Bold, "#480181"),
                    BackgroundImage = Img("https://erpsystem.runasp.net/themes/tpl_agency_04/hero/40a43253-1cbc-4b3b-8ec8-04fcf891fc71.png", 0)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_agency_04/contactus/240a60a2-b109-447b-bea9-0b39e7d534fd.png", 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_agency_04/clientoverview/07dc1871-68f1-4720-9b68-9c7154f8aefa.png", 0)
                },
                Sections = Sections()
            }
        };

        private static Theme Fashion() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_fashion_02",
            Name = "ملابس",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_fashion_02/preview/aaf4df6a-f271-44a7-9d1e-a16eeeff528f.png",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#000", "#56A3CF", "#ffffff", "#6b6b6b", "Kufi"),
                Hero = new HeroSection
                {
                    Title = Txt("موضة بتكمّل شخصيتك", 60, FontWeight.Bold, "#6DCAFF"),
                    Subtitle = Txt("مصممة بعناية...", 32, FontWeight.Normal, "#FFF"),
                    ButtonText = Txt("تسوق الآن", 20, FontWeight.Bold, "#000"),
                    BackgroundImage = Img("https://i.ibb.co/LDjWD1Z7/clothes.png", 6, 40)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_fashion_02/contactus/c9b368da-63b3-4832-9b05-1639526da783.png", 6, 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_fashion_02/clientoverview/7172b183-f5e9-4acd-820a-3ade7342a8a8.png", 6, 40)
                },
                Sections = Sections()
            }
        };

        // ===================== HELPERS =====================

        private static ThemeColors Colors(string p, string s, string bg, string t, string f) =>
            new() { Primary = p, Secondary = s, Background = bg, Text = t, FontFamily = f };

        private static List<SectionItem> Sections() => new()
    {
        Sec("hero",0), Sec("footer",1), Sec("featured_categories",2),
        Sec("best_selling",3), Sec("collections",4), Sec("offers",5),
        Sec("newsletter",6), Sec("testimonials",7), Sec("new_arrivals",8),
        Sec("brands",9), Sec("why_choose_us",10), Sec("contact_us",11)
    };

        private static SectionItem Sec(string id, int o) => new()
        {
            Id = id,
            Enabled = true,
            Order = o,
            Title = Txt("", 16, FontWeight.Normal, "#000"),
            Subtitle = Txt("", 16, FontWeight.Normal, "#000"),
            ButtonText = Txt("", 16, FontWeight.Normal, "#000"),
            BackgroundImage = Img("", 0)
        };

        private static TextContent Txt(string t, int s, FontWeight w, string c) => new()
        {
            Text = t,
            Style = new TextStyle
            {
                FontSize = s,
                FontWeight = w,
                Color = c,
                Alignment = TextAlign.Left,
                HorizontalSpacing = 0,
                VerticalSpacing = 0,
                MarginTop = 0,
                BackgroundColor = null
            }
        };

        private static ImageContent Img(string u, int r, int op = 0) => new()
        {
            Url = u,
            Style = new ImageStyle
            {
                BorderRadius = r,
                OverlayColor = "#FFFFFF",
                OverlayOpacity = op
            }
        };
    }

}
