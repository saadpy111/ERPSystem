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
                Food(),
                Furniture(),
                Cars(),
                Electronics(),
                Fashion()
            };
        }

        // ===================== THEMES =====================

        private static Theme Pharmacy() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_pharmacy_01",
            Name = "الأدوية",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_pharmacy_01/preview/bebb7172-8075-43c8-9f9f-0364b7811ff9.png",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#016680", "#007c75", "#ffffff", "#000000", "Arial"),
                Hero = new HeroSection
                {
                    Title = Txt("صحتك أولويتنا… دواؤك في متناول يدك.", 78, 700, "#ffffff", 2, verticalSpacing: 164),
                    Subtitle = Txt("نوفر لك أفضل الأدوية والمنتجات الطبية بأعلى جودة وأسعار مناسبة.", 28, 400, "#00ff23", 2),
                    ButtonText = Txt("تسوق الآن", 20, 700, "#000", 2),
                    BackgroundImage = Img("https://erpsystem.runasp.net/themes/tpl_pharmacy_01/hero/883d8aac-898e-49f9-becf-291ef5ecbbf9.png", 12, "#000000", 40)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_pharmacy_01/contactus/c447f238-197e-42a1-b36c-cf07a2977926.png", 6, "#FFFFFF", 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_pharmacy_01/clientoverview/32d4383b-a0d1-4594-8719-29495c976388.png", 6, "#FFFFFF", 40)
                },
                Sections = new List<SectionItem>
                {
                    Sec("featured_categories", 2,
                        Txt("أقسام طبية", 32, 700, "#016680", 1),
                        Txt("كل احتياجاتك الصحية في مكان واحد", 21, 500, "#007c75", 1),
                        Txt("", 14, 600, "#000000", 2),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("best_selling", 3,
                        Txt("الأكثر طلباً", 32, 700, "#016680", 2),
                        Txt("", 16, 400, "#000000", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("collections", 4,
                        Txt(" اكتشف مجموعاتنا الطبية المتميزه", 32, 500, "#016680", 2),
                        Txt("عناية متكاملة", 16, 400, "#007c75", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("newsletter", 5,
                        Txt("اشترك الآن واحصل على 30% خصم لأول طلب", 32, 700, "#016680", 1),
                        Txt("عروض خاصة وأدوية جديدة أولاً بأول", 16, 400, "#007c75", 1),
                        Txt("اشترك الان", 14, 600, "#000000", 2),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("offers", 6,
                        Txt("استمتع بخصومات تصل الى", 34, 700, "#016680", 2),
                        Txt("أفضل الأسعار على الأدوية", 21, 500, "#007c75", 2),
                        Txt("عروض مخصصه على اهم الادوية", 14, 600, "#000000", 2),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("testimonials", 7,
                        Txt("آراء العملاء", 32, 500, "#016680", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("contact_us", 8,
                        Txt("تواصل معنا", 32, 700, "#016680", 2),
                        Txt("فريقنا جاهز لمساعدتك", 16, 400, "#007c75", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("new_arrivals", 9,
                        Txt("أحدث الأدوية", 32, 700, "#016680", 2),
                        Txt("منتجات جديدة باستمرار", 16, 400, "#007c75", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("why_choose_us", 10,
                        Txt("لماذا نحن؟", 35, 700, "#016680", 1),
                        Txt("جودة عالية وخدمة سريعة", 21, 500, "#007c75", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("brands", 11,
                        Txt("شركات الأدوية", 32, 700, "#016680", 1),
                        Txt("نتعاون مع علامات تجارية موثوقة والشركات العالميه", 21, 500, "#007c75", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    // Second "contact_us" section (duplicate id, present exactly as in source data)
                    Sec("contact_us", 10,
                        Txt("محتاج مساعدة؟", 32, 500, "#016680", 2),
                        Txt("فريق الدعم متاح دائماً لمساعدتك", 21, 500, "#007c75", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),
                }
            }
        };

        private static Theme Food() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_tech_03",
            Name = "الأغذية",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_tech_03/preview/26305696-ee9c-4334-8ca4-95dd3afca0ed.png",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#2E0D76", "#7F59D2", "#ffffff", "#6b6b6b", "Neo Sans Arabic"),
                Hero = new HeroSection
                {
                    Title = Txt("طلباتك كلها هتوصل لباب بيتك … أسرع وأوفر", 58, 700, "#2E0D76", 2, verticalSpacing: 100),
                    Subtitle = Txt("أكتر من 5000 منتج متوفرين جاهزين للطلب  اختار اللي تحتاجه وهيوصل لحد باب بيتك بسرعة وجودة مضمونة", 28, 400, "#001EC0", 1),
                    ButtonText = Txt("تسوق الآن", 20, 700, "#000", 2),
                    BackgroundImage = Img("https://i.ibb.co/Z6d04N07/image.png", 12, "#FFFFFF", 40)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_tech_03/contactus/fb265cbb-75bd-4f60-8d8d-fb214567659a.png", 6, "#FFFFFF", 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_tech_03/clientoverview/5ff24b0c-dff8-4eaf-8a63-944b037583ce.jpg", 6, "#FFFFFF", 40)
                },
                Sections = new List<SectionItem>
                {
                    Sec("featured_categories", 2,
                        Txt("مجموعات مختارة بعناية", 32, 700, "#2E0D76", 1),
                        Txt("استمتع بأشهى المأكولات والمشروبات الطازجة يومياً", 16, 400, "#000000", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("best_selling", 3,
                        Txt("مختارة لك", 32, 700, "#2E0D76", 2),
                        Txt("منتجات تناسب اختياراتك اليومية وتكمّل احتياجات بيتك بكل سهولة", 21, 500, "#7F59D2", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("collections", 4,
                        Txt("عروض موسمية مميزة", 32, 500, "#2E0D76", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("newsletter", 5,
                        Txt("اشترك الآن واحصل على 10% خصم لأول طلب", 32, 700, "#2E0D76", 1),
                        Txt("اشترك دلوقتي علشان توصلك أحدث العروض، الخصومات أول بأول ومن غير ما يفوتك أي توفير.", 21, 400, "#7F59D2", 1),
                        Txt("اشترك الآن", 16, 400, "#ffffff", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("offers", 6,
                        Txt("استمتع بخصومات تصل الى", 32, 700, "#0C0929", 2),
                        Txt("جدد اسلوب حياتك مع تشكيلتنا المختارة من افضل المنتجات العالمية بأسعار تنافسية", 21, 500, "#33025E", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("testimonials", 7,
                        Txt("آراء عملائنا… انعكاس لجودة منتجاتنا", 32, 500, "#2E0D76", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("contact_us", 8,
                        Txt("لنبدأ شيئاً رائعاً معاً", 32, 700, "#2E0D76", 2),
                        Txt("لديك فكرة؟ فريقنا متاح دائماً لمساعدتك في تحويل رؤيتك إلى واقع.", 16, 400, "#000000", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("new_arrivals", 9,
                        Txt("منتجات وصلت حديثاً", 32, 700, "#2E0D76", 2),
                        Txt("منتجات مميزة بأسعار تنافسية .", 21, 400, "#33025E", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("why_choose_us", 10,
                        Txt("تجربة تسوق أسهل وأسرع", 32, 700, "#2E0D76", 1),
                        Txt("نوفّر لك كل احتياجات بيتك بجودة عالية، أسعار مناسبة، وخدمة توصيل تريحك من المشوار.", 21, 500, "#7F59D2", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("brands", 11,
                        Txt("شركاء النجاح", 32, 700, "#2E0D76", 1),
                        Txt("نتعاون مع علامات تجارية موثوقة لتقديم أفضل تجربة لعملائنا.", 21, 500, "#7F59D2", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),
                }
            }
        };

        private static Theme Furniture() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_furniture_01",
            Name = "الاثاث",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_furniture_01/preview/c43426ed-c969-41a0-8802-fdda9ef57866.jpeg",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#000", "#535929", "#fbfbfb", "#6b6b6b", "El Messiri"),
                Hero = new HeroSection
                {
                    Title = Txt("أثاث راقي… يصنع الفرق في كل زاوية.", 56, 700, "#FFF", 2, verticalSpacing: 164),
                    Subtitle = Txt("تصميمات حديثة، ألوان هادئة، وجودة تعيش سنين—حوّل كل غرفة لفرصة جديدة للراحة والجمال.", 21, 400, "#C1FFE4", 2),
                    ButtonText = Txt("تسوق الآن", 20, 700, "#000", 2),
                    BackgroundImage = Img("https://i.ibb.co/Vcsnc5jX/Desktop-13-1.png", 12, "#FFFFFF", 0)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_furniture_01/contactus/70bea904-9558-4c75-bfdd-3741c663b6e4.png", 0, "#FFFFFF", 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_furniture_01/clientoverview/8cde7579-730b-4f11-84e1-d03cd8fc7431.png", 0, "#FFFFFF", 0)
                },
                Sections = new List<SectionItem>
                {
                    Sec("featured_categories", 2,
                        Txt("مجموعات مختارة بعناية", 32, 700, "#000", 1),
                        Txt("اخترنا لك أفضل القطع بعناية لتسهّل عليك رحلة تنسيق بيتك", 21, 500, "#535929", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("best_selling", 3,
                        Txt("الأكثر مبيعًا هذا الشهر", 32, 700, "#000", 2),
                        Txt("منتجات أثبتت نفسها في بيوت حقيقية، وحققت أعلى تقييمات من عملائنا", 21, 500, "#535929", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("collections", 4,
                        Txt("حوّل بيتك بإطلالة واحدة", 32, 500, "#535929", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("newsletter", 5,
                        Txt("اشترك الآن واحصل على 10% خصم لأول طلب", 32, 700, "#000", 1),
                        Txt("عروض خاصة، منتجات جديدة، ونصائح ديكور توصلك قبل أي حد", 16, 400, "#535929", 1),
                        Txt("اشترك الآن", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("offers", 6,
                        Txt("استمتع بخصومات تصل الى", 34, 700, "#000", 2),
                        Txt("جدد اسلوب حياتك مع تشكيلتنا المختارة من افضل المنتجات العالمية بأسعار تنافسية", 21, 500, "#535929", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("testimonials", 7,
                        Txt("آراء عملائنا… انعكاس لجودة منتجاتنا", 32, 500, "#535929", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("contact_us", 8,
                        Txt("لنبدأ شيئاً رائعاً معاً", 32, 700, "#000", 2),
                        Txt("لديك فكرة؟ فريقنا متاح دائماً لمساعدتك في تحويل رؤيتك إلى واقع.", 16, 400, "#000000", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("new_arrivals", 9,
                        Txt("الأجمل هذا الموسم… بأفضل سعر !", 32, 700, "#000", 2),
                        Txt("اختيارات مميزة تناسب احتياجات الموسم بأفضل الأسعار.", 21, 400, "#000000", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("why_choose_us", 10,
                        Txt("قصة الجودة من البداية", 32, 700, "#000", 1),
                        Txt("نحرص على تقديم تجربة شراء متكاملة تجمع بين الجودة، السعر المناسب، وسهولة التعامل.", 21, 500, "#535929", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("brands", 11,
                        Txt("شركاء النجاح", 32, 700, "#000", 1),
                        Txt("نتعاون مع علامات تجارية موثوقة لتقديم أفضل تجربة لعملائنا.", 21, 500, "#535929", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),
                }
            }
        };

        private static Theme Cars() => new Theme
        {
            Id = Guid.NewGuid(),
            Code = "tpl_agency_04",
            Name = "السيارات",
            PreviewImage = "https://erpsystem.runasp.net/themes/tpl_agency_04/preview/c1c97947-37de-4884-b636-2ba5fd0a9f79.png",
            IsActive = true,
            Config = new ThemeConfig
            {
                Colors = Colors("#0c0929", "#33025e", "#ffffff", "#3e4ea3", "Neo Sans Arabic"),
                Hero = new HeroSection
                {
                    Title = Txt("مستقبل السيارات… بين يديك", 50, 700, "#FFF", 1, verticalSpacing: 100),
                    Subtitle = Txt("استكشف أحدث السيارات الكهربائية والتقنيات الذكية داخل معرض مصمم بعناية ليعرض لك الجيل الجديد من القيادة", 20, 400, "#72A1FF", 1),
                    ButtonText = Txt("تسوق الآن", 20, 700, "#000", 2),
                    BackgroundImage = Img("https://erpsystem.runasp.net/themes/tpl_agency_04/hero/40a43253-1cbc-4b3b-8ec8-04fcf891fc71.png", 12, "#FFFFFF", 0)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_agency_04/contactus/240a60a2-b109-447b-bea9-0b39e7d534fd.png", 0, "#FFFFFF", 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_agency_04/clientoverview/07dc1871-68f1-4720-9b68-9c7154f8aefa.png", 0, "#FFFFFF", 0)
                },
                Sections = new List<SectionItem>
                {
                    Sec("featured_categories", 2,
                        Txt("اختَر سيارتك بسهولة", 32, 700, "#0C0929", 1),
                        Txt("تصفح مجموعتنا حسب نوع السيارة واحتياجك اليومي أو العملي", 21, 500, "#33025E", 1),
                        Txt("عرض الكل", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("best_selling", 3,
                        Txt("الأكثر مبيعًا هذا الشهر", 32, 700, "#0C0929", 2),
                        Txt("سيارات أثبتت نفسها ، وحققت أعلى تقييمات من عملائنا", 21, 500, "#33025E", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("collections", 4,
                        Txt("مجموعات سيارات مميزة", 32, 500, "#0C0929", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("عرض الكل", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("newsletter", 5,
                        Txt("اشترك الآن واحصل على 10% خصم لأول طلب", 32, 700, "#0C0929", 1),
                        Txt("كن أول من يعرف عن أحدث السيارات والعروض الحصرية قبل أي حد", 21, 400, "#33025E", 1),
                        Txt("اشترك الآن", 16, 400, "#ffffff", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("offers", 6,
                        Txt("استمتع بخصومات تصل الى", 32, 700, "#0C0929", 2),
                        Txt("جدد اسلوب حياتك مع تشكيلتنا المختارة من افضل السيارات العالمية بأسعار تنافسية", 21, 500, "#33025E", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("testimonials", 7,
                        Txt("آراء عملائنا… انعكاس لجودة منتجاتنا", 32, 500, "#0C0929", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("contact_us", 8,
                        Txt("لنبدأ شيئاً رائعاً معاً", 32, 700, "#0C0929", 2),
                        Txt("لديك فكرة؟ فريقنا متاح دائماً لمساعدتك في تحويل رؤيتك إلى واقع.", 16, 400, "#000000", 2),
                        Txt("فريقنا متاح دائما لخدمتك", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("new_arrivals", 9,
                        Txt("سيارات مختارة بعناية لتناسب احتياجك", 32, 700, "#0C0929", 2),
                        Txt("اختيارات مميزة بأسعار تنافسية جاهزة للتسليم مع تفاصيل واضحة تساعدك تختار بسهولة.", 21, 400, "#33025E", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("why_choose_us", 10,
                        Txt("قيمة حقيقية في كل تجربة شراء سيارة", 32, 700, "#0C0929", 1),
                        Txt("نوفر لك سيارات مختارة بعناية تجمع بين الجودة، السعر المناسب، وسهولة التعامل من أول خطوة.", 21, 500, "#33025E", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("brands", 11,
                        Txt("شركاء النجاح", 32, 700, "#0C0929", 1),
                        Txt("نتعاون مع علامات تجارية موثوقة لتقديم أفضل تجربة لعملائنا.", 21, 500, "#33025E", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),
                }
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
                    Title = Txt("تكنولوجيا المستقبل… تحت إيدك دلوقتى", 56, 700, "#1E2A60", 2, verticalSpacing: 150),
                    Subtitle = Txt("اختار من أحدث الأجهزة اللي بتتعلم منك مع الوقت، وتطوّر أدائها حسب استخدامك، وتقدم لك تجربة أسرع وأقوى من أي جهاز تقليدي.", 28, 400, "#3E4EA3", 2),
                    ButtonText = Txt("تسوق الآن", 20, 700, "#000", 2),
                    BackgroundImage = Img("https://i.ibb.co/67Jx2n12/1.png", 12, "#FFFFFF", 40)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_agency_05/contactus/e1d340c9-58c5-4e3f-9a54-b6bc5074a197.png", 6, "#FFFFFF", 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_agency_05/clientoverview/b37f711f-6e8a-4ba1-b4e6-407dcc7100f0.png", 6, "#FFFFFF", 40)
                },
                Sections = new List<SectionItem>
                {
                    Sec("featured_categories", 2,
                        Txt("استكشف فئات الأجهزة الذكية", 32, 700, "#1E2A60", 1),
                        Txt("أجهزة مصممة لتناسب كل استخدام في حياتك اليومية", 21, 500, "#3E4EA3", 1),
                        Txt("عرض الكل", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("best_selling", 3,
                        Txt("الأكثر مبيعًا هذا الشهر", 32, 700, "#1E2A60", 2),
                        Txt("منتجات أثبتت نفسها مع مرور الوقت، وحققت أعلى تقييمات من عملائنا", 21, 500, "#3E4EA3", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("collections", 4,
                        Txt("مجموعات تكنولوجية مميزة", 32, 500, "#1E2A60", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("عرض الكل", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("newsletter", 5,
                        Txt("اشترك الآن واحصل على 10% خصم لأول طلب", 32, 700, "#1E2A60", 1),
                        Txt("كن أول من يعرف عن أحدث الأجهزة التكنولوجية والعروض الحصرية قبل أي حد", 21, 400, "#3E4EA3", 1),
                        Txt("اشترك الآن", 16, 400, "#ffffff", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("offers", 6,
                        Txt("استمتع بخصومات تصل الى", 32, 700, "#1E2A60", 2),
                        Txt("جدد اسلوب حياتك مع تشكيلتنا المختارة من افضل المنتجات العالمية بأسعار تنافسية", 21, 500, "#3E4EA3", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("testimonials", 7,
                        Txt("آراء عملائنا… انعكاس لجودة منتجاتنا", 32, 500, "#1E2A60", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("contact_us", 8,
                        Txt("لنبدأ شيئاً رائعاً معاً", 32, 700, "#1E2A60", 2),
                        Txt("لديك فكرة؟ فريقنا متاح دائماً لمساعدتك في تحويل رؤيتك إلى واقع.", 16, 400, "#000000", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("new_arrivals", 9,
                        Txt("عروض ذكية لفترة محدودة", 32, 700, "#1E2A60", 2),
                        Txt("منتجات تقنية مختارة بعناية تقدم لك تجربة أفضل وقيمة أعلى قبل انتهاء العرض", 21, 400, "#3E4EA3", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("why_choose_us", 10,
                        Txt("قيمة التكنولوجيا من أول تجربة", 32, 700, "#1E2A60", 1),
                        Txt("تجربة شراء متكاملة تجمع بين أحدث الأجهزة، السعر المناسب، وسهولة التعامل", 21, 500, "#3E4EA3", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("brands", 11,
                        Txt("شركاء النجاح", 32, 700, "#1E2A60", 1),
                        Txt("نتعاون مع علامات تجارية موثوقة لتقديم أفضل تجربة لعملائنا.", 21, 500, "#3E4EA3", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),
                }
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
                    Title = Txt("موضة بتكمّل شخصيتك", 58, 700, "#6DCAFF", 1, verticalSpacing: 210),
                    Subtitle = Txt("مصممة بعناية لتناسب كل تفاصيل يومك إطلالات مرنة تلائمك في جميع المناسبات", 30, 400, "#FFF", 1),
                    ButtonText = Txt("تسوق الآن", 20, 700, "#000", 2),
                    BackgroundImage = Img("https://i.ibb.co/LDjWD1Z7/clothes.png", 12, "#FFFFFF", 40)
                },
                ContactUsImages = new ContactUsImages
                {
                    ContactUsImg = Img("https://erpsystem.runasp.net/themes/tpl_fashion_02/contactus/c9b368da-63b3-4832-9b05-1639526da783.png", 6, "#FFFFFF", 0),
                    ClientOImg = Img("https://erpsystem.runasp.net/themes/tpl_fashion_02/clientoverview/7172b183-f5e9-4acd-820a-3ade7342a8a8.png", 6, "#FFFFFF", 40)
                },
                Sections = new List<SectionItem>
                {
                    Sec("featured_categories", 2,
                        Txt("مجموعات مختارة بعناية", 32, 700, "#000", 1),
                        Txt("اخترنا لك أفضل القطع بعناية لتسهّل عليك رحلة تنسيق بيتك", 21, 500, "#56A3CF", 1),
                        Txt("عرض الكل", 14, 600, "#000", 2),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("best_selling", 3,
                        Txt("الأكثر مبيعًا هذا الشهر", 32, 700, "#000", 2, marginTop: 10),
                        Txt("منتجات أثبتت نفسها في بيوت حقيقية، وحققت أعلى تقييمات من عملائنا", 21, 500, "#56A3CF", 2, marginTop: 5),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("collections", 4,
                        Txt("مجموعات موسمية مميزة", 32, 500, "#000", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("عرض الكل", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("newsletter", 5,
                        Txt("ابقَ على اتصال مع أحدث صيحاتنا", 32, 700, "#000", 1),
                        Txt("انضم لقائمة العملاء المميزين واحصل على أحدث الإصدارات قبل أي حد.", 21, 400, "#56A3CF", 1),
                        Txt("اشترك الآن", 14, 600, "#000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("offers", 6,
                        Txt("استمتع بخصومات تصل الى", 32, 700, "#000", 2),
                        Txt("جدد اسلوب حياتك مع تشكيلتنا المختارة من افضل المنتجات العالمية بأسعار تنافسية", 21, 500, "#56A3CF", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("testimonials", 7,
                        Txt("آراء عملائنا… انعكاس لجودة منتجاتنا", 32, 500, "#56A3CF", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("contact_us", 8,
                        Txt("لنبدأ شيئاً رائعاً معاً", 32, 700, "#000", 2),
                        Txt("لديك فكرة؟ فريقنا متاح دائماً لمساعدتك في تحويل رؤيتك إلى واقع.", 16, 400, "#000000", 2),
                        Txt("فريقنا متاح دائما لخدمتك", 14, 400, "#6b6b6b", 2),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("new_arrivals", 9,
                        Txt("عروض وخصومات مميزة", 32, 700, "#000", 2),
                        Txt("خصومات حصرية على أفضل القطع الرجالية — لفترة محدودة فقط.", 21, 400, "#56A3CF", 2),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("why_choose_us", 10,
                        Txt("قصة الجودة من البداية", 32, 700, "#000", 1),
                        Txt("نحرص على تقديم تجربة شراء متكاملة تجمع بين الجودة، السعر المناسب، وسهولة التعامل.", 21, 500, "#56A3CF", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),

                    Sec("brands", 11,
                        Txt("شركاء النجاح", 32, 700, "#000", 1),
                        Txt("نتعاون مع علامات تجارية موثوقة لتقديم أفضل تجربة لعملائنا.", 21, 500, "#56A3CF", 1),
                        Txt("", 16, 400, "#000000", 0),
                        Img("", 6, "#FFFFFF", 40)),
                }
            }
        };

        // ===================== HELPERS =====================

        private static ThemeColors Colors(string primary, string secondary, string background, string text, string fontFamily) =>
            new()
            {
                Primary = primary,
                Secondary = secondary,
                Background = background,
                Text = text,
                FontFamily = fontFamily
            };

        private static SectionItem Sec(string id, int order, TextContent title, TextContent subtitle, TextContent buttonText, ImageContent backgroundImage) =>
            new()
            {
                Id = id,
                Enabled = true,
                Order = order,
                Title = title,
                Subtitle = subtitle,
                ButtonText = buttonText,
                BackgroundImage = backgroundImage
            };

        /// <summary>
        /// fontWeight and alignment are passed as the raw integer values from the source data
        /// (fontWeight: 400/500/600/700, alignment: 0/1/2) and cast directly onto the
        /// corresponding enums. Adjust the casts below if the enum's underlying values differ.
        /// </summary>
        private static TextContent Txt(
            string text,
            int fontSize,
            int fontWeight,
            string color,
            int alignment,
            int horizontalSpacing = 0,
            int verticalSpacing = 0,
            int marginTop = 0,
            string backgroundColor = null) => new()
            {
                Text = text,
                Style = new TextStyle
                {
                    FontSize = fontSize,
                    FontWeight = (FontWeight)fontWeight,
                    Color = color,
                    Alignment = (TextAlign)alignment,
                    HorizontalSpacing = horizontalSpacing,
                    VerticalSpacing = verticalSpacing,
                    MarginTop = marginTop,
                    BackgroundColor = backgroundColor
                }
            };

        private static ImageContent Img(string url, int borderRadius, string overlayColor = "#FFFFFF", int overlayOpacity = 40) => new()
        {
            Url = url,
            Style = new ImageStyle
            {
                BorderRadius = borderRadius,
                OverlayColor = overlayColor,
                OverlayOpacity = overlayOpacity
            }
        };
    }
}