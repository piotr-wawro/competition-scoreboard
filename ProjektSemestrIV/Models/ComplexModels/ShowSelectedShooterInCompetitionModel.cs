﻿using ProjektSemestrIV.DAL.Entities;
using ProjektSemestrIV.DAL.Repositories;
using ProjektSemestrIV.Extensions;
using ProjektSemestrIV.Models.ShowModels;
using System.Collections.ObjectModel;
using System.Linq;

namespace ProjektSemestrIV.Models.ComplexModels
{
    class ShowSelectedShooterInCompetitionModel
    {
        Shooter shooter;
        Competition competition;

        public ShowSelectedShooterInCompetitionModel(uint shooterId, uint competitionId)
        {
            shooter = ShooterRepository.GetShooterFromDB(shooterId);
            competition = CompetitionRepository.GetCompetitionFromDB(competitionId);
        }

        public string GetShooterName()
        => shooter.Name + " " + shooter.Surname;

        public string GetCompetitionName()
        => competition.Location + ", " + competition.StartDate.Substring(0, 10) 
                + " - " + competition.EndDate.Substring(0, 10);

        public double GetPoints()
        => ShooterRepository.GetShooterSumOfPointsAtCompetitionFromDB(shooter.ID, competition.Id);

        public double GetTime()
        => ShooterRepository.GetShooterSumOfTimesAtCompetitionFromDB(shooter.ID, competition.Id);

        public uint GetPosition()
        => ShooterRepository.GetShooterPositionAtCompetitionFromDB(shooter.ID, competition.Id);

        public double GetGeneralAccuracy()
        => ShooterRepository.GetShooterGeneralAccuracyAtCompetitionFromDB(shooter.ID, competition.Id);

        public double GetAlphaAccuracy()
        => ShooterRepository.GetShooterAlphaAccuracyAtCompetitionFromDB(shooter.ID, competition.Id);

        public double GetCharlieAccuracy()
        => ShooterRepository.GetShooterCharlieAccuracyAtCompetitionFromDB(shooter.ID, competition.Id);

        public double GetDeltaAccuracy()
        => ShooterRepository.GetShooterDeltaAccuracyAtCompetitionFromDB(shooter.ID, competition.Id);

        public ObservableCollection<StatsAtStageOverview> GetShooterStatsOnStages()
        => ShooterRepository.GetShooterStatsOnStages(shooter.ID, competition.Id)
                            .Select(x => new StatsAtStageOverview(x.StageName, x.Points, x.Time, x.StagePoints))
                            .Convert();


    }
}
