﻿using System;
using System.Collections.Generic;
using System.Linq;
using Membership.Core.DataModels;
using Membership.Core.Repositories;

namespace Membership.Core.Presenters
{
    public class OfficersByYearPresenter : IOfficersByYearView
    {
        public IEnumerable<Officer> Commissioners { get; set; }
        public IEnumerable<Officer> LineOfficers { get; set; }
        public IEnumerable<Officer> OtherDistrictOfficers { get; set; }
        public IEnumerable<Officer> BoardOfDirectors { get; set; }
        public IEnumerable<Officer> TableOfficers { get; set; }
        public IEnumerable<Officer> OtherAssocOfficers { get; set; }

        private readonly IOfficerRepository _officeRepository;
        private readonly IOfficersByYearView _view;

        public OfficersByYearPresenter(IOfficersByYearView view)
        {
            _officeRepository = new OfficerRepository();
            _view = view;
        }

        public void LoadOfficersForaYear(int year)
        {
            var allRecs = _officeRepository.GetOfficersByYear(year).ToList();

            _view.Commissioners = allRecs.Where(off => off.OfficeRec.GroupId == 11);
            _view.LineOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 12).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.OtherDistrictOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 13).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.BoardOfDirectors = allRecs.Where(off => off.OfficeRec.GroupId == 21);
            _view.TableOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 22).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
            _view.OtherAssocOfficers = allRecs.Where(off => off.OfficeRec.GroupId == 23).OrderBy(off => off.OfficeId).ThenBy(dtef => dtef.FromDate).ThenByDescending(dtet => dtet.ToDate);
        }

        public IEnumerable<int> LoadYearsOnFile()
        {
            return _officeRepository.GetOfficerYearsOnFile();
        }


    }
}
