using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data
{
    public class QueryFormatUtil
    {
        private string fromTable;
        private string selectColumns;
        private string error;
        private string invalidColumn;

        public static string MetadataQuery = "SELECT * FROM dataset_meta_data";

        // Returns the resulting formatted query after a format operation
        public string getQuery()
        {
            if (fromTable == null || selectColumns == null)
            {
                throw new InvalidOperationException("No succesful query format method invoked");
            }
            return selectColumns + fromTable;
        }

        // Returns the error that caused the formatted query to fail
        public string Error { get => error; }
        public string BadColumn {get => invalidColumn;}

        // Helper Method used for formatting a sql select query for an entire dataset provided a table name
        //
        // Returns true if the query formatting is succesful, false otherwise
        // Use the Query and Error properties for results of the operation
        public bool formatTableQuery(string tableName, List<DatasetMetaData> metadata)
        {
            // Check if the table name exists in the cache (in order to prevent SQL injection)
            bool tableExists = metadata.Exists(x => x.TableName == tableName);

            if (!tableExists)
            {
                error = String.Format("Table does not exist: {0}", tableName);
            }
            else
            {
                fromTable = String.Format("FROM datasets.{0}", tableName);
            }

            return tableExists;
        }
          private string formatArray(int length){
            string arrayString = "SELECT ";
            for (int i = 0; i < length - 1; i++){
                arrayString = arrayString + "{" + i.ToString() + "}, ";
            }
            arrayString = arrayString + "{" + (length - 1).ToString() + "} ";
            return arrayString;
        }
        private void formatColumnsQuery(string[] columns){
            string columnArray = formatArray(columns.Length);
            selectColumns = String.Format(columnArray, columns);
        }

        private bool containsColumn(string column, string[] columnNames){
             foreach (string col in columnNames){
                 if (col.Equals(column)){
                     return true;
                 }
             }
             return false;
         }  

        public bool createQuery(DatasetMetaData table, string[] columns){
            foreach (string col in columns) {
                if(!containsColumn(col, table.ColumnNames)){
                    invalidColumn = col;
                    error = String.Format("Table {0} does not contain column : {1}", table.TableName, col);
                    return false;
                }
            }
            formatColumnsQuery(columns);
            return true;
        }
    }
}
