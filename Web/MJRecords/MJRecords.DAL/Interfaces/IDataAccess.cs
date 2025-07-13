using MJRecords.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public interface IDataAccess
    {
        DataTable Execute(string cmdText, List<Parm>? parms = null, CommandType cmdType = CommandType.StoredProcedure);
        Task<DataTable> ExecuteAsync (string cmdText, List<Parm>? parms = null, CommandType cmdType = CommandType.StoredProcedure);

        object ExecuteScalar(string cmdText, List<Parm>? parms = null, CommandType cmdType = CommandType.StoredProcedure);
        Task<object?> ExecuteScalarAsync(string cmdText, List<Parm>? parms = null, CommandType cmdType = CommandType.StoredProcedure);

        int ExecuteNonQuery(string cmdText, List<Parm>? parms = null, CommandType cmdType = CommandType.StoredProcedure);
        Task<int> ExecuteNonQueryAsync(string cmdText, List<Parm>? parms = null, CommandType cmdType = CommandType.StoredProcedure);
    }
}
