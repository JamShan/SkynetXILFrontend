
/**
 * This action node returns RUNNING always.
 *
 * @module b3
 * @class Runner
 * @extends Action
 **/


namespace XIL.AI.Behavior3Sharp
{
    public class Runner: Action
    {
        public override B3Status tick(Tick tick) { return B3Status.RUNNING; }
    }
}
