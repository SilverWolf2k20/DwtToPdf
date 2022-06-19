using DwtReader;
using DwtReader.Data;

using Usl;


void Work()
{
    var converter = new Converter();
    converter.Convert(@"D:\FQW\Входные файлы\specification_2013.dwt");
}

Work();
GC.Collect();
Console.WriteLine("Программа завершила работу.");
Console.Read();

public class Converter
{
    #region Constructor and destructor
    public Converter()
    {
        Logger.Level = LogLevel.Trace;
        //Logger.WriteToFile("Converter.log");
    }

    ~Converter()
    {
        Logger.Trace("Сохранение логов.");
        Logger.SaveFile();
    }
    #endregion

    #region Public methods
    public bool Convert(string dwtPath, string pdfPath = null)
    {
        Logger.Info("Начало декодирования DWT файла.");
        DwtFile dwtFile = ImportDwtFile(dwtPath);
        Logger.Info("Конец декодирования DWT файла.");

        // Проверка на успешность чтения.
        if (dwtFile == null)
            return false;

        // Автоматическое определение имени PDF файла.
        if (pdfPath is null)
            pdfPath = Path.ChangeExtension(dwtPath, ".pdf");

        Logger.Info("Начало записи в PDF файл.");
        var result = ExportToPdfFile(null, pdfPath);
        Logger.Info("Конец записи в PDF файл.");

        // Проверка на успешность записи.
        if (result == false)
            return false;

        return true;
    }
    #endregion

    #region Private methods
    private DwtFile ImportDwtFile(string dwtPath)
    {
        DwtFileReader fileReader = new DwtFileReader(dwtPath);

        Logger.Trace($"Анализ файла.");
        if (fileReader.FileAnalysis() == false) {
            Console.WriteLine(fileReader.State);
            return null;
        }

        Logger.Trace($"Чтение файла: {Path.GetFileName(dwtPath)}");
        return fileReader.Read();
    }

    private bool ExportToPdfFile(DwtFile dwtFile, string pdfPath)
    {
        Logger.Trace($"Запись в: {Path.GetFileName(pdfPath)}");
        //var drawer = new PdfDrawer(pdfPath);
        //return drawer.Draw(dwtFile);

        return true;
    }
    #endregion
}