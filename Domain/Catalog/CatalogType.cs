using Domain.Attributes;

namespace Domain.Catalog;

[Auditable]
public class CatalogType
{
    public int Id { get; set; }
    public string Type { get; set; }
}