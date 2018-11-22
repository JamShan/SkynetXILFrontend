/**
 * This action node returns `FAILURE` always.
 *
 * @module b3
 * @class Failer
 * @extends Action
 **/

namespace XIL.AI.Behavior3Sharp
{
    public class Failer : Action
    {
        public override B3Status tick(Tick tick) { return B3Status.FAILURE; }
    }
}
