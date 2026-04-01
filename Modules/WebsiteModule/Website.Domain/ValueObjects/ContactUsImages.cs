namespace Website.Domain.ValueObjects
{
    /// <summary>
    /// Groups the two images used on the Contact Us page.
    /// Each image follows the same structure as ImageContent (Url + Style).
    /// </summary>
    public class ContactUsImages
    {
        public ImageContent ContactUsImg { get; set; } = new();
        public ImageContent ClientOImg   { get; set; } = new();
    }
}
