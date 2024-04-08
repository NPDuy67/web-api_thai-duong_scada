using System.Collections.Immutable;
using System.Diagnostics;

namespace ThaiDuongScada.Domain.AggregateModels.ShiftReportAggregate;
public static class ShiftTimeHelper
{
    public static readonly TimeSpan ShortShift1StartTime = new(6, 0, 0);
    public static readonly TimeSpan ShortShift2StartTime = new(14, 0, 0);
    public static readonly TimeSpan ShortShift3StartTime = new(22, 0, 0);
    public static readonly TimeSpan LongShift1StartTime = new(10, 0, 0);
    public static readonly TimeSpan LongShift2StartTime = new(22, 0, 0);

    public static DateTime GetVietnamTime()
    {
        return DateTime.UtcNow.AddHours(7);
    }

    public static int GetShiftNumber(DateTime timestamp, EShiftDuration shiftDuration)
    {
        switch (shiftDuration)
        {
            case EShiftDuration.ShortShiftWithBreak:
            case EShiftDuration.ShortShiftWithoutBreak:
                return GetShortShiftNumber(timestamp);
            case EShiftDuration.LongShiftWithBreak:
            case EShiftDuration.LongShiftWithoutBreak:
                return GetLongShiftNumber(timestamp);
            default:
                throw new ArgumentOutOfRangeException(nameof(shiftDuration), shiftDuration, null);
        }
    }

    public static DateTime GetShiftDate(DateTime timestamp, EShiftDuration shiftDuration)
    {
        switch (shiftDuration)
        {
            case EShiftDuration.ShortShiftWithBreak:
            case EShiftDuration.ShortShiftWithoutBreak:
                return GetShortShiftDate(timestamp);
            case EShiftDuration.LongShiftWithBreak:
            case EShiftDuration.LongShiftWithoutBreak:
                return GetLongShiftDate(timestamp);
            default:
                throw new ArgumentOutOfRangeException(nameof(shiftDuration), shiftDuration, null);
        }
    }

    private static int GetShortShiftNumber(DateTime timestamp)
    {
        var shiftTime = timestamp.TimeOfDay;
        if (shiftTime > ShortShift1StartTime && shiftTime < ShortShift2StartTime)
        {
            return 1;
        }
        else if (shiftTime > ShortShift2StartTime && shiftTime < ShortShift3StartTime)
        {
            return 2;
        }
        else
        {
            return 3;
        }
    }

    private static DateTime GetShortShiftDate(DateTime timestamp)
    {
        var shiftNumber = GetShortShiftNumber(timestamp);
        if (shiftNumber == 3 && timestamp.TimeOfDay < ShortShift1StartTime)
        {
            return timestamp.Date.AddDays(-1);
        }
        else
        {
            return timestamp.Date;
        }
    }
    
    private static DateTime GetShortShiftStartTime(DateTime date, int shiftNumber)
    {
        if (shiftNumber == 1)
        {
            return date.Add(ShortShift1StartTime);
        }
        else if (shiftNumber == 2)
        {
            return date.Add(ShortShift2StartTime);
        }
        else
        {
            return date.Add(ShortShift3StartTime);
        }
    }

    private static TimeSpan GetShortShiftElapsedTime(DateTime date, int shiftNumber)
    {
        return GetVietnamTime() - GetShortShiftStartTime(date, shiftNumber);
    }

    private static int GetLongShiftNumber(DateTime timestamp)
    {
        var shiftTime = timestamp.TimeOfDay;
        if (shiftTime > LongShift1StartTime && shiftTime < LongShift2StartTime)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }

    private static DateTime GetLongShiftDate(DateTime timestamp)
    {
        var shiftNumber = GetLongShiftNumber(timestamp);
        if (shiftNumber == 2 && timestamp.TimeOfDay < LongShift1StartTime)
        {
            return timestamp.Date.AddDays(-1);
        }
        else
        {
            return timestamp.Date;
        }
    }

    private static DateTime GetLongShiftStartTime(DateTime date, int shiftNumber)
    {
        if (shiftNumber == 1)
        {
            return date.Add(LongShift1StartTime);
        }
        else
        {
            return date.Add(LongShift2StartTime);
        }
    }

    private static TimeSpan GetLongShiftElapsedTime(DateTime timestamp, int shiftNumber)
    {
        return GetVietnamTime() - GetLongShiftStartTime(timestamp, shiftNumber);
    }

    public static readonly ImmutableDictionary<EShiftDuration, TimeSpan> ShiftDurations = ImmutableDictionary.CreateRange(new Dictionary<EShiftDuration, TimeSpan>()
        {
            { EShiftDuration.ShortShiftWithBreak, TimeSpan.FromHours(7.25) },
            { EShiftDuration.ShortShiftWithoutBreak, TimeSpan.FromHours(7.5) },
            { EShiftDuration.LongShiftWithBreak, TimeSpan.FromHours(11.25) },
            { EShiftDuration.LongShiftWithoutBreak, TimeSpan.FromHours(11.5) },
        });

    public static TimeSpan GetShiftElapsedTime(DateTime time, EShiftDuration shiftDuration, int shiftNumber)
    {
        TimeSpan elapsedTime;
        if (shiftDuration == EShiftDuration.ShortShiftWithBreak || shiftDuration == EShiftDuration.ShortShiftWithoutBreak)
        {
            elapsedTime = ShiftTimeHelper.GetShortShiftElapsedTime(time, shiftNumber);
        }
        else
        {
            elapsedTime = ShiftTimeHelper.GetLongShiftElapsedTime(time, shiftNumber);
        }

        if (elapsedTime > ShiftTimeHelper.ShiftDurations[shiftDuration])
        {
            return ShiftTimeHelper.ShiftDurations[shiftDuration];
        }
        else
        {
            return elapsedTime;
        }
    }
}
