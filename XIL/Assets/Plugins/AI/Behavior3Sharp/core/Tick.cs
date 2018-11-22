using System;
using System.Collections.Generic;


namespace XIL.AI.Behavior3Sharp
{
    public class Tick
    {
        /**
        * The tree reference.
        * @property {b3.BehaviorTree} tree
        * @readOnly
        **/
        public BehaviorTree tree = null;
        /**
         * The debug reference.
         * @property {Object} debug
         * @readOnly
         */
        public object debug = null;

        /**
        * The target object reference.
        * @property {Object} target
        * @readOnly
        **/
        public object target = null;


        /**
         * The blackboard reference.
         * @property {b3.Blackboard} blackboard
         * @readOnly
         **/
        public Blackboard blackboard = null;

        /**
        * The list of open nodes. Update during the tree traversal.
        * @property {Array} _openNodes
        * @protected
        * @readOnly
        **/
        public List<BaseNode> _openNodes;

        /**
         * The number of nodes entered during the tick. Update during the tree
         * traversal.
         *
         * @property {Integer} _nodeCount
         * @protected
         * @readOnly
         **/
        public int _nodeCount = 0;


        public void Initialize()
        {

        }

        /**
        * Called when entering a node (called by BaseNode).
        * @method _enterNode
        * @param {Object} node The node that called this method.
        * @protected
        **/
        public void _enterNode(BaseNode node)
        {
            this._nodeCount++;
            this._openNodes.Add(node);

            // TODO: call debug here
        }

        /**
         * Callback when opening a node (called by BaseNode).
         * @method _openNode
         * @param {Object} node The node that called this method.
         * @protected
         **/
        public void _openNode(BaseNode node)
        {
            // TODO: call debug here
        }

        /**
        * Callback when ticking a node (called by BaseNode).
        * @method _tickNode
        * @param {Object} node The node that called this method.
        * @protected
        **/
        public void _tickNode(BaseNode node)
        {
            // TODO: call debug here
        }

        /**
         * Callback when closing a node (called by BaseNode).
         * @method _closeNode
         * @param {Object} node The node that called this method.
         * @protected
         **/
        public void _closeNode(BaseNode node)
        {
            // TODO: call debug here
            this._openNodes.RemoveAt(0);
        }

        /**
         * Callback when exiting a node (called by BaseNode).
         * @method _exitNode
         * @param {Object} node The node that called this method.
         * @protected
         **/
        public void _exitNode(BaseNode node)
        {
            // TODO: call debug here
        }

    }
}
