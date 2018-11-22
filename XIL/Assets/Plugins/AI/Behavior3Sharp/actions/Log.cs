
namespace XIL.AI.Behavior3Sharp
{
    public class Log : Action
    {
        private string info;

        public override void Initialize(Behavior3NodeCfg cfg)
        {
            base.Initialize(cfg);
            this.info = cfg.GetValue<string>("info", "log-action");
        }

        public override B3Status tick(Tick tick) { return B3Status.SUCCESS; }
    }
}
