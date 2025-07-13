using System.ComponentModel;
using System.Data;

namespace MJRecords.Types
{
    public class Parm
    {
        public string? Name { get; set; }
        public SqlDbType DataType { get; set; }
        public object? Value { get; set; }
        public int Size { get; set; }
        public ParameterDirection Direction { get; set; }

        public Parm(string? name, SqlDbType dataType, object? value, int size = 0, ParameterDirection direction = ParameterDirection.Input)
        {
            Name = name;
            DataType = dataType;
            Value = value;
            Size = size;
            Direction = direction;
        }
    }

    public enum AccessLevels
    {
        [Description("CEO")]
        CEO,

        [Description("HR Supervisor")]
        HRSupervisor,

        [Description("Regular Supervisor")]
        RegularSupervisor,

        [Description("HR Employee")]
        HREmployee,

        [Description("Regular Employee")]
        RegularEmployee,
    }

    public enum EmploymentStatusEnum
    {
        [Description("Active")]
        Active,

        [Description("Retired")]
        Retired,

        [Description("Terminated")]
        Terminated,
    }

    public enum RatingOptionsEnum
    {
        [Description("Exceeds Expectations")]
        ExceedsExcpectations,

        [Description("Meets Expectations")]
        MeetsExcpectations,

        [Description("Below Expectations")]
        BelowExcpectations,
    }

    public enum ErrorType
    {
        Model,
        Business
    }
}