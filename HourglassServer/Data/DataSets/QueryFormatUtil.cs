using System;
using System.Collections.Generic;
using HourglassServer.Models.Persistent;

namespace HourglassServer.Data
{
    public class QueryFormatUtil
    {
        private string result;
        private string error;

        public static string MetadataQuery = "SELECT * FROM dataset_meta_data";

        // Returns the resulting formatted query after a format operation
        public string getQuery()
        {
            if (result == null)
            {
                throw new InvalidOperationException("No succesful query format method invoked");
            }
            return result;
        }

        // Returns the error that caused the formatted query to fail
        public string Error { get => error; }

        // Helper Method used for formatting a sql select query for an entire dataset provided a table name
        //
        // Returns true if the query formatting is succesful, false otherwise
        // Use the Query and Error properties for results of the operation
        public bool formatSelectFullDatasetQuery(string tableName, List<DatasetMetaData> metadata)
        {
            // Check if the table name exists in the cache (in order to prevent SQL injection)
            bool tableExists = metadata.Exists(x => x.TableName == tableName);

            if (!tableExists)
            {
                error = String.Format("Table does not exist: {0}", tableName);
            }
            else
            {
                result = String.Format("SELECT * FROM datasets.{0}", tableName);
            }

            return tableExists;
        }
    }
}
