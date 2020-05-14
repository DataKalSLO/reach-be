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
    public class storedGraph{
        public int Id {get; set;}
        public string Category {get; set;}
        public object Chart {get; set;}
    }
}