using System;
using System.Collections.Generic;
/**
 *----------------------------------------
 * DataSets Model Object
 *----------------------------------------
 * This class will be used for creating DataSets Objects
 * These will be used for formatting data from the database
 *----------------------------------------
 */
namespace HourglassServer.Data{
    public class DataSet{
        public List<Object[]> Data {get; set;}
    }
}
