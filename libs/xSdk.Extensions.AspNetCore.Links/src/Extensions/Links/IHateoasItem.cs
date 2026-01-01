namespace xSdk.Extensions.Links
{
    public interface IHateoasItem
    {
        string Rel { get; set; }

        string Href { get; set; }

        string Method { get; set; }
    }
}
