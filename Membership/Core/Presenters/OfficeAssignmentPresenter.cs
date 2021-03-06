﻿using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class OfficeAssignmentPresenter : IOfficeAssignmentView
    {
        public IEnumerable<Officer> Commissioners { get; set; }
        public IEnumerable<Officer> LineOfficers { get; set; }
        public IEnumerable<Officer> OtherDistrictOfficers { get; set; }
        public IEnumerable<Officer> BoardOfDirectors { get; set; }
        public IEnumerable<Officer> TableOfficers { get; set; }
        public IEnumerable<Officer> OtherAssocOfficers { get; set; }

        private readonly IOfficerRepository _officerRepository;
        private readonly IOfficeAssignmentView _view;

        public OfficeAssignmentPresenter(IOfficeAssignmentView view)
        {
            _officerRepository = new OfficerRepository();
            _view = view;
        }

        public void LoadOfficersForaYear(int year)
        {
            var allRecs = _officerRepository.GetOfficersByYear(year).ToList();

            _view.Commissioners = allRecs.Where(off => off.OfficeRec.GroupId == 1);
            _view.LineOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 2).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.OtherDistrictOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 3).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.BoardOfDirectors = allRecs.Where(off => off.OfficeRec.GroupId == 4);
            _view.TableOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 5).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.OtherAssocOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 6).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _officerRepository.GetOfficerYearsOnFile();
        }

        public IEnumerable<Tuple<int, Office>> LoadOfficeRecords(bool currentOfficesOnly)
        {
            int tempId = 0;
            int idx = 0;
            var retVal = new List<Tuple<int, Office>>();
            var offices = _officerRepository.GetOfficesOnFile().OrderBy(x => x.GroupOrder).OrderBy(x => x.GroupId);
            foreach (var offc in offices)
            {
                var x = $"{offc.GroupId} - {offc.GroupOrder} - {offc.Title}";
                if (currentOfficesOnly && offc.CurrentlyUsed == 0) continue;
                retVal.Add(new Tuple<int, Office>(idx++, offc));
            }

            return retVal;

        }
    }
}
