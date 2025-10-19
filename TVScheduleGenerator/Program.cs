using ClosedXML.Excel;

public class ConfigurationConstants
{
    public readonly String PATH_FROM_EXE = "../../..";

    public readonly String WORKBOOK_NAME = "БД.xlsx";

    public readonly String PROGRAMS_WORKSHEET_NAME = "Программы";
    public readonly String SCHEDULES_WORKSHEET_NAME = "Расписание";

    public readonly Int32[] programDurationVariantsInMinutes = [5, 10, 15, 30, 45, 60, 90];

    public readonly DatePeriod GENERATION_RANGE = new(DateTime.Parse("18/10/2025"), DateTime.Parse("18/10/2025"));
}

public class Program
{
    static readonly ConfigurationConstants constants = new();
    static readonly XLDataLoader dataLoader = new(constants);

    private static void Main()
    {
        List<ScheduleRow> generatedScheduleRows = SchedulesGenerator.GenerateScheduleRowsByChannelIds(new(dataLoader.GetChannelIds(), constants.GENERATION_RANGE, dataLoader.GetGroupedChannelAndProgramIds(), constants.programDurationVariantsInMinutes));

        dataLoader.AddScheduleRowsInWorksheet(generatedScheduleRows);

        dataLoader.SaveWorkbook();
    }
}

public static class SchedulesGenerator
{
    public static List<ScheduleRow> GenerateScheduleRowsByChannelIds(ScheduleRowsGenerationParameters parameters)
    {
        List<ScheduleRow> generatedScheduleRowsByChannelId = new();

        List<Int32> chanmelIds = parameters.ChannelIds.Distinct().ToList();

        foreach (Int32 channelId in chanmelIds)
        {
            generatedScheduleRowsByChannelId.AddRange(GenerateScheduleRows(new(channelId, parameters.GenerationRange, parameters.GroupedChannelAndProgramIds, parameters.ProgramDurationVariantsInMinutes)));
        }

        return generatedScheduleRowsByChannelId;
    }

    public static List<ScheduleRow> GenerateScheduleRows(ScheduleRowGenerationParameters parameters)
    {
        List<ScheduleRow> generatedScheduleRows = new();

        parameters = parameters with { GroupedChannelAndProgramIds = FilterGroupedChannelAndProgramIdsByChannelId(parameters.GroupedChannelAndProgramIds, parameters.ChannelId) };

        ScheduleRow generatedScheduleRow = GenerateNewScheduleRow(parameters);

 

        while (parameters.GenerationRange.Contains(generatedScheduleRow.ProgramDuration))
        {
            parameters = parameters with { GenerationRange = ScheduleRowGenerationParameters.GetMovedDownGenerationRange(parameters.GenerationRange, generatedScheduleRow.ProgramDuration.GetDuration()) };
            generatedScheduleRows.Add(generatedScheduleRow);

            generatedScheduleRow = GenerateNewScheduleRow(parameters);
        }

        return generatedScheduleRows;
    }

    private static List<ChannelAndProgramIds> FilterGroupedChannelAndProgramIdsByChannelId(List<ChannelAndProgramIds> groupedChannelAndProgramIds, Int32 channelId)
    {
        return groupedChannelAndProgramIds.Where(g => g.ChannelId == channelId)
                                          .ToList();
    }

    public static ScheduleRow GenerateNewScheduleRow(ScheduleRowGenerationParameters parameters)
    {
        return GetNewScheduleRow(parameters.GroupedChannelAndProgramIds.GetRandomValue<List<ChannelAndProgramIds>, ChannelAndProgramIds>(), ScheduleRowGenerationParameters.GetProgramDurationAsDatePeriod(parameters.GenerationRange, parameters.ProgramDurationVariantsInMinutes));
    }

    public static ScheduleRow GetNewScheduleRow(ChannelAndProgramIds groupedChannelAndProgramIds, DatePeriod period)
    {
        return new(groupedChannelAndProgramIds.ChannelId, groupedChannelAndProgramIds.ProgramId, period);
    }
}

public static class IEnumerableExtensions
{
    private static Random random = new();

    public static V GetRandomValue<C, V>(this C collection) where C: IEnumerable<V>, IList<V>
    {
        return collection[random.Next(collection.Count())];
    }
}

public record ScheduleRowsGenerationParameters(List<Int32> ChannelIds, DatePeriod GenerationRange, List<ChannelAndProgramIds> GroupedChannelAndProgramIds, Int32[] ProgramDurationVariantsInMinutes);

public record ScheduleRowGenerationParameters(Int32 ChannelId, DatePeriod GenerationRange, List<ChannelAndProgramIds> GroupedChannelAndProgramIds, Int32[] ProgramDurationVariantsInMinutes)
{
    public static DatePeriod GetProgramDurationAsDatePeriod(DatePeriod generationRange, Int32[] programDurationVariantsInMinutes)
    {
        return new(generationRange.From, programDurationVariantsInMinutes.GetRandomValue<Int32[], Int32>());
    }

    public static DatePeriod GetMovedDownGenerationRange(DatePeriod generationRange, Int32 duration)
    {
        return new(generationRange.From.AddMinutes(duration), generationRange.To);
    }
}

public record ScheduleRow(Int32 ChannelId, Int32 ProgramId, DatePeriod ProgramDuration);

public static class DatePeriodExtensions
{
    public static Boolean Contains(this DatePeriod leftPeriod, DatePeriod rightPeriod)
    {
        return rightPeriod.From >= leftPeriod.From && rightPeriod.To <= leftPeriod.To;
    }

    public static Int32 GetDuration(this DatePeriod datePeriod)
    {
        return (Int32)datePeriod.To.Subtract(datePeriod.From).TotalMinutes;
    }
}

public record DatePeriod
{
    public DateTime From { get; init; }
    public DateTime To { get; init; }

    public DatePeriod(DateTime from, Int32 durationInMinutes) : this(from, from.AddMinutes(durationInMinutes))
    { }

    public DatePeriod(DateTime from, DateTime to)
    {
        From = from;
        To = to;
    }
}

public record ChannelAndProgramIds(Int32 ChannelId, Int32 ProgramId);

public class XLDataLoader
{
    public readonly XlWorkbookAdapter XlWorkbook;

    //public readonly XLWorksheetAdapter channelsWorksheet;
    public readonly XLWorksheetAdapter programsWorksheet;
    public readonly XLWorksheetAdapter schedulesWorksheet;

    public XLDataLoader(ConfigurationConstants constants)
    {
        XlWorkbook = new(Path.Join(constants.PATH_FROM_EXE, constants.WORKBOOK_NAME));

        //channelsWorksheet = new(inputXlWorkbook.GetWorksheet(CHANNELS_WORKSHEET_NAME));
        programsWorksheet = new(XlWorkbook.GetWorksheet(constants.PROGRAMS_WORKSHEET_NAME));
        schedulesWorksheet = new(XlWorkbook.GetWorksheet(constants.SCHEDULES_WORKSHEET_NAME));
    }

    public void AddScheduleRowsInWorksheet(List<ScheduleRow> scheduleRows)
    {
        for (Int32 I = 0; I < scheduleRows.Count; I++)
        {
            AddScheduleRowInWorksheet(scheduleRows[I], I);
        }
    }

    public void AddScheduleRowInWorksheet(ScheduleRow scheduleRow, Int32 rowIndex)
    {
        schedulesWorksheet.BulkInsert(rowIndex + 1, scheduleRow);
    }

    public List<ChannelAndProgramIds> GetGroupedChannelAndProgramIds()
    {
        return GetChannelIds().Zip(GetProgramIds())
                              .Select(g => new ChannelAndProgramIds(g.First, g.Second))
                              .ToList();
    }

    public List<Int32> GetChannelIds()
    {
        return programsWorksheet.GetCellsValue("B:B")
                                .Select(ConvertCellValueToInt32)
                                .ToList();
    }

    public List<Int32> GetProgramIds()
    {
        return programsWorksheet.GetCellsValue("A:A")
                                .Select(ConvertCellValueToInt32)
                                .ToList();
    }

    public Int32 ConvertCellValueToInt32(XLCellValue cellValue)
    {
        return (Int32)cellValue.GetNumber();
    }

    public void SaveWorkbook()
    {
        XlWorkbook.SaveWorksheetInFile();
    }
}

public class XlWorkbookAdapter
{
    readonly String workbookPath;
    readonly XLWorkbook workbook;

    public XlWorkbookAdapter(String name)
    {
        workbookPath = Path.Join(Directory.GetCurrentDirectory(), name);

        workbook = new XLWorkbook(workbookPath);
    }

    public IXLWorksheet GetWorksheet(String name)
    {
        return workbook.Worksheet(name);
    }

    public void AddWorksheet(String name)
    {
        workbook.AddWorksheet(name);
    }

    public void SaveWorksheetInFile()
    {
        workbook.SaveAs(workbookPath);
    }
}

public class XLWorksheetAdapter
{
    readonly IXLWorksheet worksheet;

    public XLWorksheetAdapter(IXLWorksheet worksheet)
    {
        this.worksheet = worksheet;
    }

    public XLCellValue GetCellValue(Cell cell)
    {
        return GetCellValue(cell.Address);
    }

    public void SetCellValue(Cell cell)
    {
        SetCellValue(cell.Address, cell.Value);
    }

    public void BulkInsert(Int32 rowIndex, ScheduleRow scheduleRow)
    {
        worksheet.Cell(rowIndex, 2).Value = scheduleRow.ChannelId;
        worksheet.Cell(rowIndex, 3).Value = scheduleRow.ProgramId;
        worksheet.Cell(rowIndex, 4).Value = scheduleRow.ProgramDuration.From;
        worksheet.Cell(rowIndex, 5).Value = scheduleRow.ProgramDuration.To;
    }

    public XLCellValue GetCellValue(String cellAdress)
    {
        return worksheet.Cell(cellAdress).Value;
    }

    public void SetCellValue(String cellAdress, XLCellValue value)
    {
        worksheet.Cell(cellAdress).Value = value;
    }

    public List<XLCellValue> GetCellsValue(String address)
    {
        return worksheet.Range(address)
                        .CellsUsed()
                        .Select(c => c.Value)
                        .ToList();
    }
}

public record Cell(String Address, XLCellValue Value);