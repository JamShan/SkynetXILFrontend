/**
 * This action node returns `ERROR` always.
 *
 * @module b3
 * @class Error
 * @extends Action
 **/

namespace XIL.AI.Behavior3Sharp
{
    public class Error : Action
    {
        public override B3Status tick(Tick tick) { return B3Status.ERROR; }
    }
}
