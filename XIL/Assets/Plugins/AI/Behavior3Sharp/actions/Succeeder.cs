/**
 * This action node returns `SUCCESS` always.
 *
 * @module b3
 * @class Succeeder
 * @extends Action
 **/

namespace XIL.AI.Behavior3Sharp
{
    public class Succeeder : Action
    {
        public override B3Status tick(Tick tick) { return B3Status.SUCCESS; }
    }
}
