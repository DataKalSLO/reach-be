using System;
using System.Collections.Generic;
using System.Linq;
using HourglassServer.Custom.Exceptions;
using HourglassServer.Data;

namespace HourglassServer.Custom.Constraints
{
    public class ConstraintChecker <CONTENT_TYPE>
        where CONTENT_TYPE : class
    {
        /*
         * This delegate allows us to defined a type for a function. This functions,
         * takes a ConstraintEnvironment and some class (e.g. Story, Graph)
         * and performs any checks it needs. It return true if the constraint
         * is satisfied, false otherwise
         */
        public delegate bool Constraint(ConstraintEnvironment environment, CONTENT_TYPE entity);

        protected ConstraintEnvironment environment;
        protected CONTENT_TYPE entity;

        protected Dictionary<Constraints, Constraint> Constraints { get; set; }
        protected Dictionary<Constraints, (string message, string tag)> ConstraintErrors { get; set; }

        private const string NoConstraintsGivenError = "Expected at least one constraint.";

        public ConstraintChecker(ConstraintEnvironment environment, CONTENT_TYPE entity)
        {
            this.environment = environment;
            this.entity = entity;
            Constraints = new Dictionary<Constraints, Constraint>();
            ConstraintErrors = new Dictionary<Constraints, (string message, string tag)>();
            CreatePermissions();
        }

        protected virtual void CreatePermissions()
        {
            Constraints.Add(Custom.Constraints.Constraints.HAS_USER_ACCOUNT,
                (env, newStory) => env.context.Person.Any(p => p.Email == env.user.GetUserId()));
            ConstraintErrors.Add(Custom.Constraints.Constraints.HAS_USER_ACCOUNT, (
                "Account required for action.", ErrorTag.ForbiddenRole));

            Constraints.Add(Custom.Constraints.Constraints.HAS_ADMIN_ACCOUNT,
                (env, newStory) => env.user.HasRole(Role.Admin));
            ConstraintErrors.Add(Custom.Constraints.Constraints.HAS_ADMIN_ACCOUNT,
                ("Administrator account required for action.", ErrorTag.ForbiddenRole));
        }

        /*
         * Assert Methods - Nothing is constraint is satisfied, throw error otherwise
         */

        public void AssertAtLeastOneContstraint(Constraints[] constraints)
        {
            if (!SatisfiesAtLeastOneConstraint(constraints))
                ThrowPermissionException(constraints[0]);
        }

        public void AssertAllConstraints(Constraints[] constraints)
        {
            (bool success, int failingIndex) = SatisfiesAllConstraints(constraints);
            if (!success)
                ThrowPermissionException(constraints[failingIndex]);
        }

        public void AssertConstraint(Constraints action)
        {
            if (!this.SatisfiesConstraint(action))
                ThrowPermissionException(action);
        }

        public void ThrowPermissionException(Constraints action)
        {
            throw new HourglassError(ConstraintErrors[action].message, ConstraintErrors[action].tag);
        }

        /*
         * Satisfies Methods - Return True if constraint satisfied, false otherwise
         */

        public bool SatisfiesAtLeastOneConstraint(Constraints[] constraints)
        {
            if (constraints.Length == 0)
                throw new InvalidOperationException(NoConstraintsGivenError);
            foreach (Constraints constraintName in constraints)
            {
                if (this.SatisfiesConstraint(constraintName))
                    return true;
            }
            return false;
        }

        //indexFailed will index of failed constraint , otherwise -1
        public (bool success, int indexFailed) SatisfiesAllConstraints(Constraints[] constraints)
        {
            if (constraints.Length == 0)
                throw new InvalidOperationException(NoConstraintsGivenError);
            for (int i=0; i<constraints.Length; i++)
            {
                Constraints constraintName = constraints[i];
                if (!this.SatisfiesConstraint(constraintName))
                    return (false, i);
            }
            return (true, -1);
        }

        public bool SatisfiesConstraint(Constraints action)
        {
            return Constraints[action](this.environment, this.entity);
        }
    }
}
