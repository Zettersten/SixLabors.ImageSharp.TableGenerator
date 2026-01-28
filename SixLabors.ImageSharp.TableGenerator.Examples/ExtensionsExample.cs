using BlazorFast.ImageSharp.TableGenerator.Extensions;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp;


namespace BlazorFast.ImageSharp.TableGenerator.Examples;

public static class ExtensionsExample
{
    public record Person(string Name, int Age, string City);

    public static void Run()
    {
        var people = new[]
        {
            new Person("Alice", 30, "New York"),
            new Person("Bob", 25, "San Francisco"),
            new Person("Charlie", 35, "Seattle"),
            new Person("Diana", 28, "Austin"),
            new Person("Eve", 32, "Boston"),
        };

        // Example 1: Default light theme
        var lightTable = people.ToTableImage();
        lightTable.Save("output/extension-light-theme.png");
        Console.WriteLine("Created: extension-light-theme.png");

        // Example 2: Dark theme
        var darkTable = people.ToTableImage(new TableGeneratorOptions { Theme = ThemeMode.Dark });
        darkTable.Save("output/extension-dark-theme.png");
        Console.WriteLine("Created: extension-dark-theme.png");

        // Example 3: Minimal theme
        var minimalTable = people.ToTableImage(
            new TableGeneratorOptions { Theme = ThemeMode.Minimal }
        );
        minimalTable.Save("output/extension-minimal-theme.png");
        Console.WriteLine("Created: extension-minimal-theme.png");

        // Example 4: Compact theme
        var compactTable = people.ToTableImage(
            new TableGeneratorOptions { Theme = ThemeMode.Compact }
        );
        compactTable.Save("output/extension-compact-theme.png");
        Console.WriteLine("Created: extension-compact-theme.png");

        // Example 5: Custom property filtering
        var filteredTable = people.ToTableImage(
            new TableGeneratorOptions
            {
                PropertyFilter = prop => prop.Name != "City", // Exclude City column
            }
        );
        filteredTable.Save("output/extension-filtered-properties.png");
        Console.WriteLine("Created: extension-filtered-properties.png");

        // Example 6: Custom formatters
        var formattedTable = people.ToTableImage(
            new TableGeneratorOptions
            {
                Theme = ThemeMode.Dark,
                PropertyNameFormatter = prop => prop.Name.ToUpper(),
                ValueFormatter = val => val?.ToString()?.ToUpper() ?? "N/A",
            }
        );
        formattedTable.Save("output/extension-custom-formatters.png");
        Console.WriteLine("Created: extension-custom-formatters.png");

        // Example 7: Property ordering
        var orderedTable = people.ToTableImage(
            new TableGeneratorOptions { PropertyOrder = new[] { "City", "Name", "Age" } }
        );
        orderedTable.Save("output/extension-property-order.png");
        Console.WriteLine("Created: extension-property-order.png");

        // Example 8: No headers
        var noHeadersTable = people.ToTableImage(
            new TableGeneratorOptions { IncludeHeaders = false }
        );
        noHeadersTable.Save("output/extension-no-headers.png");
        Console.WriteLine("Created: extension-no-headers.png");
    }
}
