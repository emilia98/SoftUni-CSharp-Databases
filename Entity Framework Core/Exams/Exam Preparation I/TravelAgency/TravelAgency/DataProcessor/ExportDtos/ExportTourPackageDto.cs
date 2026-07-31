using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("TourPackage")]
    public class ExportTourPackageDto
    {
        [XmlElement("Name")]
        public string Name { get; set; } = null!;

        [XmlElement("Description")]
        public string Description { get; set; } = string.Empty;

        [XmlElement("Price")]
        public string Price { get; set; } = null!;
    }
}
