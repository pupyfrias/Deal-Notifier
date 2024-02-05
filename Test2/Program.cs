// See https://aka.ms/new-console-template for more information

using DealNotifier.Core.Application.Enums;

var name = "New Samsung Montage Black and Silver (Virgin Mobile) PayLo";
/*
int brandId = Enum.GetValues<Brand>()
                 .Select(e => (int)e)
                 .FirstOrDefault(e => name.Contains(e.ToString(), StringComparison.OrdinalIgnoreCase));

Console.WriteLine(Brand.Samsung.ToString());
Console.WriteLine(brandId);
*/


// Esto devolverá 0 si no encuentra coincidencias

// Mostrar resultado
Console.WriteLine($"Brand ID: {brandId}");