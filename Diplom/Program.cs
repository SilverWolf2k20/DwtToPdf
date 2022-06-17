using DwtToPdf;

void Work()
{
    var converter = new Converter();
    converter.Convert(@"D:\FQW\Входные файлы\specification_2013.dwt");
}

Work();
GC.Collect();
Console.WriteLine("Программа завершила работу.");
Console.Read();