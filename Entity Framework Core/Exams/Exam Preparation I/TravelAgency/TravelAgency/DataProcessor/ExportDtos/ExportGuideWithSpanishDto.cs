using System.Xml.Serialization;

namespace TravelAgency.DataProcessor.ExportDtos
{
    [XmlType("Guide")]
    public class ExportGuideWithSpanishDto
    {
        [XmlElement("FullName")]
        public string FullName { get; set; } = null!;

        [XmlArray("TourPackages")]
        public ExportTourPackageDto[] TourPackages { get; set; }
            = Array.Empty<ExportTourPackageDto>();
    }
}
