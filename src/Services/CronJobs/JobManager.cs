﻿using Common.Constants;
using Data.Models;
using Hangfire;
using System;

namespace Services.CronJobs
{
    public static class JobManager
    {
        private const string cronJobExpressionLength = "*/1 * * * *";

        public static void StartVideoUpdaterJob()
        {
           RecurringJob.AddOrUpdate<MakeVisibleVideosJob>(CronJobsNames.MakeVisibleVideoJobName, x => x.Work(null), cronJobExpressionLength);
        }

        public static void ScheduleVideoUpdaterJob(DateTime jobStartTime)
        {
            BackgroundJob.Schedule<MakeVisibleVideosJob>(x => x.Work(null),jobStartTime);
        }
        public static void TriggerVideoUpdaterJob()
        {
            StartVideoUpdaterJob();
            RecurringJob.Trigger(CronJobsNames.MakeVisibleVideoJobName);
        }

        public static void StopVideoUpdaterJob()
        {
            RecurringJob.RemoveIfExists(CronJobsNames.MakeVisibleVideoJobName);
        }

        public static void StartSubscriptionEmailJob(string htmlText)
        {
            BackgroundJob.Enqueue<SendSubscribersEmailJob>(x => x.Work(htmlText,null));
        }

        public static void StartSubscriptionEmailJob(Video video)
        {
            BackgroundJob.Enqueue<SendSubscribersEmailJob>(x => x.Work(video, null));
        }

        public static void StartContactLetterJob(ContactLetter letter)
        {
            BackgroundJob.Enqueue<SendAdminsContactEmailJob>(x => x.Work(letter, null));
        }
    }
}