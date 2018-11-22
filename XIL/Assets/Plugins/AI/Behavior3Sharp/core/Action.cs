/**
 * Action is the base class for all action nodes. Thus, if you want to create
 * new custom action nodes, you need to inherit from this class. For example,
 * take a look at the Runner action:
 *
 *     class Runner extends b3.Action {
 *       constructor(){
 *         super({name: 'Runner'});
 *       }
 *       tick(tick) {
 *         return b3.RUNNING;
 *       }
 *     };
 *
 * @module b3
 * @class Action
 * @extends BaseNode
 **/

using System.Collections.Generic;

namespace XIL.AI.Behavior3Sharp
{
    public class Action: BaseNode
    {
        public Action()
        {
            this.category = Constants.ACTION;
        }

        public override void Initialize(Behavior3NodeCfg cfg)
        {
            base.Initialize(cfg);
            this.parameters = new Dictionary<string, object>();
	        this.properties = new Dictionary<string, object>();
        }

    }
}
