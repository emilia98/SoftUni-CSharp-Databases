namespace Cadastre.DataProcessor.ExportDtos
{
    public class ExportPropertyDto
    {
        public string PropertyIdentifier { get; set; } = null!;

        public int Area { get; set; }

        public string Address { get; set; } = null!;

        public string DateOfAcquisition { get; set; } = null!;

        public ICollection<ExportOwnerDto> Owners { get; set; } = Array.Empty<ExportOwnerDto>();
    }
}
