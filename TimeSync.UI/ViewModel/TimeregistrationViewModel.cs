﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Castle.Core;
using TimeSync.DataAccess;
using TimeSync.IoC;
using TimeSync.Model;

namespace TimeSync.UI.ViewModel
{
    [Interceptor(typeof(ExceptionInterceptor))]
    public class TimeregistrationViewModel : BaseViewModel
    {
        private ObservableCollection<Timeregistration> _timeregistrations;
        private IRepository<List<Toolkit>> _toolkitRepo;
        public TimeManager TimeManager { get; set; }
        public ObservableCollection<Timeregistration> Timeregistrations {
            get { return _timeregistrations; }
            set
            {
                _timeregistrations = value;
                RaisePropertyChangedEvent("Timeregistrations");
            } 
        }
        public List<Toolkit> ListOfToolkits { get; set; }
        public List<string> ListOfToolkitNames { get; set; }
        public List<string> ListOfTeams { get; set; }

        public TimeregistrationViewModel()
        {
            IRepository<ObservableCollection<Timeregistration>> timeregRepo = new Repository<ObservableCollection<Timeregistration>>("TimeregistrationSaveLocation");
            Timeregistrations = new ObservableCollection<Timeregistration>();
            Timeregistrations = timeregRepo.GetData();

            _toolkitRepo = new Repository<List<Toolkit>>("ToolkitSaveLocation");
            ListOfToolkits = _toolkitRepo.GetData();
            ListOfToolkitNames = (from tk in ListOfToolkits select tk.DisplayName).ToList();
        }

        public ICommand SynchroniseCommand => new DelegateCommand(Synchronise);

        public virtual void Synchronise()
        {
            TimeManager.Sync(Timeregistrations.ToList());
        }

        public ICommand AddNewTimeregistrationCommand => new DelegateCommand(AddNewTimeregistration);

        public virtual void AddNewTimeregistration()
        {
            UpdateLists();
            Timeregistrations.Add(new Timeregistration
            {
                ListOfToolkitNames = ListOfToolkitNames,
                ListOfToolkits = ListOfToolkits,
                ListOfTeams = new ObservableCollection<string>(),
                ListOfTimeslots = new ObservableCollection<string>()
            });
        }

        public ICommand DeleteSelectedTimeregsCommand => new DelegateCommand(DeleteSelectedTimeregs);

        public virtual void DeleteSelectedTimeregs()
        {
            Timeregistrations = new ObservableCollection<Timeregistration>(Timeregistrations.Where(x => !x.ToBeDeleted));
        }

        private void UpdateLists()
        {
            ListOfToolkits = _toolkitRepo.GetData();
            ListOfToolkitNames = (from tk in ListOfToolkits select tk.DisplayName).ToList();
        }
    }
}
