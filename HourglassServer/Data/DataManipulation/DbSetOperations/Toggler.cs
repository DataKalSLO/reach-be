using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HourglassServer.Data.DataManipulation.DbSetOperations
{
    public class Toggler
    {
        public static ToggleState ToggleEntity<T>(DbSet<T> dbSet, T potentialEntity)
            where T : class
        {
            bool bookmarkIsEnabled;
            if (dbSet.Any(existingEntity => existingEntity == potentialEntity))
            {
                DbSetMutator.PerformOperationOnDbSet<T>(dbSet, MutatorOperations.DELETE, potentialEntity);
                bookmarkIsEnabled = false;
            }
            else
            {
                DbSetMutator.PerformOperationOnDbSet<T>(dbSet, MutatorOperations.ADD, potentialEntity);
                bookmarkIsEnabled = true;
            }
            return new ToggleState(bookmarkIsEnabled);
        }
    }

    public class ToggleState
    {
        public ToggleState(bool enabled)
        {
            this.Enabled = enabled;
        }

        public bool Enabled { get; set; }
    }
}
