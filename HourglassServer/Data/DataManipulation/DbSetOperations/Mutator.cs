using System;
using Microsoft.EntityFrameworkCore;

namespace HourglassServer.Data.DataManipulation.DbSetOperations
{
    public enum MutatorOperations { UPDATE, ADD, DELETE };

    public class DbSetMutator
    {
        public static void PerformOperationOnDbSet<T>(DbSet<T> dbSet, MutatorOperations operation, T model) where T : class
        {
            switch (operation)
            {
                case MutatorOperations.UPDATE:
                    dbSet.Update(model);
                    break;
                case MutatorOperations.ADD:
                    dbSet.Add(model);
                    break;
                case MutatorOperations.DELETE:
                    dbSet.Remove(model);
                    break;
                default:
                    throw new ArgumentException("Could not recognize operation: " + operation.ToString());
            }
        }
    }
}
