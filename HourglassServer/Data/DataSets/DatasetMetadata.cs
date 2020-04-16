using System;
using System.Collections.Generic;
/**
 *----------------------------------------
 * MetaData Model object
 *----------------------------------------
 * This class will be used for creating MetaData Objects
 * These will be used for formatting data from the database
 * MetaData provides information about data stored in the database
 * This includes table names column names and column types  
 *----------------------------------------
 */

namespace HourglassServer.Data{
    public class DatasetMetadata{
        public string TableName {get; set;}
        public string[] ColumnNames{get; set;}
        public string[] ColumnTypes{get; set;}      
    }
}
