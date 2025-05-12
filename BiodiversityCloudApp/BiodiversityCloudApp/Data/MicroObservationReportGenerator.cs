using BiodiversityCloudApp.DTOs;
using BiodiversityCloudApp.DTOs.Common;
using BiodiversityCloudApp.DTOs.ObservationRecords;
using BiodiversityCloudApp.DTOs.Observations;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BiodiversityCloudApp.Controllers
{
    public class MicroObservationReportDocument : IDocument
    {
        private readonly List<ObservationRecordDto> _observations;
        private readonly Dictionary<Guid, AnimalDto> _animalData;
        private readonly Dictionary<Guid, ObservationDto> _observationData;
        private readonly string _userName;
        private readonly DateTime _reportDate;

        public MicroObservationReportDocument(
            List<ObservationRecordDto> observations,
            Dictionary<Guid, AnimalDto> animalData,
            Dictionary<Guid, ObservationDto> observationData,
            string userName = "Bob Charlie")
        {
            _observations = observations;
            _animalData = animalData;
            _observationData = observationData;
            _userName = userName;
            _reportDate = DateTime.UtcNow;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            container.Page(page =>
            {
                page.Margin(20);
                page.Size(PageSizes.A4);
                page.PageColor(Colors.White);
                page.DefaultTextStyle(x => x.FontSize(10));

                ComposeHeader(page);

                page.Content().Element(content =>
                {
                    content.Column(column =>
                    {
                        ComposeUserSummary(column);
                        ComposeSummarySection(column);
                        ComposeEcologicalIndices(column);
                        ComposeSpeciesAnalysis(column);
                        ComposeSpeciesChart(column);
                        ComposeSpeciesPieChart(column);
                        ComposeTemporalAnalysis(column);
                        ComposeMonthlyChart(column);
                        ComposeSpatialAnalysis(column);
                        ComposeDetailedObservations(column);
                    });
                });

                ComposeFooter(page);
            });
        }

        private void ComposeHeader(PageDescriptor page)
        {
            page.Header()
                .PaddingBottom(10)
                .Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text("Biodiversity Observation Report")
                            .FontSize(20).Bold().FontColor(Colors.Blue.Medium);

                        column.Item().Text($"Generated on {_reportDate:yyyy-MM-dd HH:mm} UTC")
                            .FontSize(10).FontColor(Colors.Grey.Medium);
                    });
                });
        }

        private void ComposeUserSummary(ColumnDescriptor column)
        {
            column.Item().PaddingBottom(10).Column(innerColumn =>
            {
                innerColumn.Item().Text($"Report for: {_userName}").FontSize(14).Bold();

                var firstObservation = _observations.OrderBy(o => o.Timestamp).FirstOrDefault();
                var lastObservation = _observations.OrderByDescending(o => o.Timestamp).FirstOrDefault();

                if (firstObservation != null && lastObservation != null)
                {
                    innerColumn.Item().Text($"Observation period: {firstObservation.Timestamp:yyyy-MM-dd} to {lastObservation.Timestamp:yyyy-MM-dd}")
                        .FontSize(12);
                }
            });
        }

        private void ComposeSummarySection(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).Column(innerColumn =>
            {
                innerColumn.Item().Text("Summary").FontSize(16).Bold();

                var uniqueSpecies = _observations.Select(o => o.AnimalId).Distinct().Count();
                var totalPhotos = _observations.Sum(o => o.PhotoIds?.Count ?? 0);
                var observationsWithComments = _observations.Count(o => !string.IsNullOrEmpty(o.Comment));

                innerColumn.Item().Grid(grid =>
                {
                    grid.Columns(4);
                    grid.Spacing(5);

                    grid.Item().Background(Colors.Grey.Lighten3).Padding(5).Column(col =>
                    {
                        col.Item().Text("Total Observations").Bold();
                        col.Item().Text(_observations.Count.ToString());
                    });

                    grid.Item().Background(Colors.Grey.Lighten3).Padding(5).Column(col =>
                    {
                        col.Item().Text("Unique Species").Bold();
                        col.Item().Text(uniqueSpecies.ToString());
                    });

                    grid.Item().Background(Colors.Grey.Lighten3).Padding(5).Column(col =>
                    {
                        col.Item().Text("Total Photos").Bold();
                        col.Item().Text(totalPhotos.ToString());
                    });

                    grid.Item().Background(Colors.Grey.Lighten3).Padding(5).Column(col =>
                    {
                        col.Item().Text("Detailed Records").Bold();
                        col.Item().Text($"{observationsWithComments} ({(observationsWithComments * 100 / Math.Max(1, _observations.Count))}%)");
                    });
                });
            });
        }

        private void ComposeEcologicalIndices(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).Column(innerColumn =>
            {
                innerColumn.Item().Text("Ecological Indices").FontSize(16).Bold();

                var speciesCounts = _observations
                    .GroupBy(o => o.AnimalId)
                    .Select(g => g.Count())
                    .ToList();

                var richness = speciesCounts.Count;
                var totalIndividuals = speciesCounts.Sum();
                var shannonIndex = CalculateShannonIndex(speciesCounts);
                var simpsonIndex = CalculateSimpsonIndex(speciesCounts);
                var evenness = shannonIndex / Math.Max(1, Math.Log(richness));

                innerColumn.Item().Grid(grid =>
                {
                    grid.Columns(3);
                    grid.Spacing(5);

                    grid.Item().Background(Colors.Green.Lighten4).Padding(5).Column(col =>
                    {
                        col.Item().Text("Species Richness").Bold();
                        col.Item().Text(richness.ToString());
                    });

                    grid.Item().Background(Colors.Green.Lighten4).Padding(5).Column(col =>
                    {
                        col.Item().Text("Shannon Index").Bold();
                        col.Item().Text(shannonIndex.ToString("F2"));
                    });

                    grid.Item().Background(Colors.Green.Lighten4).Padding(5).Column(col =>
                    {
                        col.Item().Text("Simpson Index").Bold();
                        col.Item().Text(simpsonIndex.ToString("F2"));
                    });

                    grid.Item().Background(Colors.Green.Lighten4).Padding(5).Column(col =>
                    {
                        col.Item().Text("Total Individuals").Bold();
                        col.Item().Text(totalIndividuals.ToString());
                    });

                    grid.Item().Background(Colors.Green.Lighten4).Padding(5).Column(col =>
                    {
                        col.Item().Text("Pielou's Evenness").Bold();
                        col.Item().Text(evenness.ToString("F2"));
                    });
                });
            });
        }

        private float CalculateShannonIndex(List<int> speciesCounts)
        {
            float total = speciesCounts.Sum();
            if (total == 0) return 0;

            float index = 0;
            foreach (var count in speciesCounts)
            {
                if (count > 0)
                {
                    float proportion = count / total;
                    index -= proportion * (float)Math.Log(proportion);
                }
            }
            return index;
        }

        private float CalculateSimpsonIndex(List<int> speciesCounts)
        {
            float total = speciesCounts.Sum();
            if (total == 0) return 0;

            float index = 0;
            foreach (var count in speciesCounts)
            {
                float proportion = count / total;
                index += proportion * proportion;
            }
            return 1 - index; // Simpson's Diversity Index (1-D)
        }

        private void ComposeSpeciesAnalysis(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).Column(innerColumn =>
            {
                innerColumn.Item().Text("Species Analysis").FontSize(16).Bold();

                var speciesCounts = GetSpeciesCounts();

                innerColumn.Item().Text("Top 10 Observed Species").FontSize(14);
                innerColumn.Item().PaddingBottom(5);

                innerColumn.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Species");
                        header.Cell().Element(CellStyle).AlignRight().Text("Count");
                        header.Cell().Element(CellStyle).AlignRight().Text("Relative %");
                    });

                    var totalCount = speciesCounts.Sum(x => x.Count);
                    foreach (var species in speciesCounts)
                    {
                        var percentage = (species.Count * 100f) / totalCount;
                        table.Cell().Element(CellStyle).Text(species.AnimalName);
                        table.Cell().Element(CellStyle).AlignRight().Text(species.Count.ToString());
                        table.Cell().Element(CellStyle).AlignRight().Text(percentage.ToString("F1") + "%");
                    }
                });
            });
        }

        private void ComposeSpeciesChart(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).Column(innerColumn =>
            {
                innerColumn.Item().Text("Species Abundance").FontSize(16).Bold();

                var speciesCounts = GetSpeciesCounts();
                var maxCount = speciesCounts.Max(x => x.Count);

                innerColumn.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(5);
                        columns.RelativeColumn();
                    });

                    foreach (var species in speciesCounts)
                    {
                        var percentage = (species.Count * 100f) / maxCount;

                        table.Cell().Element(CellStyle).Text(species.AnimalName);
                        table.Cell().Element(CellStyle).AlignLeft()
                            .Background(Colors.Blue.Lighten3)
                            .Width(percentage / 100f)
                            .Height(15)
                            .Text("");
                        table.Cell().Element(CellStyle).AlignRight().Text($"{species.Count} ({percentage:F1}%)");
                    }
                });
            });
        }

        private void ComposeSpeciesPieChart(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).Column(innerColumn =>
            {
                innerColumn.Item().Text("Species Composition").FontSize(16).Bold();

                var speciesCounts = GetSpeciesCounts().Take(8).ToList();
                var othersCount = GetSpeciesCounts().Skip(8).Sum(x => x.Count);
                var totalCount = speciesCounts.Sum(x => x.Count) + (othersCount > 0 ? othersCount : 0);

                if (othersCount > 0)
                {
                    speciesCounts.Add(("Other species", othersCount, 0));
                }

                var colors = new[]
                {
            Colors.Blue.Medium,
            Colors.Green.Medium,
            Colors.Red.Medium,
            Colors.Orange.Medium,
            Colors.Purple.Medium,
            Colors.Teal.Medium,
            Colors.Pink.Medium,
            Colors.Yellow.Medium,
            Colors.Grey.Medium
        };

                // Visualization using a grid of colored squares
                innerColumn.Item().Grid(grid =>
                {
                    grid.Columns(2);
                    grid.Spacing(10);

                    // Visual representation using proportional squares
                    grid.Item().Stack(stack =>
                    {
                        // Proper padding implementation for text
                        stack.Item().PaddingBottom(5).Text("Visual Representation:");

                        // Create a grid of colored squares proportional to species count
                        int totalSquares = 100; // Total squares to represent 100%
                        foreach (var (species, i) in speciesCounts.Select((s, i) => (s, i)))
                        {
                            var percentage = (species.Count * 100f) / totalCount;
                            var squaresCount = (int)Math.Round(totalSquares * percentage / 100);

                            // Proper padding implementation for rows
                            stack.Item().Container().PaddingBottom(5).Row(row =>
                            {
                                row.RelativeItem(3).Text(species.AnimalName);
                                row.RelativeItem(7).Grid(squaresGrid =>
                                {
                                    squaresGrid.Columns(10); // 10 columns for the squares
                                    squaresGrid.Spacing(2);

                                    for (int j = 0; j < squaresCount; j++)
                                    {
                                        squaresGrid.Item()
                                            .Height(10)
                                            .Width(10)
                                            .Background(colors[i % colors.Length]);
                                    }
                                });
                            });
                        }
                    });

                    // Detailed legend with counts
                    grid.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn();
                            columns.RelativeColumn(3);
                            columns.RelativeColumn();
                        });

                        table.Header(header =>
                        {
                            header.Cell().Element(CellStyle).Text("");
                            header.Cell().Element(CellStyle).Text("Species");
                            header.Cell().Element(CellStyle).AlignRight().Text("Count (%)");
                        });

                        foreach (var (species, i) in speciesCounts.Select((s, i) => (s, i)))
                        {
                            var percentage = (species.Count * 100f) / totalCount;
                            table.Cell().Element(CellStyle).Height(10).Width(10).Background(colors[i % colors.Length]);
                            table.Cell().Element(CellStyle).Text(species.AnimalName);
                            table.Cell().Element(CellStyle).AlignRight().Text($"{species.Count} ({percentage:F1}%)");
                        }
                    });
                });
            });
        }
        private void ComposeTemporalAnalysis(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).Column(innerColumn =>
            {
                innerColumn.Item().Text("Temporal Analysis").FontSize(16).Bold();

                var monthlyData = GetMonthlyData();

                innerColumn.Item().Text("Observations by Month").FontSize(14);
                innerColumn.Item().PaddingBottom(5);

                innerColumn.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(5);
                        columns.RelativeColumn();
                    });

                    foreach (var month in monthlyData)
                    {
                        table.Cell().Element(CellStyle).Text(month.Period);
                        table.Cell().Element(CellStyle).Text("");
                        table.Cell().Element(CellStyle).AlignRight().Text(month.Count.ToString());
                    }
                });
            });
        }

        private void ComposeMonthlyChart(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).Column(innerColumn =>
            {
                innerColumn.Item().Text("Monthly Observation Trends").FontSize(16).Bold();

                var monthlyData = GetMonthlyData();
                var maxCount = monthlyData.Max(x => x.Count);

                innerColumn.Item().PaddingTop(5).Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(3);
                        columns.RelativeColumn(5);
                        columns.RelativeColumn();
                    });

                    foreach (var month in monthlyData)
                    {
                        var percentage = (month.Count * 100f) / maxCount;

                        table.Cell().Element(CellStyle).Text(month.Period);
                        table.Cell().Element(CellStyle).AlignLeft()
                            .Background(Colors.Green.Lighten3)
                            .Width(percentage / 100f)
                            .Height(15)
                            .Text("");
                        table.Cell().Element(CellStyle).AlignRight().Text($"{month.Count} ({percentage:F1}%)");
                    }
                });
            });
        }

        private void ComposeSpatialAnalysis(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).Column(innerColumn =>
            {
                innerColumn.Item().Text("Spatial Analysis").FontSize(16).Bold();

                var topLocations = _observations
                    .GroupBy(o => $"{Math.Round(o.Location.Latitude, 2)}, {Math.Round(o.Location.Longitude, 2)}")
                    .OrderByDescending(g => g.Count())
                    .Take(5)
                    .ToList();

                innerColumn.Item().Text("Top 5 Observation Locations").FontSize(14);
                innerColumn.Item().PaddingBottom(5);

                innerColumn.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Location");
                        header.Cell().Element(CellStyle).AlignRight().Text("Observations");
                    });

                    foreach (var location in topLocations)
                    {
                        table.Cell().Element(CellStyle).Text(location.Key);
                        table.Cell().Element(CellStyle).AlignRight().Text(location.Count().ToString());
                    }
                });
            });
        }

        private void ComposeDetailedObservations(ColumnDescriptor column)
        {
            column.Item().PaddingVertical(10).Column(innerColumn =>
            {
                innerColumn.Item().Text("Detailed Observations").FontSize(16).Bold();

                innerColumn.Item().Table(table =>
                {
                    table.ColumnsDefinition(columns =>
                    {
                        columns.RelativeColumn(2);
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn();
                        columns.RelativeColumn(3);
                    });

                    table.Header(header =>
                    {
                        header.Cell().Element(CellStyle).Text("Species");
                        header.Cell().Element(CellStyle).Text("Date/Time");
                        header.Cell().Element(CellStyle).Text("Location");
                        header.Cell().Element(CellStyle).Text("Photos");
                        header.Cell().Element(CellStyle).Text("Comment");
                    });

                    foreach (var obs in _observations.OrderByDescending(o => o.Timestamp))
                    {
                        var animalName = _animalData.TryGetValue(obs.AnimalId, out var animal)
                            ? animal.Name
                            : "Unknown";

                        table.Cell().Element(CellStyle).Text(animalName);
                        table.Cell().Element(CellStyle).Text(obs.Timestamp.ToString("yyyy-MM-dd HH:mm"));
                        table.Cell().Element(CellStyle).Text($"{obs.Location.Latitude:N4}, {obs.Location.Longitude:N4}");
                        table.Cell().Element(CellStyle).Text(obs.PhotoIds?.Count.ToString() ?? "0");
                        table.Cell().Element(CellStyle).Text(obs.Comment ?? string.Empty);
                    }
                });
            });
        }

        private void ComposeFooter(PageDescriptor page)
        {
            page.Footer()
                .AlignCenter()
                .Text(text =>
                {
                    text.Span("Page ").FontColor(Colors.Grey.Medium);
                    text.CurrentPageNumber().FontColor(Colors.Grey.Medium);
                    text.Span(" of ").FontColor(Colors.Grey.Medium);
                    text.TotalPages().FontColor(Colors.Grey.Medium);
                });
        }

        private List<(string AnimalName, int Count, int Photos)> GetSpeciesCounts()
        {
            return _observations
                .GroupBy(o => o.AnimalId)
                .Select(g => (
                    AnimalName: _animalData.TryGetValue(g.Key, out var animal) ? animal.Name : "Unknown",
                    Count: g.Count(),
                    Photos: g.Sum(x => x.PhotoIds?.Count ?? 0)
                ))
                .OrderByDescending(x => x.Count)
                .Take(10)
                .ToList();
        }

        private List<(string Period, int Count)> GetMonthlyData()
        {
            return _observations
                .GroupBy(o => new { o.Timestamp.Year, o.Timestamp.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .Select(g => (Period: $"{g.Key.Year}-{g.Key.Month:00}", Count: g.Count()))
                .ToList();
        }

        private static IContainer CellStyle(IContainer container) => container
            .BorderBottom(1).PaddingVertical(3).PaddingHorizontal(2);
    }
}