namespace Book.ViewModels.Samples.Chapter08.Sample15
{
    public struct ProgressInfo
    {
        private static readonly string[] stageNames = new[]
        {
            "Extracting DNA...",
            "Sequencing Genome...",
            "Cloning..."
        };

        private readonly int stage;
        private readonly int totalStages;
        private readonly int percentComplete;
        private readonly int percentStageComplete;

        public ProgressInfo(
            int stage,
            int totalStages,
            int percentComplete,
            int percentStageComplete)
        {
            this.stage = stage;
            this.totalStages = totalStages;
            this.percentComplete = percentComplete;
            this.percentStageComplete = percentStageComplete;
        }

        public int Stage => this.stage;

        public string StageName
        {
            get
            {
                if (this.percentComplete == 100)
                {
                    return "Done!";
                }

                return stageNames[this.stage];
            }
        }

        public int TotalStages => this.totalStages;

        public int PercentComplete => this.percentComplete;

        public int PercentStageComplete => this.percentStageComplete;

        // just hide some ugly, irrelevant math
        internal ProgressInfo IncreaseProgress()
        {
            var stage = this.stage;
            var percentStageComplete = this.percentStageComplete + 1;

            if (percentStageComplete == 101)
            {
                percentStageComplete = 1;
                ++stage;
            }

            var percentComplete = (int)(((double)stage / this.totalStages * 100) + ((double)percentStageComplete / this.totalStages));

            return new ProgressInfo(
                stage,
                this.totalStages,
                percentComplete,
                percentStageComplete);
        }
    }
}