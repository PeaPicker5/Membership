﻿using System;
using System.Collections.Generic;
using Membership.Core.Officers.DataModels;

namespace Membership.Core.Officers.Repositories
{
    public interface IOfficerRepository
    {
        IEnumerable<Office> GetAssignableOffices();

        /// <summary>
        /// Gets the Office records assigned to a given member.
        /// </summary>
        /// <param name="memberId">The event identifier.</param>
        /// <returns></returns>
        IEnumerable<Officer> GetOfficersByMember(Guid memberId);

        /// <summary>
        /// Gets all Officers for a given year.
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        IEnumerable<Officer> GetOfficersByYear(int year);

        IEnumerable<Officer> GetOfficersByTitle(int officeId);
        IEnumerable<int> GetOfficerYearsOnFile();
        IEnumerable<Office> GetOfficesOnFile();
        IEnumerable<Office> GetOfficesByYear(int year);

        void InsertOfficerRecord(Officer officerRec);
        void UpdateOfficerRecord(Officer officerRec);
        void DeleteOfficerRecord(Officer officerRec);
    }
}
