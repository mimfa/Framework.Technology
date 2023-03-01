using System;
using System.Linq;
using System.Data;
using System.Collections.Generic;
using MiMFa.Model;
using System.Windows.Forms;
using System.Data.OleDb;
using MiMFa.General;
using MiMFa.Service;
using MiMFa.Model.IO;

namespace MiMFa.Exclusive.Technology.Excel
{
    public class Convert : Exclusive.Technology.Convert , IConvert
    {
        public Convert(bool clearTemporary = true) : base(clearTemporary)
        {
        }


        public override ChainedFile ToChainedFile(string path)
        {
            return new ChainedFile(ToRows(path));
        }
        public override void ToFile(string exceladdress, string table)
        {
            ToFile(exceladdress, ConvertService.ToDataTable(table),true);
        }
        public override void ToFile(string exceladdress, IEnumerable<string> tables)
        {
            foreach (var item in tables)
                ToFile(exceladdress, ConvertService.ToDataTable(item), true);
        }
        public override string ToText(string exceladdress)
        {
            return ConvertService.ToString(ToDataTable(exceladdress, ""));
        }
        public override IEnumerable<string> ToLines(string exceladdress)
        {
            return ConvertService.ToStrings(exceladdress, "");
        }
        public override IEnumerable<IEnumerable<string>> ToRows(string exceladdress)
        {
            return ConvertService.ToStringSquareMatrix(exceladdress, "");
        }
        public override string ToString(string exceladdress)
        {
            return ConvertService.ToString(ToDataTable(exceladdress, ""));
        }
        public override IEnumerable<string> ToStrings(string exceladdress)
        {
            return ConvertService.ToStrings(exceladdress, "");
        }

        public virtual void ToFile( string exceladdress, DataTable dt, bool openAfter = true)
        {
             ConvertService.ToExcelFile( dt, exceladdress, dt.TableName, openAfter);
        }
        public virtual void ToFile( string exceladdress, DataTable dt, string sheetName = "sheet1", bool openAfter = true)
        {
             ConvertService.ToExcelFile( dt, exceladdress, sheetName, openAfter);
        }
        public virtual void ToFile( string exceladdress, DataGridView dgv, string sheetName = "sheet1", bool openAfter = true)
        {
            ConvertService.ToExcelFile(dgv, exceladdress, sheetName, openAfter);
        }
        public virtual void ToFile( string exceladdress, SmartTable dt, string sheetName = "sheet1", bool openAfter = true)
        {
            ConvertService.ToExcelFile(dt, exceladdress, sheetName, openAfter);
        }
        public virtual void ToFile<T, F>( string exceladdress, Dictionary<T, F> dic, string sheetName = "sheet1", bool openAfter = true)
        {
            ConvertService.ToExcelFile(dic, exceladdress, sheetName, openAfter);
        }
        public virtual void ToFile<T, F>( string exceladdress, IEnumerable<KeyValuePair<T, F>> dic, string sheetName = "sheet1", bool openAfter = true)
        {
            ConvertService.ToExcelFile(dic, exceladdress, sheetName, openAfter);
        }
        public virtual void ToFile(string exceladdress, ChainedFile lt, string sheetName = "sheet", bool openAfter = false)
        {
            ConvertService.ToExcelFile(lt, exceladdress, sheetName, openAfter);
        }
        public virtual void ToFile<T>(string exceladdress, IEnumerable<T> lt, string sheetName = "sheet1", bool openAfter = true, bool showProcess = false)
        {
            ConvertService.ToExcelFile(lt, exceladdress, sheetName, openAfter);
        }
        public virtual DataTable ToDataTable(string exceladdress, string sheetName, string condition = "")
        {
            DataSet ds = ToDataSet(exceladdress, new List<string> { sheetName }, condition);
            if (ds != null && ds.Tables.Count > 0) return ds.Tables[0];
            return null;
        }
        public virtual DataTable ToDataTable(string exceladdress, string condition = "")
        {
            var dts = ToDataTables(exceladdress, condition);
            if (dts.Any()) return dts.First();
            return null;
        }
        public virtual string ToString(string exceladdress, string sheetName, string condition = "")
        {
            return ConvertService.ToString(ToDataTable(exceladdress, sheetName, condition));
        }
        public virtual string ToString(string exceladdress, string condition = "")
        {
            return ConvertService.ToString(ToDataTable(exceladdress, condition));
        }
        public virtual IEnumerable<DataTable> ToDataTables(string exceladdress, string condition = "")
        {
            return ConvertService.ToDataTables(exceladdress, condition);
        }
        public virtual IEnumerable<DataRow> ToDataRows(string exceladdress, string condition = "")
        {
            foreach (DataTable table in ConvertService.ToDataTables(exceladdress, condition))
                foreach (DataRow dr in table.Rows)
                    yield return dr;
        }
        public virtual IEnumerable<string> ToStrings(string exceladdress, string condition = "")
        {
            return ConvertService.ToStrings(exceladdress, condition);
        }
        public virtual DataSet ToDataSet(string exceladdress, string sheetName, string condition = "")
        {
            return ToDataSet(exceladdress, new List<string> { sheetName }, condition);
        }
        public virtual DataSet ToDataSet(string exceladdress, string condition = "")
        {
            return ToDataSet(exceladdress, 0, -1, condition);
        }
        public virtual DataSet ToDataSet(string exceladdress, int startRecord = 0, int maxRecord = -1, string condition = "")
        {
            return ConvertService.ToDataSet(exceladdress,startRecord ,   maxRecord,  condition); 
        }
        public virtual DataSet ToDataSet(string exceladdress, IEnumerable<string> sheetNames, string condition = "")
        {
            return ConvertService.ToDataSet(exceladdress, sheetNames, condition);
        }
        public virtual SmartTable ToSmartTable(string exceladdress, string sheetName, string condition = "")
        {
            DataTable dt = ToDataTable(exceladdress, sheetName, condition);
            if (dt == null) return null;
            return new SmartTable(dt);
        }
        public virtual IEnumerable<SmartTable> ToSmartTables(string exceladdress, int startRecord = 0, int maxRecord = -1, string condition = "")
        {
            return ConvertService.ToMiMFaTables(exceladdress, startRecord, maxRecord, condition);
        }
        public virtual IEnumerable<string> ToSheets(string exceladdress)
        {
            return ConvertService.ToSheets(exceladdress );
        }
    }
}
