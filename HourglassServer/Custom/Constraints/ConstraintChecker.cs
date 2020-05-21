using System;
using System.Collections.Generic;
using HourglassServer.Data;

namespace HourglassServer.Custom.Constraints
{
    public abstract class ConstraintChecker <CONSTRAINT_NAME, ENV, CONTENT_TYPE> where CONSTRAINT_NAME: Enum where ENV : ConstraintEnvironment where CONTENT_TYPE : class
    {
        /*
         * This delegate allows us to defined a type for a function. This functions,
         * takes a ConstraintEnvironment and some class (e.g. Story, Graph)
         * and performs any checks it needs. It return true if the constraint
         * is satisfied, false otherwise
         */
        public delegate bool Constraint(ENV environment, CONTENT_TYPE entity);

        protected ENV environment;
        protected CONTENT_TYPE entity;

        protected Dictionary<CONSTRAINT_NAME, Constraint> constraints { get; set; }
        protected Dictionary<CONSTRAINT_NAME, (string message, string tag)> constraintErrors { get; set; }

        private const string NoConstraintsGivenError = "Expected at least one constraint.";

        public ConstraintChecker(ENV environment, CONTENT_TYPE entity)
        {
            this.environment = environment;
            this.entity = entity;
            CreatePermissions();
        }

        protected abstract void CreatePermissions(); // will set the contraint/error dict

        /*
         * Assert Methods - Nothing is constraint is satisfied, throw error otherwise
         */

        public void AssertAtLeastOneContstraint(CONSTRAINT_NAME[] constraints)
        {
            if (!SatisfiesAtLeastOneConstraint(constraints))
                ThrowPermissionException(constraints[0]);
        }

        public void AssertAllConstraints(CONSTRAINT_NAME[] constraints)
        {
            (bool success, int failingIndex) = SatisfiesAllConstraints(constraints);
            if (!success)
                ThrowPermissionException(constraints[failingIndex]);
        }

        public void AssertConstraint(CONSTRAINT_NAME action)
        {
            if (!this.SatisfiesConstraint(action))
                ThrowPermissionException(action);
        }

        public void ThrowPermissionException(CONSTRAINT_NAME action)
        {
            throw new HourglassError(constraintErrors[action].message, constraintErrors[action].tag);
        }

        /*
         * Satisfies Methods - Return True if constraint satisfied, false otherwise
         */

        public bool SatisfiesAtLeastOneConstraint(CONSTRAINT_NAME[] constraints)
        {
            if (constraints.Length == 0)
                throw new InvalidOperationException(NoConstraintsGivenError);
            foreach (CONSTRAINT_NAME constraintName in constraints)
            {
                if (this.SatisfiesConstraint(constraintName))
                    return true;
            }
            return false;
        }

        //indexFailed will index of failed constraint , otherwise -1
        public (bool success, int indexFailed) SatisfiesAllConstraints(CONSTRAINT_NAME[] constraints)
        {
            if (constraints.Length == 0)
                throw new InvalidOperationException(NoConstraintsGivenError);
            for (int i=0; i<constraints.Length; i++)
            {
                CONSTRAINT_NAME constraintName = constraints[i];
                if (!this.SatisfiesConstraint(constraintName))
                    return (false, i);
            }
            return (true, -1);
        }

        public bool SatisfiesConstraint(CONSTRAINT_NAME action)
        {
            return constraints[action](this.environment, this.entity);
        }
    }
}
