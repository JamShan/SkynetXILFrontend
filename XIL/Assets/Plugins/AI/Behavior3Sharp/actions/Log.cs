
namespace XIL.AI.Behavior3Sharp
{
    public class Log : Action
    {
        private string info;

        public override void Initialize(Behavior3NodeCfg cfg)
        {
            base.Initialize(cfg);
            this.info = cfg.GetValue<string>("info", "log-action");
            this.name = "LogAction";
            this.title = "LogAction";
        }

        public override B3Status tick(Tick tick) { return B3Status.SUCCESS; }
    }
}
