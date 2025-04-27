using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using BiodiversityCloudApp.DTOs;

namespace BiodiversityCloudApp.Data
{
    public class MicroObservationReportGenerator
    {
        public byte[] Generate(List<MicroObservationDto> observations)
        {
            var document = new MicroObservationReportDocument(observations);
            return document.GeneratePdf();
        }
    }

    public class MicroObservationReportDocument : IDocument
    {
        private readonly List<MicroObservationDto> observations;

        public MicroObservationReportDocument(List<MicroObservationDto> observations)
        {
            this.observations = observations;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(30);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(12));

                page.Header()
                    .Text("Micro Observations Report")
                    .SemiBold().FontSize(20).FontColor(Colors.Blue.Medium);

                page.Content().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    // Table header
                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyleHeader).Text("Latitude");
                        header.Cell().Element(CellStyleHeader).Text("Longitude");
                        header.Cell().Element(CellStyleHeader).Text("Timestamp");
                        header.Cell().Element(CellStyleHeader).Text("Comment");
                        header.Cell().Element(CellStyleHeader).Text("Photo");
                        header.Cell().Element(CellStyleHeader).Text("Animal ID");

                        static IContainer CellStyleHeader(IContainer container) => container
                            .Padding(5)
                            .Background(Colors.Grey.Lighten2)
                            .Border(1)
                            .AlignCenter();
                    });

                    // Table rows
                    foreach (var obs in observations)
                    {
                        table.Cell().Element(CellStyleRow).Text($"{obs.Latitude}");
                        table.Cell().Element(CellStyleRow).Text($"{obs.Longitude}");
                        table.Cell().Element(CellStyleRow).Text($"{obs.Timestamp:yyyy-MM-dd HH:mm}");
                        table.Cell().Element(CellStyleRow).Text(obs.Comment ?? "N/A");
                        table.Cell().Element(CellStyleRow).Text(string.IsNullOrEmpty(obs.PhotoUrl) ? "No" : "Yes");
                        table.Cell().Element(CellStyleRow).Text(obs.AnimalId.ToString());

                        static IContainer CellStyleRow(IContainer container) => container
                            .BorderBottom(1)
                            .PaddingVertical(3)
                            .AlignLeft();
                    }
                });

                page.Footer()
                    .AlignCenter()
                    .Text($"Generated on {DateTime.UtcNow:yyyy-MM-dd HH:mm} UTC")
                    .FontSize(10).FontColor(Colors.Grey.Darken1);
            });
        }
    }
}
