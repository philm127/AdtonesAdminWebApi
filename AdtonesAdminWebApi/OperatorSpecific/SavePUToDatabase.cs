
using AdtonesAdminWebApi.DAL.Interfaces;
using AdtonesAdminWebApi.Services;
using AdtonesAdminWebApi.ViewModels;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AdtonesAdminWebApi.OperatorSpecific
{
    
    public interface ISavePUToDatabase
    {
        Task<bool> DoSaveToDatabase<T>(IEnumerable<T> source, Func<IEnumerable<T>, DataTable, List<DataRow>> rowConverter, int operatorId);
    }
    public class SavePUToDatabase : ISavePUToDatabase
    {

        ReturnResult result = new ReturnResult();
        private readonly IConnectionStringService _connService;

        public SavePUToDatabase(IConnectionStringService connService)
        {
            _connService = connService;
        }

        public async Task<bool> DoSaveToDatabase<T>(IEnumerable<T> source, Func<IEnumerable<T>, DataTable, List<DataRow>> rowConverter, int operatorId)
        {
            
            var operatorConnectionString = await _connService.GetConnectionStringByOperator(operatorId);
            try
            {
                // creating inmemory table
                DataTable table = new DataTable("PromotionalUsers");
                table.Columns.Add("ID", typeof(int));
                table.Columns.Add("MSISDN", typeof(string));
                table.Columns.Add("BatchID", typeof(int));
                table.Columns.Add("WeeklyPlay", typeof(int));
                table.Columns.Add("DailyPlay", typeof(int));
                table.Columns.Add("Status", typeof(int));
                table.BeginInit();

                // converting source items to DataRow instances via rowConverter.
                List<DataRow> promotionalRecords = rowConverter(source, table);

                // adding all converted DataRows to inmemory table.
                promotionalRecords.ForEach(table.Rows.Add);

                table.EndInit();

                // bulk insert.
                using (SqlBulkCopy copy = new SqlBulkCopy(operatorConnectionString))
                {
                    copy.BatchSize = 10000;
                    copy.DestinationTableName = "dbo.PromotionalUsers";
                    await copy.WriteToServerAsync(table, DataRowState.Added);
                }
                return true;
            }
            catch (Exception ex)
            {
                var _logging = new ErrorLogging()
                {
                    ErrorMessage = ex.Message.ToString(),
                    StackTrace = ex.StackTrace.ToString(),
                    PageName = "SavePUToDatabase For Operator " + operatorId.ToString(),
                    ProcedureName = "DoSaveToDatabase"
                };
                _logging.LogError();
                throw;
            }
        }

    }
}
